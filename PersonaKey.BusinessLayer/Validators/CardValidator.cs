using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Validators
{
    public class CardValidator : AbstractValidator<Card>
    {
        public CardValidator()
        {
            RuleFor(c => c.CardNumber)
                .NotEmpty().WithMessage("Kart numarası boş olamaz")
                .Length(10, 20).WithMessage("Kart numarası 10 ile 20 karakter arasında olmalıdır");

            RuleFor(c => c.PersonId)
                .GreaterThan(0); // We are sending a message, for security reasons
        }
    }
}
