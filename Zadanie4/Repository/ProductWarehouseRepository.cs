using System.Data.SqlClient;
using Zadanie4.Model;

namespace Zadanie4.Repository
{
    public class ProductWarehouseRepository
    {
        private IConfiguration _configuration;

        public ProductWarehouseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ProductWarehouse> getById(int id)
        {
           await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

            await con.OpenAsync();

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from ProductWarehouse where IdProduct=@IdProduct";
            cmd.Parameters.AddWithValue("@IdProduct", id);

            var dr = cmd.ExecuteReader();

            if (!dr.Read()) return null;

            var result = new ProductWarehouse(
                   (int)dr["IdProductWarehouse"],
                   (int)dr["IdWarehouse"],
                   (int)dr["IdProduct"],
                   (int)dr["IdOrder"],
                   (int)dr["Amount"],
                   (int)dr["Price"],
                   DateTime.Parse(dr["CreatedAt"].ToString())
                  );

            con.Close();
            return result;
        }

        public async Task<ProductWarehouse> getByIdOrder(int idOrder)
        {
           await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

            await con.OpenAsync();

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from ProductWarehouse where IdOrder=@IdProduct";
            cmd.Parameters.AddWithValue("@IdOrder", idOrder);

            var dr = cmd.ExecuteReader();

            if (!dr.Read()) return null;

            var result = new ProductWarehouse(
                   (int)dr["IdProductWarehouse"],
                   (int)dr["IdWarehouse"],
                   (int)dr["IdProduct"],
                   (int)dr["IdOrder"],
                   (int)dr["Amount"],
                   (int)dr["Price"],
                   DateTime.Parse(dr["CreatedAt"].ToString())
                  );

            con.Close();
            return result;
        }

        public async Task<int> create(ProductWarehouse productWarehouse)
        {
           await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            await con.OpenAsync();

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Insert Into ProductWarehouse(IdProductWarehouse, IdWarehouse, IdProduct, IdOrder,  Amount, Price, createdAt )" +
                " Values(@IdProductWarehouse ,@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, Date.now())";

            cmd.Parameters.AddWithValue("@IdProductWarehouse", productWarehouse.IdProductWarehouse);
            cmd.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
            cmd.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
            cmd.Parameters.AddWithValue("@IdOrder", productWarehouse.IdOrder);
            cmd.Parameters.AddWithValue("@Amount", productWarehouse.Amount);
            cmd.Parameters.AddWithValue("@Price", productWarehouse.Price);
            

            var affectedCount = cmd.ExecuteNonQuery();
            return affectedCount;
        }
      
    }
}
