using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OwnersPets.Models;

namespace OwnersPets.Controllers
{
    public class PetsController : ApiController
    {
        string connectionString;
        public PetsController()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        // GET api/Pets/
        public IEnumerable<Pet> GetPets()
        {
            List<Pet> Pets = new List<Pet>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM Pets";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pet Pet = new Pet();
                            Pet.Id = Int32.Parse(reader["id"].ToString());
                            Pet.Name = reader["Name"].ToString();
                            Pets.Add(Pet);
                        }
                    }
                }
            }
            return Pets;
        }

        // GET api/Pets/5
        public Pet Get(int id)
        {
            Pet Pet = new Pet();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = "select* from Pets where id = @I ";
                        command.Parameters.AddWithValue("@I", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Pet.Id = Int32.Parse(reader["id"].ToString());
                                Pet.Name = reader["Name"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Pet;
        }

        // POST api/Pets
        [HttpPost]
        public void PostPet([FromBody]string petName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO Pets(Name) VALUES(@Name)";
                    command.Parameters.AddWithValue("@Name", petName);
                    command.Prepare();

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
        }


        // PUT api/Pets/5
        [HttpPut]
        public int UpdatePet(int id, [FromBody] string name)
        {
            int result = -1;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "UPDATE Pet "
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

        // DELETE api/Pets/5
        [HttpDelete]
        public int Delete(int id)
        {
            int result = -1;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM Pets WHERE Id =@I ";
                    command.Prepare();
                    command.Parameters.AddWithValue("@I", id);
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