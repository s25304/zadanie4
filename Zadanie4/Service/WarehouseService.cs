using Microsoft.AspNetCore.Mvc.Formatters;
using System.Data.Common;
using System.Data.SqlClient;
using Zadanie4.DTO;
using Zadanie4.Model;
using Zadanie4.Repository;

namespace Zadanie4.Service
{
    public class WarehouseService
    {
        private ProductRepository productRepository;
        private WarehouseRepository warehouseRepository;
        private OrderRepository orderRepository;
        private ProductWarehouseRepository productWarehouseRepository;
        private IConfiguration _configuration;

        
        public  WarehouseService(ProductRepository productRepository, WarehouseRepository warehouseRepository, OrderRepository orderRepository ,ProductWarehouseRepository productWarehouseRepository, IConfiguration _configuration)
        {
            this.productRepository = productRepository;
            this.warehouseRepository = warehouseRepository;
            this.orderRepository= orderRepository;
            this.productWarehouseRepository= productWarehouseRepository;
            this._configuration= _configuration;
        }
      public async void addByProcedure(WarehouseProductDto dto)
        {
            await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

            await con.OpenAsync();

            await using var cmd = new SqlCommand("AddProductToWarehouse",con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
            cmd.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
            cmd.Parameters.AddWithValue("@Amount", dto.Amount);
            cmd.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<int> addProudctToWarehouse(WarehouseProductDto source)
        {
            validate(source);
            var order = await orderRepository.getByIdProductAndAmount(source.IdProduct, source.Amount);
            if (order == null)
            {
                throw new Exception();
            }

            var warehouseProduct = await productWarehouseRepository.getByIdOrder(order.IdOrder);
            if (warehouseProduct != null)
            {
                throw new Exception();
            }

            var product = await productRepository.getById(source.IdProduct);

            var result = new ProductWarehouse(
                order.IdProduct,
                source.IdWarehouse,
                source.IdProduct,
                order.IdOrder,
                source.Amount,
                (source.Amount * product.price),
                source.CreatedAt
                );

            addProductToWarehouse(result, order.IdOrder);

            return result.IdProductWarehouse;
        }


        private async void addProductToWarehouse(ProductWarehouse src, int IdOrder)
        {   //transactional
            await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

            await using var cmd = new SqlCommand("update  Order  set fulfilledAt = Date.now() where IdOrder=@IdOrder", con);
            cmd.Parameters.AddWithValue("@IdOrder", IdOrder);

            await con.OpenAsync();
            DbTransaction tran = await con.BeginTransactionAsync();
            cmd.Transaction = (SqlTransaction)tran;

            try
            {
                await cmd.ExecuteNonQueryAsync();
                cmd.Parameters.Clear();

                cmd.CommandText = "Insert Into ProductWarehouse(IdProductWarehouse, IdWarehouse, IdProduct, IdOrder,  Amount, Price, createdAt )" +
                " Values(@IdProductWarehouse ,@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, Date.now())";

                cmd.Parameters.AddWithValue("@IdProductWarehouse", src.IdProductWarehouse);
                cmd.Parameters.AddWithValue("@IdWarehouse", src.IdWarehouse);
                cmd.Parameters.AddWithValue("@IdProduct", src.IdProduct);
                cmd.Parameters.AddWithValue("@IdOrder", src.IdOrder);
                cmd.Parameters.AddWithValue("@Amount", src.Amount);
                cmd.Parameters.AddWithValue("@Price", src.Price);

                await cmd.ExecuteReaderAsync();

                await tran.CommitAsync();
            } catch(Exception ex)
            {
                await tran.RollbackAsync();
                throw new Exception(ex);
            }
            
        }
        private void validate(WarehouseProductDto src)
        {
            if (productRepository.getById(src.IdProduct) == null)
            {
                throw new Exception();
            }
            if (warehouseRepository.getById(src.IdWarehouse) == null)
            {
                throw new Exception();
            }
            if (src.Amount <= 0)
            {
                throw new Exception();
            }
        }
    }
}
