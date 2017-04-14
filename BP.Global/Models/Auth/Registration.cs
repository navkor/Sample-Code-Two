namespace BP.Auth
{
    public class Registration
    {
        public int ID { get; private set; }
        public virtual IdentityAttribute IdentityAttribute { get; set; }
        public virtual ProfileAttribute ProfileAttribute { get; set; }
        public virtual AccountAttribute AccountAttribute { get; set; }
        public virtual LoginAttribute LoginAttribute { get; set; }
    }
}
