// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Http.Result
{
    /// <summary>
    /// An <see cref="IResult"/> that on execution invokes <see cref="M:HttpContext.SignOutAsync"/>.
    /// </summary>
    internal sealed partial class SignOutResult : IResult
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SignOutResult"/> with the default sign out scheme.
        /// </summary>
        public SignOutResult()
            : this(Array.Empty<string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SignOutResult"/> with the default sign out scheme.
        /// specified authentication scheme and <paramref name="properties"/>.
        /// </summary>
        /// <param name="properties"><see cref="AuthenticationProperties"/> used to perform the sign-out operation.</param>
        public SignOutResult(AuthenticationProperties properties)
            : this(Array.Empty<string>(), properties)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SignOutResult"/> with the
        /// specified authentication scheme.
        /// </summary>
        /// <param name="authenticationScheme">The authentication scheme to use when signing out the user.</param>
        public SignOutResult(string authenticationScheme)
            : this(new[] { authenticationScheme })
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SignOutResult"/> with the
        /// specified authentication schemes.
        /// </summary>
        /// <param name="authenticationSchemes">The authentication schemes to use when signing out the user.</param>
        public SignOutResult(IList<string> authenticationSchemes)
            : this(authenticationSchemes, properties: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SignOutResult"/> with the
        /// specified authentication scheme and <paramref name="properties"/>.
        /// </summary>
        /// <param name="authenticationScheme">The authentication schemes to use when signing out the user.</param>
        /// <param name="properties"><see cref="AuthenticationProperties"/> used to perform the sign-out operation.</param>
        public SignOutResult(string authenticationScheme, AuthenticationProperties? properties)
            : this(new[] { authenticationScheme }, properties)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SignOutResult"/> with the
        /// specified authentication schemes and <paramref name="properties"/>.
        /// </summary>
        /// <param name="authenticationSchemes">The authentication scheme to use when signing out the user.</param>
        /// <param name="properties"><see cref="AuthenticationProperties"/> used to perform the sign-out operation.</param>
        public SignOutResult(IList<string> authenticationSchemes, AuthenticationProperties? properties)
        {
            AuthenticationSchemes = authenticationSchemes ?? throw new ArgumentNullException(nameof(authenticationSchemes));
            Properties = properties;
        }

        /// <summary>
        /// Gets or sets the authentication schemes that are challenged.
        /// </summary>
        public IList<string> AuthenticationSchemes { get; init; }

        /// <summary>
        /// Gets or sets the <see cref="AuthenticationProperties"/> used to perform the sign-out operation.
        /// </summary>
        public AuthenticationProperties? Properties { get; init; }

        /// <inheritdoc />
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            var logger = httpContext.RequestServices.GetRequiredService<ILogger<SignOutResult>>();

            Log.SignOutResultExecuting(logger, AuthenticationSchemes);

            if (AuthenticationSchemes.Count == 0)
            {
                await httpContext.SignOutAsync(Properties);
            }
            else
            {
                for (var i = 0; i < AuthenticationSchemes.Count; i++)
                {
                    await httpContext.SignOutAsync(AuthenticationSchemes[i], Properties);
                }
            }
        }

        private static partial class Log
        {
            public static void SignOutResultExecuting(ILogger logger, IList<string> authenticationSchemes)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    SignOutResultExecuting(logger, authenticationSchemes.ToArray());
                }
            }

            [LoggerMessage(1, LogLevel.Information,
                "Executing SignOutResult with authentication schemes ({Schemes}).",
                EventName = "SignOutResultExecuting",
                SkipEnabledCheck = true)]
            private static partial void SignOutResultExecuting(ILogger logger, string[] schemes);
        }
    }
}
