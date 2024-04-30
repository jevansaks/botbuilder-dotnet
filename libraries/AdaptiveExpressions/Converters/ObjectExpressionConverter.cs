// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using AdaptiveExpressions.Properties;

namespace AdaptiveExpressions.Converters
{
    /// <summary>
    /// Converter which allows json to be expression to object or static object.
    /// </summary>
    /// <typeparam name="T">The property type to construct.</typeparam>
    public class ObjectExpressionConverter<T> : JsonConverter<ObjectExpression<T>>
    {
        public override ObjectExpression<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new ObjectExpression<T>(reader.GetString());
            }
            else
            {
                return new ObjectExpression<T>(JsonValue.Parse(ref reader));
            }
        }

        public override void Write(Utf8JsonWriter writer, ObjectExpression<T> value, JsonSerializerOptions options)
        {
            if (value.ExpressionText != null)
            {
                writer.WriteStringValue(value.ToString());
            }
            else
            {
                JsonValue.Create(value.Value).WriteTo(writer, options);
            }
        }
    }
}
