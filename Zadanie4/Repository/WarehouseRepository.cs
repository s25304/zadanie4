using System.Data.SqlClient;
using Zadanie4.Model;

namespace Zadanie4.Repository
{
    public class WarehouseRepository
    {
        private IConfiguration _configuration;

        public WarehouseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Warehouse>  getById(int id)
        {
           await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

            await con.OpenAsync();

            await using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from Warehouse where IdProduct=@IdProduct";
            cmd.Parameters.AddWithValue("@IdProduct", id);

            var dr = cmd.ExecuteReader();

            if (!dr.Read()) return null;

            var result = new Warehouse(
                   (int)dr["IdWarehouse"],
                   dr["Name"].ToString(),
                   dr["Address"].ToString()
                   );

            con.Close();
            return result;
        }
    }
}
