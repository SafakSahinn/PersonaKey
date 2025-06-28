using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.EntityLayer.Concrete
{
    public class Card
    {
        public int Id { get; set; } // Primary Key
        public string CardNumber { get; set; } // Card Number od UID
        public int PersonId { get; set; } // Foreign Key - which person does it belong to?
        public bool IsActive { get; set; } // Card active/passive status

        // Navigation Properties
        public virtual Person Person { get; set; } // One person is connected to one card
    }
}
