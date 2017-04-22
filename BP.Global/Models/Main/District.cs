namespace BP.Global.Models.Main
{
    public class District
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public virtual Country Country { get; set; }
        public virtual FormName DistrictFormName { get; set; }
    }
}