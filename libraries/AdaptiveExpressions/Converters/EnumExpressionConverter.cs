// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
    [RequiresDynamicCode("EnumExpression is not AOT compatible yet")]
    [RequiresUnreferencedCode("EnumExpression is not AOT compatible yet")]
    public class EnumExpressionConverter<T> : JsonConverter<EnumExpression<T>>
        where T : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumExpressionConverter{T}"/> class.
        /// </summary>
        public EnumExpressionConverter()
        {
        }

        /// <summary>
        /// Reads and converts the JSON type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
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

        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "AOT callers will ensure we have a JsonTypeInfo")]
        [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "AOT callers will ensure we have a JsonTypeInfo")]
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
