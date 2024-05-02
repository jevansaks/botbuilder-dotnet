// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using AdaptiveExpressions.Properties;
using Json.More;
using Json.Schema;

namespace AdaptiveExpressions.Converters
{
    /// <summary>
    /// Converter which allows json to be expression to object or static object.
    /// </summary>
    public class IntExpressionConverter : JsonConverter<IntExpression>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntExpressionConverter"/> class.
        /// </summary>
        public IntExpressionConverter()
        {
        }

        public override IntExpression Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new IntExpression((string)reader.GetString());
            }
            else
            {
                return new IntExpression(JsonValue.Parse(ref reader));
            }
        }

        public override void Write(Utf8JsonWriter writer, IntExpression value, JsonSerializerOptions options)
        {
            if (value.ExpressionText != null)
            {
                writer.WriteStringValue(value.ToString());
            }
            else
            {
                writer.WriteNumberValue(value.Value);
            }
        }
    }
}
