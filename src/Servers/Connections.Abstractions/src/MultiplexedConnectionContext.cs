// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.Connections
{
    /// <summary>
    /// Encapsulates all information about a multiplexed connection.
    /// </summary>
    public abstract class MultiplexedConnectionContext : BaseConnectionContext, IAsyncDisposable
    {
        /// <summary>
        /// Asynchronously accept an incoming stream on the connection.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract ValueTask<MultiplexedStreamContext?> AcceptAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates an outbound connection 
        /// </summary>
        /// <param name="features"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract ValueTask<MultiplexedStreamContext> ConnectAsync(IFeatureCollection? features = null, CancellationToken cancellationToken = default);
    }
}
