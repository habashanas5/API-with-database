using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly String connectionString;

        public ProductsController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO products " +
                        "(Name, Brand, Category, Price, Description) VALUES " +
                        "(@Name, @Brand, @Category, @Price, @Description)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", productDto.Name);
                        command.Parameters.AddWithValue("@Brand", productDto.Brand);
                        command.Parameters.AddWithValue("@Category", productDto.Category);
                        command.Parameters.AddWithValue("@Price", productDto.Price);
                        command.Parameters.AddWithValue("@Description", productDto.Description);
                        command.ExecuteNonQuery();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("Product", "Failed to create the product.");
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM products";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product();
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreateAt = reader.GetDateTime(6);

                                products.Add(product);
                            }
                        }
                    }

                }
                return Ok(products);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("Product", "Failed to create the product.");
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            Product product = new Product();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM products WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreateAt = reader.GetDateTime(6);

                            }

                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
                return Ok(product);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("Product", "Failed to create the product.");
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE products SET Name=@Name, Brand=@Brand, Category=@Category, " +
                                 "Price=@Price, Description=@Description WHERE Id=@Id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", productDto.Name);
                        command.Parameters.AddWithValue("@Brand", productDto.Brand);
                        command.Parameters.AddWithValue("@Category", productDto.Category);
                        command.Parameters.AddWithValue("@Price", productDto.Price);
                        command.Parameters.AddWithValue("@Description", productDto.Description);
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("Product", "Failed to update the product.");
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM products WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("Product", "Failed to update the product.");
                return BadRequest(ModelState);
            }
        }

    }
}   
