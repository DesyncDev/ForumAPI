using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Forum.Application.Features.Register.Command;

namespace Forum.Application.Features.Auth.Register.Command
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username can't be empty")
                .MinimumLength(5).WithMessage("USERNAME - Min: 5 characters")
                .MaximumLength(30).WithMessage("USERNAME - Max: 30 characters");

            RuleFor(dn => dn.DisplayName)
                .NotEmpty().WithMessage("Display name can't be empty")
                .MinimumLength(5).WithMessage("DISPLAY NAME - Min: 5 characters")
                .MaximumLength(80).WithMessage("DISPLAY NAME - Max: 80 characters");

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email can't be empty")
                .MinimumLength(10).WithMessage("EMAIL - Min: 10 characters")
                .MaximumLength(254).WithMessage("EMAIL - Max: 254 characters")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password can't be empty")
                .MinimumLength(8).WithMessage("PASSWORD - Min: 8 characters")
                .MaximumLength(32).WithMessage("PASSWORD - Max: 32 characters");
        }
    }
}