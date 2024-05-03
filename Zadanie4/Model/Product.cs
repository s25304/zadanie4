namespace Zadanie4.Model
{
    public class Product
    {
        public int idProduct;
        public string name;
        public string description;
        public int price;
            
        public Product(int idProduct, string name, string description, int price)
        {
            this.idProduct = idProduct;
            this.name = name;
            this.description = description;
            this.price = price;
        }
    }
}
