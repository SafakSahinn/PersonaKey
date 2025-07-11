﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.EntityLayer.Concrete
{
    public class Role
    {
        public Role() // Costructor - It works when the class is newly created
        {
            People = new HashSet<Person>(); // One role can belong to many people
            Permissions = new HashSet<Permission>();
            AppUsers = new HashSet<AppUser>();
        }

        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // Role name, etc: "IT", "Security"

        // Navigation Property
        public virtual ICollection<Person> People { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<AppUser> AppUsers { get; set; }
        public virtual RoleAccess RoleAccess { get; set; }
    }
}
