using System.Collections;
using System.Data.SqlClient;
using Zadanie4.Model;

namespace Zadanie4.Repository
{
    public class ProductRepository
    {
        private IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<Product>> getAll()
        {
           await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            await con.OpenAsync();

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from Product";

            var dr = cmd.ExecuteReader();
            var result = new List<Product>();
            while(dr.Read())
            {
                var record = new Product(
                    (int)dr["IdProduct"],
                    dr["Name"].ToString(),
                    dr["Description"].ToString(),
                    (int)dr["Price"]
                    );
                result.Add( record );   
            }
            con.Close();
            return result;
        }
        public async Task<Product> getById(int id)
        {
            await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

             await con.OpenAsync();

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from Product where IdProduct=@IdProduct";
            cmd.Parameters.AddWithValue("@IdProduct", id);

            var dr = cmd.ExecuteReader();

            if (!dr.Read()) return null;

            var result = new Product(
                   (int)dr["IdProduct"],
                   dr["Name"].ToString(),
                   dr["Description"].ToString(),
                   (int)dr["Price"]
                   );

            con.Close();
            return result;
        }
        public async Task<int> create(Product product)
        {
            await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
           await con.OpenAsync();

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Insert Into Product(IdProduct, Name, Description, Price) Values(@IdProduct, @Name, @Description, @Price)";

            cmd.Parameters.AddWithValue("@IdProduct", product.idProduct);
            cmd.Parameters.AddWithValue("@Name", product.name);
            cmd.Parameters.AddWithValue("@Description", product.description);
            cmd.Parameters.AddWithValue("@Price", product.price);
            
            var affectedCount = cmd.ExecuteNonQuery();
            return affectedCount;
        }
        public async Task<int> delete(int id)
        {
            await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
           await con.OpenAsync();

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Delete from Product where IdProduct=@IdProduct";
            cmd.Parameters.AddWithValue("@id", id);

            var affectedCount = cmd.ExecuteNonQuery();
            return affectedCount;
        }
    }
}

