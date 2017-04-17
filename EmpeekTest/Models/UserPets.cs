using System.Collections.Generic;

namespace OwnersPets.Models
{
    public class UserPets
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PetId { get; set; }
        public string PetName { get; set; }
    }
}