using EmpeekTest.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using OwnersPets.Models;

namespace OwnersPets.Controllers
{
    public class UserPetsController : ApiController
    {
        string connectionString;
        public UserPetsController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        // PUT api/userpets

        [HttpPut]
        public  int PutUserPet(int id, [FromBody] UserPetsTransferObject UserPetsTransferObject)
        {

            int result = -1;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"INSERT INTO UserPets(UserId, PetId) SELECT @userId, ID FROM PETS WHERE NAME = @name ";
                        command.Parameters.AddWithValue("@userId", id);
                        command.Parameters.AddWithValue("@name", UserPetsTransferObject.PetName);

                        try
                        {
                            result = command.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        // GET api/userpets/5

        public IEnumerable<UserPets> Get(int id)
        {
            List<UserPets> userPets = new List<UserPets>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"select userid, u.name as UserName, PetId as PetId, p.name as PetName
                                                from userpets 
                                                join pets as p on petid=p.id
                                                join users as u on userid= u.id
                                                where userid= @I ";
                        command.Parameters.AddWithValue("@I", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                           
                            while (reader.Read())
                            {
                                UserPets userpet = new UserPets();
                                userpet.UserId = id;
                                userpet.UserName = reader["UserName"].ToString();
                                userpet.PetId = (Int32.Parse(reader["petid"].ToString()));
                                userpet.PetName= reader["PetName"].ToString();
                                userPets.Add(userpet);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return userPets;
        }

        // DELETE api/userpets/5
        [HttpDelete]
        public int DeleteUserPet(int id, [FromBody]UserPetsTransferObject UserPetsTransferObject)
        {
            int result = -1;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @" DELETE FROM USERPETS WHERE (userid=@userid and petid=@petid) ";
                        command.Parameters.AddWithValue("@userId", id);
                        command.Parameters.AddWithValue("@petId", UserPetsTransferObject.PetId);

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
            }
            catch (Exception)
            {
            }
            return result;
        }

    }
}