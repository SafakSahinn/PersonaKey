using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.EntityLayer.Concrete
{
    public class Department
    {
        public Department() // Constructor - It works when the class is newly created
        {
            People = new HashSet<Person>(); // We start the People collection empty
        }

        public int Id { get; set; } // Primary Key
        public string Name { get; set; }

        // Navigation Properties
        public virtual ICollection<Person> People { get; set; } // One-to-many: One department has many persons
    }
}
