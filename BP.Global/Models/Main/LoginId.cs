namespace BP.Global.Models.Main
{
    public class LoginId
    {
        public int ID { get; private set; }
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public virtual LoginAttribute LoginAttribute { get; set; }
        public virtual PresidenceType PresidenceType { get; set; }
        public virtual LoginIdType LoginIdType { get; set; }
        public virtual ProfileAttribute ProfileAttribute { get; set; }
        public virtual LoginPreferenceAttribute PreferenceAttribute { get; set; }
    }
}