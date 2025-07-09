using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.EntityLayer.Concrete
{
    public class AppUser
    {
        public int Id { get; set; }                  // Primary Key
        public string UserName { get; set; }         // Name Surname
        public string Email { get; set; }            // Email for entry
        public string Password { get; set; }         // Password 

        public int RoleId { get; set; }              // Foreign Key
        public virtual Role Role { get; set; }       // Navigation Property

        public bool IsActive { get; set; } = true;   // Active/Passive
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Created date
    }
}
