using CarBookingSystemAPI.DataAccess;
using CarBookingSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;

namespace CarBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {

        private readonly DatabaseHelper _databaseHelper;
        public SupplierController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }


        [HttpGet("supplier")]
        public ActionResult Getsupplier()
        {
            string query = "Select *from suppliers";
            var sup = new List<Dictionary<string, object>>();
            try
            {
                DataTable result = _databaseHelper.ExecuteQuery(query);

                foreach (DataRow row in result.Rows)
                {
                    var supcol = new Dictionary<string, object>();
                    foreach (DataColumn column in result.Columns)
                    {
                        supcol[column.ColumnName] = row[column];
                    }
                    sup.Add(supcol);
                }
                return Ok(sup);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public ActionResult CreateSupplier(Supplier supplier)
        {
            if (supplier == null || string.IsNullOrEmpty(supplier.Name) || string.IsNullOrEmpty(supplier.Contact) || string.IsNullOrEmpty(supplier.Address))
            {
                return BadRequest();
            }

            string query = $@"
            INSERT INTO suppliers (Name, Contact,address) 
            VALUES ('{supplier.Name}', '{supplier.Contact}', '{supplier.Address}')";


            try
            {
                _databaseHelper.ExecuteQuery(query);
                return Ok(new
                {
                    message = "added sucessfully ",
                    data = supplier
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete]
        public ActionResult DeleteSupplier(int id)
        {
            if (id == null) { return BadRequest(); }
            string deleteQuery = $@"delete from Suppliers where supplierId = {id} ";
            try
            {
                _databaseHelper.ExecuteQuery(deleteQuery);
                return Ok(new { message = "the suppiler has been removed ", data = id });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal sever error:{ex.Message}");

            }
        }

        [HttpPut("updateed")]
        public ActionResult UpdateSupplier(Supplier supplier)
        {
            if (supplier == null || string.IsNullOrEmpty(supplier.Name) || string.IsNullOrEmpty(supplier.Contact) || string.IsNullOrEmpty(supplier.Address))
            {
                return BadRequest();

            }
            string query = $@"update Suppliers set name = '{supplier.Name}' ,contact='{supplier.Contact}', address= '{supplier.Address}' where supplierid = '{supplier.SupplierId}'";
           
            try
            {
                _databaseHelper.ExecuteQuery(query);
                return Ok(new { message = "The suppiler details has been updated ", data = supplier});

            }catch(Exception ex)
            {
                return StatusCode(500, $"Internal sever error:{ex.Message}");
            }
        }

    }
}
    