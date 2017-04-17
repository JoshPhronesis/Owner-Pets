using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OwnersPets.Models;

namespace OwnersPets.Controllers
{
    public class UsersController : ApiController
    {
        static string connectionString;
        public UsersController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        // GET api/users/
       public IEnumerable<User> GetUsers()
        {
            List<User> users = new List<User>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM Users";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User();
                            user.UserId = Int32.Parse(reader["id"].ToString());
                            user.UserName=reader["Name"].ToString();
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }

        // GET api/users/5
        public User Get(int id)
        {
            User user = new User();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                   
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = "select Name from users where id = @I ";
                        command.Parameters.AddWithValue("@I", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user.UserId = id;
                                user.UserName = reader["Name"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }

        // POST api/users
        [System.Web.Http.HttpPost]
        public  string PostUser([FromBody]string Name)
        {
            
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText= "INSERT INTO Users(Name) VALUES(@Name)";
                    command.Parameters.AddWithValue("@Name", Name);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return "success";
        }


        // PUT api/users/5
        [HttpPut]
        public int UpdateUser(int id, [FromBody] string name)
        {
            int result = -1;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "UPDATE User "
                        + "SET @N " + "WHERE Id=@I ";
                    command.Prepare();
                    command.Parameters.AddWithValue("@N", name);
                    command.Parameters.AddWithValue("@I", id);
                    try
                    {
                        result = command.ExecuteNonQuery();
                    }
                    catch (SQLiteException)
                    {
                    }
                }
            }
            return result;
        }

        // DELETE api/users/5
        [HttpDelete]
        public int Delete(int id)
        {
            int result = -1;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM Users WHERE Id =@I ";
                    command.Parameters.AddWithValue("@I", id);
                    command.Prepare();

                    try
                    {
                        result = command.ExecuteNonQuery();
                    }
                    catch (SQLiteException)
                    {
                        throw;
                    }
                }
            }
            return result;
        }
    }
}
