namespace BP.Global.Models.Main
{
    public class StringTable
    {
        public int ID { get; private set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public virtual StreetAttribute StreetAttibute { get; set; }
    }
}