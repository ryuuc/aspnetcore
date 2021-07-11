// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.AspNetCore.Connections
{
    public abstract class MultiplexedStreamContext : ConnectionContext
    {
        internal IDictionary<object, object?>? _persistentState;

        public virtual IDictionary<object, object?> PersistentState
        {
            get
            {
                // Lazily allocate connection metadata
                return _persistentState ?? (_persistentState = new ConnectionItems());
            }
            set
            {
                _persistentState = value;
            }
        }
    }
}
