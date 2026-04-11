using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Domain.Commom.Enums;

namespace Forum.Application.Commom.OneOf.Errors
{
    public sealed record UsernameAlreadyExistError() : AppError("Username already exists", ErrorTypes.Conflict, nameof(UsernameAlreadyExistError));
}