using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Commom.Interfaces;
using Forum.Application.Commom.Interfaces.Repositories;
using Forum.Application.Commom.OneOf;
using Forum.Application.Commom.OneOf.Errors;
using Forum.Application.Features.Register.Command;
using Forum.Domain.Entities;
using MediatR;
using OneOf;

namespace Forum.Application.Features.Auth.Register.Command
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, OneOf<RegisterResponse, AppError>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _hasher;
        private readonly IUnitOfWork _uow;

        public RegisterHandler(IUserRepository userRepo, IPasswordHasher hasher, IUnitOfWork uow)
        {
            _userRepo = userRepo;
            _hasher = hasher;
            _uow = uow;
        }

        public async Task<OneOf<RegisterResponse, AppError>> Handle(RegisterCommand command, CancellationToken ct)
        {
            var usernameExists = await _userRepo.CheckUserByUsernameAsync(command.Username, ct);

            if (usernameExists)
                return new UsernameAlreadyExistError();

            var emailExists = await _userRepo.CheckUserByEmailAsync(command.Email, ct);

            if (emailExists)
                return new EmailAlreadyExistError();

            var normalizedEmail = command.Email.Trim().ToLower();
            var hashedPass = _hasher.Hash(command.Password);

            var newUser = new User(
                command.Username,
                normalizedEmail,
                hashedPass,
                command.DisplayName
            );

            _userRepo.Add(newUser);
            await _uow.CommitAsync(ct);

            return new RegisterResponse(
                newUser.Id,
                newUser.CreatedAt
            );
        }
    }
}