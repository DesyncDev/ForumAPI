using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Domain.Commom.Enums;

namespace Forum.Application.Commom.OneOf
{
    public record AppError(string detail, ErrorTypes type, string errorName);
}