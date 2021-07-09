// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Microsoft.AspNetCore.Http.Result
{
    public class OkObjectResultTest
    {
        [Fact]
        public async Task OkObjectResult_SetsStatusCodeAndValue()
        {
            // Arrange
            var result = new OkObjectResult("Hello world");
            var httpContext = GetHttpContext();

            // Act
            await result.ExecuteAsync(httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, httpContext.Response.StatusCode);
        }

        private static HttpContext GetHttpContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.PathBase = new PathString("");
            httpContext.Response.Body = new MemoryStream();
            httpContext.RequestServices = CreateServices();
            return httpContext;
        }

        private static IServiceProvider CreateServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            return services.BuildServiceProvider();
        }
    }
}
