using ECommerce.API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ECommerce.API.Data
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration configuration;
        private readonly string dbConnection;
        private readonly string dateFormat;
        

        public DataAccess(IConfiguration configuration)
        {
            this.configuration = configuration;
            dbConnection = this.configuration.GetConnectionString("ECommerceDbConnectionString");
            dateFormat = this.configuration["Constants:DateFormat"];
        }

        public Cart GetActiveCartOfUser(int userid)
        {
            var cart = new Cart();
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();

                string query = "SELECT COUNT(*) From Carts WHERE UserId=" + userid + " AND Ordered='false';";
                command.CommandText = query;

                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    return cart;
                }

                query = "SELECT CartId From Carts WHERE UserId=" + userid + " AND Ordered='false';";
                command.CommandText = query;

                int cartid = (int)command.ExecuteScalar();

                query = "select * from CartItems WHERE CartId=" + cartid + ";";
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CartItems item = new()
                    {
                        Id = (int)reader["CartItemId"],
                        Product = GetProduct((int)reader["ProductId"])
                    };
                    cart.CartItems.Add(item);
                }

                cart.Id = cartid;
                cart.User = GetUser(userid);
                cart.Ordered = false;
                cart.OrderedOn = "";
            }
            return cart;
        }

        public List<Cart> GetAllPreviousCartsOfUser(int userid)
        {
            var carts = new List<Cart>();
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "SELECT CartId FROM Carts WHERE UserId=" + userid + " AND Ordered='true';";
                command.CommandText = query;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var cartid = (int)reader["CartId"];
                    carts.Add(GetCart(cartid));
                }
            }
            return carts;
        }

        public Cart GetCart(int cartid)
        {
            var cart = new Cart();
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();

                string query = "SELECT * FROM CartItems WHERE CartId=" + cartid + ";";
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CartItems item = new()
                    {
                        Id = (int)reader["CartItemId"],
                        Product = GetProduct((int)reader["ProductId"])
                    };
                    cart.CartItems.Add(item);
                }
                reader.Close();

                query = "SELECT * FROM Carts WHERE CartId=" + cartid + ";";
                command.CommandText = query;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cart.Id = cartid;
                    cart.User = GetUser((int)reader["UserId"]);
                    cart.Ordered = bool.Parse((string)reader["Ordered"]);
                    cart.OrderedOn = (string)reader["OrderedOn"];
                }
                reader.Close();
            }
            return cart;
        }

        public Offer GetOffer(int id)
        {
            var offer = new Offer();
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                connection.Open();

                string query = $"SELECT * FROM Offers WHERE OfferId = {id}";
                using SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    offer.Id = (int)reader["OfferId"];
                    offer.Title = (string)reader["Title"];
                    offer.Discount = (int)reader["Discount"];
                }
            }
            return offer;
        }

        public List<PaymentMethod> GetPaymentMethods()
        {
            var result = new List<PaymentMethod>();
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM PaymentMethods;";
                command.CommandText = query;

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PaymentMethod paymentMethod = new()
                    {
                        Id = (int)reader["PaymentMethodId"],
                        Type = (string)reader["Type"],
                        Provider = (string)reader["Provider"],
                        Available = bool.Parse((string)reader["Available"]),
                        Reason = (string)reader["Reason"]
                    };
                    result.Add(paymentMethod);
                }
            }
            return result;
        }

        public List<ProductCategory> GetProductCategories()
        {
            var productCategories = new List<ProductCategory>();
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                connection.Open();

                string query = "SELECT * FROM ProductCategories";
                using SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var category = new ProductCategory
                    {
                        Id = (int)reader["CategoryId"],
                        Category = (string)reader["Category"],
                        SubCategory = (string)reader["SubCategory"]
                    };
                    productCategories.Add(category);
                }
            }
            return productCategories;
        }

        public Product GetMinProduct(string category, string subcategory)
        {
            var product = new Product();
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                connection.Open();

                string query = @"SELECT p.ProductId, p.Title, p.description_, p.Price, p.Quantity, p.ImageName, pc.Category, MIN(p.Price - (p.Price * (o.Discount * 0.01))) AS LowestPriceWithDiscount
                                FROM Products p
                                INNER JOIN Offers o ON p.OfferId = o.OfferId
                                INNER JOIN ProductCategories pc ON p.CategoryId = pc.CategoryId
                                WHERE pc.Category = @Category AND pc.SubCategory = @Subcategory
                                GROUP BY p.ProductId, p.Title, p.description_, p.Price, p.Quantity, p.ImageName, pc.Category";


                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@Subcategory", subcategory);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product()
                    {
                        Id = (int)reader["ProductId"],
                        Title = (string)reader["Title"],
                        description_ = (string)reader["description_"],
                        Price = (double)reader["Price"],
                        Quantity = (int)reader["Quantity"],
                        ImageName = (string)reader["ImageName"],
                        ProductCategory = new ProductCategory
                        {
                            Category = category,
                            SubCategory = subcategory
                        }
                };
                }
            }
            return product;
        }

        public Product GetMaxOfferBySubcategory(string category, string subcategory)
        {
            var product = new Product();
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                connection.Open();

                string query = @"SELECT TOP 1 p.ProductId, p.Title, p.description_, p.Price, p.Quantity, p.ImageName, pc.Category,
                                MAX(o.Discount) AS MaxDiscount
                                 FROM Products p
                                 INNER JOIN Offers o ON p.OfferId = o.OfferId
                                 INNER JOIN ProductCategories pc ON p.CategoryId = pc.CategoryId
                                 WHERE pc.Category = @Category AND pc.SubCategory = @Subcategory
                                 GROUP BY p.ProductId, p.Title, p.description_, p.Price, p.Quantity, p.ImageName, pc.Category
                                 ORDER BY MaxDiscount DESC";

                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@Subcategory", subcategory);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product()
                    {
                        Id = (int)reader["ProductId"],
                        Title = (string)reader["Title"],
                        description_ = (string)reader["description_"],
                        Price = (double)reader["Price"],
                        Quantity = (int)reader["Quantity"],
                        ImageName = (string)reader["ImageName"],
                        ProductCategory = new ProductCategory
                        {
                            Category = category,
                            SubCategory = subcategory
                        },
                        offer = new Offer
                        {
                            Discount = (int)reader["MaxDiscount"]
                        }
                    };
                }
            }
            return product;
        }

        public List<Product> GetDistinctProducts(string category)
        {
            var products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                connection.Open();

                string query = @"SELECT DISTINCT p.ProductId, p.Title, p.description_, p.Price, p.Quantity, p.ImageName, pc.Category, pc.SubCategory
                        FROM Products p
                        INNER JOIN ProductCategories pc ON p.CategoryId = pc.CategoryId
                        WHERE pc.Category = @category";

                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@category", category);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        Id = (int)reader["ProductId"],
                        Title = (string)reader["Title"],
                        description_ = (string)reader["description_"],
                        Price = (double)reader["Price"],
                        Quantity = (int)reader["Quantity"],
                        ImageName = (string)reader["ImageName"],
                        ProductCategory = new ProductCategory
                        {
                            Category = (string)reader["Category"],
                            SubCategory = (string)reader["SubCategory"]
                        }
                    };

                    products.Add(product);
                }
            }
            return products;
        }

        public ProductCategory GetProductCategory(int id)
        {
            var productCategory = new ProductCategory();

            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                connection.Open();

                string query = $"SELECT * FROM ProductCategories WHERE CategoryId = {id}";
                using SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    productCategory.Id = (int)reader["CategoryId"];
                    productCategory.Category = (string)reader["Category"];
                    productCategory.SubCategory = (string)reader["SubCategory"];
                }
            }

            return productCategory;
        }

        public Product GetProduct(int id)
        {
            var product = new Product();
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                connection.Open();

                string query = $"SELECT * FROM Products WHERE ProductId = {id}";
                using SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    product.Id = (int)reader["ProductId"];
                    product.Title = (string)reader["Title"];
                    product.description_ = (string)reader["description_"];
                    product.Price = (double)reader["Price"];
                    product.Quantity = (int)reader["Quantity"];
                    product.ImageName = (string)reader["ImageName"];

                    var categoryId = (int)reader["CategoryId"];
                    product.ProductCategory = GetProductCategory(categoryId);

                    var offerId = (int)reader["OfferId"];
                    product.offer = GetOffer(offerId);
                    product.nota = reader["nota"] != DBNull.Value ? (double)reader["nota"] : 0;

                }
            }
            return product;
        }

        public List<Product> GetProducts(string category, string subcategory, int count)
        {
            var products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                connection.Open();

                string query = $"SELECT TOP {count} * FROM Products WHERE CategoryId = (SELECT CategoryId FROM ProductCategories WHERE Category = @c AND SubCategory = @s) ORDER BY NEWID()";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@c", category);
                command.Parameters.AddWithValue("@s", subcategory);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        Id = (int)reader["ProductId"],
                        Title = (string)reader["Title"],
                        description_ = (string)reader["description_"],
                        Price = (double)reader["Price"],
                        Quantity = (int)reader["Quantity"],
                        ImageName = (string)reader["ImageName"]
                    };

                    var categoryId = (int)reader["CategoryId"];
                    product.ProductCategory = GetProductCategory(categoryId);

                    var offerId = (int)reader["OfferId"];
                    product.offer = GetOffer(offerId);

                    products.Add(product);
                }
            }
            return products;
        }

        public bool InsertUser(User user)
        {
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE Email='" + user.Email + "';";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    connection.Close();
                    return false;
                }

                query = "INSERT INTO Users (FirstName, LastName, Address, Mobile, Email, Password, CreatedAt, ModifiedAt) values (@fn, @ln, @add, @mb, @em, @pwd, @cat, @mat);";

                command.CommandText = query;
                command.Parameters.Add("@fn", System.Data.SqlDbType.NVarChar).Value = user.FirstName;
                command.Parameters.Add("@ln", System.Data.SqlDbType.NVarChar).Value = user.LastName;
                command.Parameters.Add("@add", System.Data.SqlDbType.NVarChar).Value = user.Address;
                command.Parameters.Add("@mb", System.Data.SqlDbType.NVarChar).Value = user.Mobile;
                command.Parameters.Add("@em", System.Data.SqlDbType.NVarChar).Value = user.Email;
                command.Parameters.Add("@pwd", System.Data.SqlDbType.NVarChar).Value = user.Password;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = user.CreatedAt;
                command.Parameters.Add("@mat", System.Data.SqlDbType.NVarChar).Value = user.ModifiedAt;

                command.ExecuteNonQuery();
            }
            return true;
        }

        public string IsUserPresent(string email, string password)
        {
            User user = new User();
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Email=@em AND Password=@psw;";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@em", email);
                command.Parameters.AddWithValue("@psw", password);
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return "";
                }

                query = "SELECT * FROM Users WHERE Email=@em AND Password=@psw;";
                command.CommandText = query;

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user.Id = (int)reader["UserId"];
                    user.FirstName = (string)reader["FirstName"];
                    user.LastName = (string)reader["LastName"];
                    user.Email = (string)reader["Email"];
                    user.Address = (string)reader["Address"];
                    user.Mobile = (string)reader["Mobile"];
                    user.Password = (string)reader["Password"];
                    user.CreatedAt = (string)reader["CreatedAt"];
                    user.ModifiedAt = (string)reader["ModifiedAt"];
                }

                string TokenKey = "MNU66iBl3T5rh6H52i69SuperSecretKey";
                string duration = "60";
                var symmetrickey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey));
                var credentials = new SigningCredentials(symmetrickey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("firstName", user.FirstName),
                    new Claim("lastName", user.LastName),
                    new Claim("address", user.Address),
                    new Claim("mobile", user.Mobile),
                    new Claim("email", user.Email),
                    new Claim("createdAt", user.CreatedAt),
                    new Claim("modifiedAt", user.ModifiedAt)
                };

                var jwtToken = new JwtSecurityToken(
                    issuer: "localhost",
                    audience: "localhost",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToInt32(duration)),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }
        }

        public void InsertReview(Review review)
        {
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                SqlCommand insertCommand = new SqlCommand("INSERT INTO Reviews (UserId, ProductId, Review, CreatedAt, nota) VALUES (@uid, @pid, @rv, @cat, @not);", connection);
                insertCommand.Parameters.AddWithValue("@uid", review.User.Id);
                insertCommand.Parameters.AddWithValue("@pid", review.Product.Id);
                insertCommand.Parameters.AddWithValue("@rv", review.Value);
                insertCommand.Parameters.AddWithValue("@cat", review.CreatedAt);
                insertCommand.Parameters.AddWithValue("@not", review.Nota);

                SqlCommand updateCommand = new SqlCommand("UPDATE Products SET nota = (SELECT AVG(nota) FROM Reviews WHERE ProductId = @pid) WHERE ProductId = @pid;", connection);
                updateCommand.Parameters.AddWithValue("@pid", review.Product.Id);

                connection.Open();
                insertCommand.ExecuteNonQuery();
                updateCommand.ExecuteNonQuery();
            }
        }

        public User GetUser(int id)
        {
            var user = new User();
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Users WHERE UserId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user.Id = (int)reader["UserId"];
                    user.FirstName = (string)reader["FirstName"];
                    user.LastName = (string)reader["LastName"];
                    user.Email = (string)reader["Email"];
                    user.Address = (string)reader["Address"];
                    user.Mobile = (string)reader["Mobile"];
                    user.Password = (string)reader["Password"];
                    user.CreatedAt = (string)reader["CreatedAt"];
                    user.ModifiedAt = (string)reader["ModifiedAt"];
                }
            }
            return user;
        }

        public bool InsertCartItem(int userId, int productId)
        {
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                connection.Open();
                string query = "SELECT COUNT(*) FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    query = "INSERT INTO Carts (UserId, Ordered, OrderedOn) VALUES (" + userId + ", 'false', '');";
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }

                query = "SELECT CartId FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
                command.CommandText = query;
                int cartId = (int)command.ExecuteScalar();


                query = "INSERT INTO CartItems (CartId, ProductId) VALUES (" + cartId + ", " + productId + ");";
                command.CommandText = query;
                command.ExecuteNonQuery();
                return true;
            }
        }

        public int InsertOrder(Order order)
        {
            int value = 0;

            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "INSERT INTO Orders (UserId, CartId, PaymentId, CreatedAt) values (@uid, @cid, @pid, @cat);";

                command.CommandText = query;
                command.Parameters.Add("@uid", System.Data.SqlDbType.Int).Value = order.User.Id;
                command.Parameters.Add("@cid", System.Data.SqlDbType.Int).Value = order.Cart.Id;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = order.CreatedAt;
                command.Parameters.Add("@pid", System.Data.SqlDbType.Int).Value = order.Payment.Id;

                connection.Open();
                value = command.ExecuteNonQuery();

                if (value > 0)
                {
                    query = "UPDATE Carts SET Ordered='true', OrderedOn='" + DateTime.Now.ToString(dateFormat) + "' WHERE CartId=" + order.Cart.Id + ";";
                    command.CommandText = query;
                    command.ExecuteNonQuery();

                    query = "SELECT TOP 1 Id FROM Orders ORDER BY Id DESC;";
                    command.CommandText = query;
                    value = (int)command.ExecuteScalar();
                }
                else
                {
                    value = 0;
                }
            }

            return value;
        }

        public int InsertPayment(Payment payment)
        {
            int value = 0;
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = @"INSERT INTO Payments (PaymentMethodId, UserId, TotalAmount, ShippingCharges, AmountReduced, AmountPaid, CreatedAt) 
                                VALUES (@pmid, @uid, @ta, @sc, @ar, @ap, @cat);";

                command.CommandText = query;
                command.Parameters.Add("@pmid", System.Data.SqlDbType.Int).Value = payment.PaymentMethod.Id;
                command.Parameters.Add("@uid", System.Data.SqlDbType.Int).Value = payment.User.Id;
                command.Parameters.Add("@ta", System.Data.SqlDbType.NVarChar).Value = payment.TotalAmount;
                command.Parameters.Add("@sc", System.Data.SqlDbType.NVarChar).Value = payment.ShipingCharges;
                command.Parameters.Add("@ar", System.Data.SqlDbType.NVarChar).Value = payment.AmountReduced;
                command.Parameters.Add("@ap", System.Data.SqlDbType.NVarChar).Value = payment.AmountPaid;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = payment.CreatedAt;

                connection.Open();
                value = command.ExecuteNonQuery();

                if (value > 0)
                {
                    query = "SELECT TOP 1 Id FROM Payments ORDER BY Id DESC;";
                    command.CommandText = query;
                    value = (int)command.ExecuteScalar();
                }
                else
                {
                    value = 0;
                }
            }
            return value;
        }

        public double CalculateCartTotalValue(int cartId)
        {
            double totalValue = 0;
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                SqlCommand command = new SqlCommand
                {
                    Connection = connection
                };

                connection.Open();

                string query = @"SELECT SUM(p.Price - (p.Price * (o.Discount * 0.01))) AS TotalValue
                         FROM CartItems ci
                         JOIN Products p ON ci.ProductId = p.ProductId
                         LEFT JOIN Offers o ON p.OfferId = o.OfferId
                         WHERE ci.CartId = @CartId;";

                command.CommandText = query;
                command.Parameters.AddWithValue("@CartId", cartId);

                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    totalValue = Convert.ToDouble(result);
                    totalValue = Math.Round(totalValue, 2);
                }
            }
            return totalValue;
        }



        public List<Review> GetProductReviews(int productId)
        {
            var reviews = new List<Review>();
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Reviews WHERE ProductId = @ProductId;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@ProductId", productId);


                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var review = new Review()
                    {
                        Id = (int)reader["ReviewId"],
                        Value = (string)reader["Review"],
                        CreatedAt = (string)reader["CreatedAt"],
                        Nota = reader["nota"] != DBNull.Value ? (int)reader["nota"] : 0
                    };

                    var userid = (int)reader["UserId"];
                    review.User = GetUser(userid);

                    var productid = (int)reader["ProductId"];
                    review.Product = GetProduct(productid);

                    reviews.Add(review);
                }
            }
            return reviews;
        }
    }
}