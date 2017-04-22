namespace BP.Global.Models.Main
{
    public class City
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public virtual District District { get; set; }
        public virtual PostalCode PostalCode { get; set; }
    }
}