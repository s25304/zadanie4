namespace Zadanie4.Model
{
    public class Warehouse
    {
        public int IdWarehouse;
        public string Name;
        public string Address;
        public Warehouse(int idWarehouse, string name, string address)
        {
            IdWarehouse = idWarehouse;
            Name = name;
            Address = address;
        }   
    }
}
