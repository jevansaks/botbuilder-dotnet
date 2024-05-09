// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
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

        /// <summary>
        /// Reads and converts the JSON type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override ValueExpression Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var typeInfo = options.GetTypeInfo(typeToConvert);
            if (reader.TokenType == JsonTokenType.String)
            {
                return new ValueExpression(reader.GetString(), typeInfo);
            }
            else
            {
                return new ValueExpression(JsonValue.Parse(ref reader), typeInfo);
            }
        }

        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "AOT callers will ensure we have a JsonTypeInfo")]
        [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "AOT callers will ensure we have a JsonTypeInfo")]
        public override void Write(Utf8JsonWriter writer, ValueExpression value, JsonSerializerOptions options)
        {
            if (value.ExpressionText != null)
            {
                writer.WriteStringValue(value.ToString());
            }
            else
            {
                if (value.ValueJsonTypeInfo != null)
                {
                    JsonSerializer.SerializeToNode(value.Value, value.ValueJsonTypeInfo).AsValue().WriteTo(writer, options);
                }
                else
                {
                    JsonValue.Create(value.Value).WriteTo(writer, options);
                }
            }
        }
    }
}
