using BrandedGames.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrandedGames.Common.Exceptions;

public class AuthorizationException : SystemException
{
    public AuthorizationException(ErrorCode errorCode, object responseParams = null) : base(errorCode, responseParams) { }
    public AuthorizationException(List<ExceptionDetail> details) : base(details) { }
}
