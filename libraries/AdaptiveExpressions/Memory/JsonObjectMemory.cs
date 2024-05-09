// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;
using Json.Path;

namespace AdaptiveExpressions.Memory
{
    /// <summary>
    /// Implementation of <see cref="IMemory"/> over JsonObject.
    /// </summary>
    public class JsonObjectMemory : IMemory
    {
        private JsonObject _memory = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonObjectMemory"/> class.
        /// </summary>
        /// <param name="memory">The object to wrap.</param>
        public JsonObjectMemory(JsonObject memory)
        {
            _memory = memory;
        }

        /// <inheritdoc/>
        public IMemory CreateMemoryFrom(object value)
        {
            if (value is JsonObject jobj)
            {
                return new JsonObjectMemory(jobj);
            }

            throw new InvalidOperationException("Unknown value");
        }

        /// <inheritdoc/>
        public void SetValue(string path, object value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryGetValue(string path, out object value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string Version()
        {
            throw new NotImplementedException();
        }
    }
}
