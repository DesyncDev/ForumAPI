using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Forum.Application.Commom.Interfaces;
using Forum.Application.Commom.Interfaces.Repositories;
using Forum.Application.Commom.OneOf;
using Forum.Application.Commom.OneOf.Errors;
using Forum.Application.Features.Login.Command;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using OneOf;

namespace Forum.Application.Features.Auth.Login.Command
{
    public class LoginHandler : IRequestHandler<LoginCommand, OneOf<LoginResponse, AppError>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepo;
        private readonly IRefreshTokenRepository _rtRepo;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _token;

        public LoginHandler(IUnitOfWork uow, IUserRepository userRepo, IPasswordHasher hasher, ITokenService token,
            IRefreshTokenRepository rtToken
        )
        {
            _uow = uow;
            _userRepo = userRepo;
            _rtRepo = rtToken;
            _hasher = hasher;
            _token = token;
        }

        // Methods
        public async Task<OneOf<LoginResponse, AppError>> Handle(LoginCommand command, CancellationToken ct)
        {
            var user = await _userRepo.GetUserByEmailAsync(command.Email, ct);

            if (user is null || !_hasher.Verify(command.Password, user.PasswordHash))
                return new InvalidCredentialsError();

            var (token, expires) = _token.GenerateAccessToken(user);
            var refreshToken = _token.GenerateRefreshToken(user.Id);

            _rtRepo.Add(refreshToken);
            await _uow.CommitAsync(ct);

            return new LoginResponse(
                token,
                refreshToken.Token,
                expires
            );
        }
    }
}