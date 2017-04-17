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
    public class UserDetailsController : ApiController
    {
        string connectionString;

        public UserDetailsController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        // GET api/userdetails
        [HttpGet]
        public IEnumerable<UserDetails> Get()
        {
            List<UserDetails> users = new List<UserDetails>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = @"select o.id, o.name, count(petid) as petscount 
                                    from users o
                                    Left Outer join userpets on userid = o.id
                                    group by(o.id)  ";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserDetails user = new UserDetails();

                                //UserPets userPets = new UserPets();

                                user.Id = Int32.Parse(reader["id"].ToString());
                                user.Name = reader["Name"].ToString();
                                user.PetsCount = Int32.Parse(reader["PetsCount"].ToString());
                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (SQLiteException e)
            {
            }
            return users;
        }
    }
}