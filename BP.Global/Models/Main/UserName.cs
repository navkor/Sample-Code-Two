namespace BP.Global.Models.Main
{
    public class UserName
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public virtual NameAttribute NameAttribute { get; set; }
        public virtual UserNameType UserNameType { get; set; }
    }
}