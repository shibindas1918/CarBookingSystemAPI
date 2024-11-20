﻿using CarBookingSystemAPI.DataAccess;
using CarBookingSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CarBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;
        public CarController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }
        [HttpGet("Car")]
        public ActionResult GetCars()
        {
            string query = "Select *from cars";
           
           
                try
                {
                    DataTable result = _databaseHelper.ExecuteQuery(query);
                    var cars = new List<Dictionary<string, object>>();

                    foreach (DataRow row in result.Rows)
                    {
                        var car1 = new Dictionary<string, object>();
                        foreach (DataColumn column in result.Columns)
                        {
                            car1[column.ColumnName] = row[column];
                        }
                        cars.Add(car1);

                    }
                    return Ok(cars);
                }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }


        }
        [HttpGet("Search")]
        public ActionResult SearchCars( decimal? minPrice, decimal? maxPrice)
        {
            string query = @"
                            SELECT c.*, s.Name AS SupplierName 
                            FROM Cars c 
                            JOIN Suppliers s ON c.SupplierId = s.SupplierId
                            WHERE 1 = 1"; 

            // Add conditions based on provided price parameters
            if (minPrice.HasValue)
            {
                query += $" AND c.Price >= {minPrice.Value}";
            }
            if (maxPrice.HasValue)
            {
                query += $" AND c.Price <= {maxPrice.Value}";
            }


            // Call the DatabaseHelper to execute the query

            try
            {
                DataTable result = _databaseHelper.ExecuteQuery(query);
                var cars = new List<Dictionary<string, object>>();

                foreach (DataRow row in result.Rows)
                {
                    var car1 = new Dictionary<string, object>();
                    foreach (DataColumn column in result.Columns)
                    {
                        car1[column.ColumnName] = row[column];
                    }
                    cars.Add(car1);

                }
                return Ok(cars);
            }catch (Exception ex)
            {
                return StatusCode(500 ,$"internal sever error {ex.Message}");
            }

            
        }

        [HttpPost]
        public ActionResult CreateCar (Car car)
        {
            if(car==null || string.IsNullOrEmpty(car.CarModel) || string.IsNullOrEmpty(car.Category))
            {
                return BadRequest();
            }
    
                string query = $@"INSERT INTO cars (CarModel, Category,supplierid,price) 
                               VALUES ('{car.CarModel}', '{car.Category}','{car.supplierid}','{car.price}')";

           
            try
            {
                _databaseHelper.ExecuteQuery(query);
                return Ok("Supplier added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        public ActionResult Deletecars(int id)
        {
            if (id == null) { return BadRequest(); }
            string deleteQuery = $@"delete from cars where carId = {id} ";
            try
            {
                _databaseHelper.ExecuteQuery(deleteQuery);
                return Ok(new { message = "the car has been removed ", data = id });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal sever error:{ex.Message}");

            }
        }

        [HttpPut]
        public ActionResult UpdateSupplier(Car cars)
        {
            if (cars == null || string.IsNullOrEmpty(cars.CarModel) || string.IsNullOrEmpty(cars.Category))
            {
                return BadRequest();

            }
            string query = $@"update cars set CarModel = '{cars.CarModel}' ,Category='{cars.Category}', supplierid= '{cars.supplierid}' where carid = '{cars.CarId}'";
          
                try
                {
                    _databaseHelper.ExecuteQuery(query);
                    return Ok(new { message = "The suppiler details has been updated ", data = cars });

                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal sever error:{ex.Message}");
                }
        }
    }
}