using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Validators
{
    public class AccessLogValidator : AbstractValidator<AccessLog>
    {
        public AccessLogValidator()
        {
            RuleFor(x => x.CardId)
        .GreaterThan(0).WithMessage("Geçerli bir kart seçmelisiniz.");

            RuleFor(x => x.DoorId)
        .GreaterThan(0).WithMessage("Geçerli bir kapı seçmelisiniz.");

            RuleFor(x => x.AccessTime)
        .NotEmpty().WithMessage("Giriş zamanı boş olamaz.");

            RuleFor(x => x.ExitTime)
        .Must((log, exitTime) => !exitTime.HasValue || exitTime >= log.AccessTime)
        .WithMessage("Çıkış zamanı, giriş zamanından önce olamaz.");
        }
    }
}
