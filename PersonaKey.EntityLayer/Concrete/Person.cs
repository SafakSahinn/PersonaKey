using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.EntityLayer.Concrete
{
    public class Person
    {
        public int Id { get; set; } // Primary Key
        public string FullName { get; set; } // Person name and surname
        public string Email { get; set; } // Person email adress


        public int DepartmentId { get; set; } // Foreing Key
        public int RoleId { get; set; } // Foreign Key to Role

        //Navigation Properties
        public virtual Department Department { get; set; }
        public virtual Role Role { get; set; }
        public virtual Card Card { get; set; } // One-to-one relation: A person has one card
    }
}
