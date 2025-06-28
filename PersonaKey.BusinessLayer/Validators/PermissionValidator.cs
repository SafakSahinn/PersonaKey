using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Validators
{
    public class PermissionValidator : AbstractValidator<Permission>
    {
        public PermissionValidator()
        {
            RuleFor(p => p.RoleId)
        .GreaterThan(0); // No message will be displayed to the user

            RuleFor(p => p.DoorId)
        .GreaterThan(0); // No message will be displayed to the user
        }
    }
}
