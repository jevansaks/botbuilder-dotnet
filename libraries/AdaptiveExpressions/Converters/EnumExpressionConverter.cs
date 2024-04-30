// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using AdaptiveExpressions.Properties;

namespace AdaptiveExpressions.Converters
{
    /// <summary>
    /// Converter which allows json to be expression to object or static object.
    /// </summary>
    /// <typeparam name="T">The enum type to construct.</typeparam>
    public class EnumExpressionConverter<T> : JsonConverter<EnumExpression<T>>
        where T : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumExpressionConverter{T}"/> class.
        /// </summary>
        public EnumExpressionConverter()
        {
        }

        public override EnumExpression<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new EnumExpression<T>(reader.GetString());
            }
            else
            {
                return new EnumExpression<T>(JsonValue.Parse(ref reader));
            }
        }

        public override void Write(Utf8JsonWriter writer, EnumExpression<T> value, JsonSerializerOptions options)
        {
            if (value.ExpressionText != null)
            {
                writer.WriteStringValue(value.ToString());
            }
            else
            {
                // TODO: Could be - writer.WriteStringValue(value.ValueToString()); ?
                JsonValue.Create(value.Value).WriteTo(writer, options);
            }
        }
    }
}
