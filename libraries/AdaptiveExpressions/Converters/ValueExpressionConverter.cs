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
    public class ValueExpressionConverter : JsonConverter<ValueExpression>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueExpressionConverter"/> class.
        /// </summary>
        public ValueExpressionConverter()
        {
        }

        public override ValueExpression Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new ValueExpression(reader.GetString());
            }
            else
            {
                return new ValueExpression(JsonValue.Parse(ref reader));
            }
        }

        public override void Write(Utf8JsonWriter writer, ValueExpression value, JsonSerializerOptions options)
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
