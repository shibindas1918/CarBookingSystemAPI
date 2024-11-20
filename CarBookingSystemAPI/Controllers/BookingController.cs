using CarBookingSystemAPI.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;
        public BookingController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;

        }
        [HttpGet("Bookings")]
        public ActionResult GetPrices(int carId)
        {
            string query = $@"
            SELECT s.Name, sp.Price 
            FROM SupplierPrices sp
            INNER JOIN Suppliers s ON sp.SupplierId = s.SupplierId
            WHERE sp.CarId = {carId}";

            DataTable result = _databaseHelper.ExecuteQuery(query);

            return Ok(result);
        }
    }
}
