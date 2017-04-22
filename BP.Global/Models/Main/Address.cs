namespace BP.Global.Models.Main
{
    public class Address
    {
        public int ID { get; private set; }
        public string NickName { get; set; }
        public virtual StreetAttribute StreetAttribute { get; set; }
        public virtual AddressCityAttribute AddressCityAttribute { get; set; }
        public virtual AddressType AddressType { get; set; }
        public virtual BusinessAttribute BusinessAttribute { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
