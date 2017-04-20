namespace BP.Global.Models.Main
{
    public class NameFormatMap
    {
        public int ID { get; private set; }
        public int DisplayOrder { get; set; }
        public virtual UserNameType UserNameType { get; set; }
    }
}