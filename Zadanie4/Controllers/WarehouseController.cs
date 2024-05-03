using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Zadanie4.DTO;
using Zadanie4.Service;

namespace Zadanie4.Controllers
{
    [ApiController]
    [Route("/api/WarehouseProduct")]
    public class WarehouseController : ControllerBase
    {
        private WarehouseService service;

        public WarehouseController(WarehouseService service)
        {
            this.service = service;
        }

     

    [HttpPost("/add")]
        public  IActionResult addUser([FromBody] WarehouseProductDto source)
        {
            try
            {
                return Ok(service.addProudctToWarehouse(source));

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPost("/addByProcedure")]
        public IActionResult addUser2([FromBody] WarehouseProductDto source)
        {
            try
            {
                service.addByProcedure(source);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}