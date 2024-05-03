namespace Zadanie4.Model
{
    public class ProductWarehouse
    {
        public int IdProductWarehouse;
        public int IdWarehouse;
        public int IdProduct;
        public int IdOrder;
        public int Amount;
        public int Price;
        public DateTime createdAt;

        public ProductWarehouse(int IdProductWarehouse, int IdWarehouse, int IdProduct, int IdOrder,int Amount, int Price, DateTime createdAt)
        {
            this.IdProductWarehouse = IdProductWarehouse;   
            this.IdWarehouse = IdWarehouse;
            this.IdProduct = IdProduct;
            this.IdOrder = IdOrder;
            this.Amount= Amount;
            this.Price = Price;
            this.createdAt= createdAt;
        }
    }
}
