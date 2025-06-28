using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Validators
{
    public class DepartmentValidator : AbstractValidator<Department>
    {
        public DepartmentValidator()
        {
            RuleFor(d => d.Name)
        .NotEmpty().WithMessage("Departman adı boş olamaz.")
        .MinimumLength(2).WithMessage("Departman adı en az 2 karakter olmalı.")
        .MaximumLength(30).WithMessage("Departman adı en fazla 50 karakter olabilir.");

        }
    }
}
