using Microsoft.Win32.SafeHandles;
using BP.Auth;
using BP.VM.PullModels.Authentication;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BP.Service.Services.Authentication
{
    public class AccountService : IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;
        private Random random = new Random();
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        /*
            This particular function will format the Registration class so that a person who has passed all other validation can register for an account.
            This is handled asynchronously of course
        */
        public async Task<Registration> RegisterNewStandardAccount(RegisterAccount pullModel, BPAuthContext context)
        {
            // they are registering a new account, so let's go ahead and create this account
            var registrationType = await context.RegistrationTypes.FirstOrDefaultAsync(x => x.Index == 300);
           
            var nameType = await context.NameTypes.FirstOrDefaultAsync(x => x.Index == 1000);
           
            var role = await context.Roles.FirstOrDefaultAsync(x => x.Index == 300);
         
            var salt = ""; var validationCode = "";
            using (var encrypt = new EncryptionHandler())
            {
                salt = encrypt.RandomString(random.Next(8, 15));
                validationCode = encrypt.RandomString(random.Next(8, 12));
            }
            var userNameList = new ConcurrentBag<UserName> {
                new UserName {
                    Name = pullModel.Email,
                    NameType = nameType
                }
            };
            var createdDateType = await context.ProfileDateTypes.FirstOrDefaultAsync(x => x.Index == 10);
     
            var passwordSetDateType = await context.ProfileDateTypes.FirstOrDefaultAsync(x => x.Index == 11);

            var identityDatesList = new ConcurrentBag<DateTable> {
                new DateTable {
                    DateLine = DateTimeOffset.Now,
                    DateType = createdDateType
                },
                new DateTable {
                    DateLine = DateTimeOffset.Now,
                    DateType = passwordSetDateType
                }
            };
            var encryptedPassword = EncryptRegistrationPassword(pullModel.Password, salt);
            var identityAttribute = new IdentityAttribute {
                UserNames = userNameList.ToList(),
                Password = encryptedPassword,
                Salt = salt,
                IdentityDates = identityDatesList.ToList()
            }; identityDatesList = new ConcurrentBag<DateTable> {
                new DateTable {
                    DateLine = DateTimeOffset.Now,
                    DateType = createdDateType
                },
            };
            var emailList = new ConcurrentBag<EmailAddress> {
                new EmailAddress {
                    Email = pullModel.Email,
                    ValidationString = validationCode,
                    EmailDates = identityDatesList.ToList(),
                    Default = true,
                    Validated = false
                }
            };
            var profileAttribute = new ProfileAttribute {
                EmailAddresses = emailList.ToList()
            };
            var accountAttribute = new AccountAttribute {
                AccountDates = identityDatesList.ToList(),
                RegistrationType = registrationType,
                UserRole = role
            };
            return new Registration {
                IdentityAttribute = identityAttribute,
                ProfileAttribute = profileAttribute,
                AccountAttribute = accountAttribute
            };
        }

        public async Task<MethodResults> ActivateAccount(ActivateEmailModel model, BPAuthContext context)
        {
            var methodResults = new MethodResults { Success = false, ID = model.ID };

            // the first thing we need to do is go get the account, if it exists
            var emailAddress = new EmailAddress();
            emailAddress = await context.EmailAddresses.FirstOrDefaultAsync(x => x.ValidationString.Equals(model.ActivateString));
            if (emailAddress == null) return methodResults;
            var registration = new Registration();
            registration = await context.Registrations.FindAsync(model.ID);
            if (registration == null) return methodResults;
            if (registration.ProfileAttribute.EmailAddresses.Any(x => x.ID == emailAddress.ID))
            {
                // this email address belongs to this registration
                // so let's activate this account
                var emailEntry = context.Entry(emailAddress);
                emailEntry.Property(nameof(EmailAddress.Validated)).CurrentValue = true;
                emailEntry.Property(nameof(EmailAddress.ValidationString)).CurrentValue = "done";
                var verifiedDate = await context.ProfileDateTypes.FirstOrDefaultAsync(x => x.Index == 120);
                var dateTable = new DateTable
                {
                    DateLine = DateTimeOffset.Now,
                    DateType = verifiedDate
                };
                emailEntry.Entity.EmailDates.Add(dateTable);
                emailEntry.State = EntityState.Modified;
                methodResults.Success = true;
                methodResults.Message = emailAddress.Email;
                methodResults.ID = registration.ID;
                await context.SaveChangesAsync();
            }

            return methodResults;
        }

        /*
            This particular function will take a request to log in, and return whether that username/email address belongs to an account, and whether that account is in "able to log in" status or not. 
        */
        public async Task<Registration> AccountLoginStepOne(LoginAccountStep1 pullModel, BPAuthContext context)
        {
            var registration = new Registration();
            // at this point, we are going to check the username/email address of the 
            // database to see if any of them match what we're looking at here
            registration = await CanLoginWithEmail(pullModel.Email, context);
            // find out if this registration has any dates associated with it.
            if (registration.ID > 0)
            {
                // this is not a new registration
                var twoDaysAgo = DateTimeOffset.Now.AddHours(-48);
                // first, let's see if it's been locked in the past 48 hours
                var dateList = registration?.LoginAttribute?.LoginDates?.Where(x => x.DateLine > twoDaysAgo && x.DateType.Index == 110)?.OrderBy(y => y.DateLine) ?? new List<DateTable>().AsEnumerable();
                if (dateList.Count() > 0)
                {
                    TimeSpan difference = new TimeSpan();
                    // there has been at least one lockout in the past 48 hours
                    var firstLock = dateList.FirstOrDefault().DateLine;
                    difference = DateTimeOffset.Now - firstLock;
                    if (difference.TotalMinutes < StaticClasses.TwentyFourHourWait)
                    {
                        var totalMinutes = difference.TotalMinutes;
                        // it's been less than 24 hours since the most recent lockout
                        twoDaysAgo = firstLock.AddMinutes(-1441);
                        // if the user has had the maximum allow lockouts in the previous 24 hour period
                        var lockoutCount = dateList.Count(y => y.DateLine > twoDaysAgo);
                        if (lockoutCount > StaticClasses.MaxLockoutsIn24Hours) return new Registration();
                        // they obviously haven't or we wouldn't be here
                        // so let's find out if they've waited long enough from the previous lockout
                        if (lockoutCount == StaticClasses.MaxLockoutsIn24Hours)
                        {
                            if (difference.TotalMinutes < StaticClasses.ThirdTimeLockout)
                                return new Registration();
                        }
                        if (lockoutCount == (StaticClasses.MaxLockoutsIn24Hours - 1))
                        {
                            if (difference.TotalMinutes < StaticClasses.SecondTimeLockout)
                                return new Registration();
                        }
                        if (difference.TotalMinutes < StaticClasses.FirstTimeLockout)
                            return new Registration();
                    }
                }
            }
            return registration;
        }

        public async Task<Registration> AccountLoginStepTwo(LoginAccountStep2 pullModel, string salt, BPAuthContext context)
        {
            var registration = new Registration();

            registration = await CanLoginWithEmail(pullModel.Email, context).ConfigureAwait(continueOnCapturedContext: false); 
            if (registration.ID > 0)
            {
                var registerEntry = context.Entry(registration);
                // this account has been checked and can be logged in
                // so now, check against the DB is the password is the same
                // the salt will be passed from the ViewBag
                var encryptedPassword = EncryptRegistrationPassword(pullModel.Password, salt);
                // now check that this password matched the assigned password for this account
                if (registration.IdentityAttribute.Password.Equals(encryptedPassword))
                {
                    // log the login
                    var dateType = context.ProfileDateTypes.FirstOrDefault(x => x.Index == 300);
                    var dateTable = new DateTable {
                        DateLine = DateTimeOffset.Now,
                        DateType = dateType
                    };
                    // check to see if this person has ever logged in before
                    if (registerEntry.Entity.LoginAttribute != null)
                    {
                        registerEntry.Entity.LoginAttribute.LoginDates.Add(dateTable);
                    } else
                    {
                        var loginDates = new List<DateTable>();
                        loginDates.Add(dateTable);
                        var loginAttribute = new LoginAttribute {
                            LoginDates = loginDates
                        };
                        registerEntry.Entity.LoginAttribute = loginAttribute;
                    }
                    registerEntry.State = EntityState.Modified;
                    await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false); 
                    return registration;
                } else
                {
                    // this person did not log in correctly
                    // count the number of times this has happened since the last lock
                    var twoDaysAgo = DateTimeOffset.Now.AddHours(-48);
                    var dateType = context.ProfileDateTypes.FirstOrDefault(x => x.Index == 200);
                    if (registration.LoginAttribute.LoginDates.Any(y => y.DateType.Index == 110 && y.DateLine > twoDaysAgo))
                    {
                        // there is at least one lockout that has happened
                        twoDaysAgo = DateTimeOffset.Now.AddMinutes(-1441);
                        if (registration.LoginAttribute.LoginDates.Any(y => y.DateType.Index == 110 && y.DateLine > twoDaysAgo))
                        {
                            // one of those has been in the past 24 hours
                            // so now we have to get the top one
                            var lastLocked = registration.LoginAttribute.LoginDates.OrderByDescending(y => y.DateLine).FirstOrDefault(x => x.DateType.Index == 110 && x.DateLine > twoDaysAgo).DateLine;
                            // now we have to count the number of failed since this
                            var failedCount = registration.LoginAttribute.LoginDates.Count(y => y.DateType.Index == 200 && y.DateLine > lastLocked);
                            if (failedCount == StaticClasses.MaxFailedLogins)
                            {
                                // it's time to lock the account and return a blank registration
                                dateType = context.ProfileDateTypes.FirstOrDefault(x => x.Index == 110);
                            }
                        }
                    } else
                    {
                        // it's never been locked before, let's see if they need to be locked now
                        twoDaysAgo = DateTimeOffset.Now.AddMinutes(-1441);
                        if (registration.LoginAttribute.LoginDates.Count(y => y.DateType.Index == 200 && y.DateLine > twoDaysAgo) >= StaticClasses.MaxFailedLogins)
                        {
                            dateType = context.ProfileDateTypes.FirstOrDefault(x => x.Index == 110);
                        }
                    }
                    // we need to log the failure and return a blank registration
                    var dateTable = new DateTable
                    {
                        DateLine = DateTimeOffset.Now,
                        DateType = dateType
                    };
                    registerEntry.Entity.LoginAttribute.LoginDates.Add(dateTable);
                    registerEntry.State = EntityState.Modified;
                    await context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);
                    return new Registration();
                }
            }
            return registration;
        }

        public async Task<Registration> CanLoginWithEmail(string userName, BPAuthContext context)
        {
            var returnValue = new Registration();

            // first, let's go get the email entity
            var email = await context.EmailAddresses.FirstOrDefaultAsync(x => x.Email.Equals(userName)).ConfigureAwait(continueOnCapturedContext: false); 
            if (email != null)
            {
                if (email.Validated)
                    returnValue = email.ProfileAttribute.Registration;
            }
            else
            {
                // this email doesn't fit any of the email address
                // so let's look at user names
                var userNames = context.UserNames.Where(x => x.NameType.Index == 1000);
                if (userNames.Any(x => x.Name.Equals(userName)))
                {
                    var theUserName = await userNames.FirstOrDefaultAsync(x => x.Name.Equals(userName)).ConfigureAwait(continueOnCapturedContext: false); 
                    returnValue = theUserName.IdentityAttribute.Registration;
                }
                else
                {
                    return new Registration();
                }
            }

            return returnValue;
        }
        
        
        public string EncryptRegistrationPassword(string password, string salt)
        {
            using (var encrypt = new EncryptionHandler())
            {
                return encrypt.SetPassword(password, salt);
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }
    }
}
