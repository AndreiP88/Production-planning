namespace data
{
    public class Equips
    {
        public int ID;
        public int AreaID;
        public int TemplateID;
        public string Name;
        public int Code;
        public string Commissioned;
        public string Decommissioned;
        public int Sort;

        public Equips(int id, int areaID, int templateID, string name, int code, string comm, string decomm, int sort)
        {
            ID = id;
            AreaID = areaID;
            TemplateID = templateID;
            Name = name;
            Commissioned = comm;
            Decommissioned = decomm;
            Sort = sort;
        }
    }
}