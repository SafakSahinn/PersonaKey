using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Validators
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(r => r.Name)
        .NotEmpty().WithMessage("Role adı boş olamaz.")
        .MinimumLength(2).WithMessage("Role adı en az 2 karakter olmalı.")
        .MaximumLength(50).WithMessage("Role adı en fazla 50 karakter olabilir.")
        .Matches(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$").WithMessage("Sadece harf ve boşluk karakterlerine izin verilir.");

        }
    }
}
