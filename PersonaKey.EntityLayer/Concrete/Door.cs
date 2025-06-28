using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.EntityLayer.Concrete
{
    public class Door
    {
        public Door()
        {
            AccessLogs = new HashSet<AccessLog>(); // We start empty collection for door-based access records
            Permissions = new HashSet<Permission>(); // Permissions for the door were initialized as an empty collection
        }

        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // Door name, etc: "Storage Gate"
        public string MacAddress { get; set; } // Door device MAC address

        // Navigation Properties
        public virtual ICollection<AccessLog> AccessLogs { get; set; } // There may be many access records from one door
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
