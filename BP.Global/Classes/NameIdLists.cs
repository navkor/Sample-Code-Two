namespace BP
{
    public class NameIdLists
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class NameIdValueLists : NameIdLists
    {
        public string Value { get; set; }
    }

    public class NameStringId
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class NameStringIdSelected : NameStringId
    {
        public bool Selected { get; set; }
    }

    public class NameIdSelectedLists : NameIdLists
    {
        public bool Selected { get; set; }
    }

    public class NameIdValueNoteLists : NameIdValueLists
    {
        public string Note { get; set; }
        public int Index { get; set; }
    }

    public class IdIndexLists
    {
        public int ID { get; set; }
        public int Index { get; set; }
    }

    public class IdIndexValueLists : IdIndexLists
    {
        public string Value { get; set; }
    }

    public class NameIdSelectedValueLists : NameIdSelectedLists
    {
        public string Value { get; set; }
    }
}
