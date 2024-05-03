using Microsoft.AspNetCore.Mvc.Formatters;
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
            orderRepository.updateFulfilledColumn(order.IdOrder);

            var result = new ProductWarehouse(
                order.IdProduct,
                source.IdWarehouse,
                source.IdProduct,
                order.IdOrder,
                source.Amount,
                (source.Amount * product.price),
                source.CreatedAt
                );
            productWarehouseRepository.create(result);

            return result.IdProductWarehouse;
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
