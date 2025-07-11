using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.EntityLayer.Concrete
{
    public class RoleAccess
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public bool CanLogin { get; set; } = false;
        public bool CanEditSite { get; set; } = false;

        // Navigation
        public virtual Role Role { get; set; }
    }
}
