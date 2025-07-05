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
                .MinimumLength(2).WithMessage("İsim en az 2 karakter olmalı")
                .MaximumLength(50).WithMessage("İsim en fazla 50 karakter olabilir")
                .Matches(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$").WithMessage("Sadece harf ve boşluk karakterlerine izin verilir.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir email giriniz")
                .MaximumLength(100).WithMessage("Email en fazla 100 karakter olabilir");

            RuleFor(x => x.DepartmentId)
        .GreaterThan(0);  // We don't give messages, just rules

            RuleFor(x => x.RoleId)
                .GreaterThan(0);  // We don't give messages, just rules
        }
    }
}
