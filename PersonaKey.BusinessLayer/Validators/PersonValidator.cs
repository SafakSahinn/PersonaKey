using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.FullName)
        .NotEmpty().WithMessage("İsim boş olamaz")
        .MinimumLength(2).WithMessage("İsim en az 2 karakter olmalı");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir email giriniz");

            RuleFor(x => x.DepartmentId)
        .GreaterThan(0);  // We don't give messages, just rules

            RuleFor(x => x.RoleId)
                .GreaterThan(0);  // We don't give messages, just rules
        }
    }
}
