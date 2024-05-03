using System.Data.SqlClient;
using Zadanie4.Model;

namespace Zadanie4.Repository
{
    public class OrderRepository
    {
        private IConfiguration _configuration;

        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Order> getById(int id)
        {
            await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

            await con.OpenAsync();

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from Order where IdProduct=@IdProduct";
            cmd.Parameters.AddWithValue("@IdProduct", id);

            var dr = cmd.ExecuteReader();

            if (!dr.Read()) return null;

            var result = new Order(
                   (int)dr["IdOrder"],
                   (int)dr["IdProduct"],
                   (int)dr["Amount"],
                   DateTime.Parse(dr["CreatedAt"].ToString()),
                   DateTime.Parse(dr["FulfilledAt"].ToString())
                   );

            con.Close();
            return result;
        }
        public async Task<Order> getByIdProductAndAmount(int ProductId, int Amount)
        {
           await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

            await con.OpenAsync();

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * Order  where IdProduct=@IdProduct and Amount =@Amount";
            cmd.Parameters.AddWithValue("@IdProduct", ProductId);
            cmd.Parameters.AddWithValue("@Amount", Amount);

            var dr = cmd.ExecuteReader();

            if (!dr.Read()) return null;

            var result = new Order(
                   (int)dr["IdOrder"],
                   (int)dr["IdProduct"],
                   (int)dr["Amount"],
                   DateTime.Parse(dr["CreatedAt"].ToString()),
                   DateTime.Parse(dr["FulfilledAt"].ToString())
                   );

            con.Close();
            return result;
        }
        public async void  updateFulfilledColumn (int orderId)
        {
            await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

             con.Open();

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "update  Order  set fulfilledAt = Date.now() where IdOrder=@IdOrder";
            cmd.Parameters.AddWithValue("@IdOrder", orderId);

            var dr = cmd.ExecuteNonQuery();

            con.Close();
            
        }
    }
}

