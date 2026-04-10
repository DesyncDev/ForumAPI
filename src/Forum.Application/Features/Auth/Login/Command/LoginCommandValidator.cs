using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Forum.Application.Features.Login.Command;

namespace Forum.Application.Features.Auth.Login.Command
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(e => e.Email)
            .NotEmpty().WithMessage("Email can't be empty")
            .MinimumLength(10).WithMessage("EMAIL - Min: 10 characters")
            .MaximumLength(254).WithMessage("EMAIL - Max: 254 characters")
            .EmailAddress().WithMessage("This field must be a valid email address");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password can't be null")
                .MinimumLength(8).WithMessage("PASSWORD - Min: 8 characters")
                .MaximumLength(32).WithMessage("PASSWORD - Max: 32 characters");
        }
    }
}