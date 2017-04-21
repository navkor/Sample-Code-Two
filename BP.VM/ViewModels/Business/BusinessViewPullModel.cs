namespace BP.VM.ViewModels.Business
{
    public class BusinessViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Logins { get; set; }
        public NameIdLists BusinessType { get; set; }
    }
}
