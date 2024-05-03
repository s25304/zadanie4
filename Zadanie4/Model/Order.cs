namespace Zadanie4.Model
{
    public class Order
    {
        public int IdOrder;
        public int IdProduct;
        public int Amount;
        public DateTime CreatedAt;
        public DateTime FulfilledAt;

        public Order(int IdOrder, int IdProduct, int Amount, DateTime CreatedAt, DateTime FulfilledAt) {
            this.IdOrder = IdOrder;      
            this.IdProduct = IdProduct; 
            this.Amount = Amount;
            this.CreatedAt = CreatedAt;
            this.FulfilledAt= FulfilledAt;
        }
    }
   
}
