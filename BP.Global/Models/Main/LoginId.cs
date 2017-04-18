namespace BP.Global.Models.Main
{
    public class LoginId
    {
        public int ID { get; private set; }
        public string UserId { get; set; }
        public virtual LoginIdType LoginIdType { get; set; }
        public virtual ProfileAttribute ProfileAttribute { get; set; }
        public virtual PreferenceAttribute PreferenceAttribute { get; set; }
    }
}