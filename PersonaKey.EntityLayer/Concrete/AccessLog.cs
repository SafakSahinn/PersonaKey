using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.EntityLayer.Concrete
{
    public class AccessLog
    {
        public int Id { get; set; } // Primary Key
        public int CardId { get; set; } // Which card was used to log in?
        public int DoorId { get; set; } // Which door was entered?
        public DateTime? AccessTime { get; set; } // Entry time
        public DateTime? ExitTime { get; set; } // Exit time
        public bool IsSuccess { get; set; } // Login successful?

        // Navigation Properties
        public virtual Card Card { get; set; }
        public virtual Door Door { get; set; }
    }
}
