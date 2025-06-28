using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Validators
{
    public class DoorValidator : AbstractValidator<Door>
    {
        public DoorValidator()
        {
            RuleFor(d => d.Name)
        .NotEmpty().WithMessage("Kapı adı boş olamaz")
        .MaximumLength(30).WithMessage("Kapı adı en fazla 30 karakter olmalıdır");

            RuleFor(d => d.MacAddress)
        .NotEmpty().WithMessage("MAC adresi boş olamaz")
        .Matches("^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$")
        .WithMessage("Geçerli bir MAC adresi giriniz (örn. AA:BB:CC:DD:EE:FF)");
        }
    }
}
