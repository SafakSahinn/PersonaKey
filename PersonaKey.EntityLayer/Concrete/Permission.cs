using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.EntityLayer.Concrete
{
    public class Permission
    {
        public int Id { get; set; } // Primary Key
        public int RoleId { get; set; } // Foreign Key to Role
        public int DoorId { get; set; } // Foreign Key to Door

        // Navigation Properties
        public virtual Role Role { get; set; }
        public virtual Door Door { get; set; }
    }
}
