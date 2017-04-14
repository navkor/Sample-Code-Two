namespace BP.Auth
{
    public class UserName
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public virtual IdentityAttribute IdentityAttribute { get; set; }
        public virtual NameType NameType { get; set; }
    }
}