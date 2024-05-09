// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using AdaptiveExpressions.Properties;
using Json.More;

namespace AdaptiveExpressions.Converters
{
    /// <summary>
    /// Converter which allows json to be expression to object or static object.
    /// </summary>
    /// <typeparam name="T">The type of the items of the array.</typeparam>
    [RequiresDynamicCode("ArrayExpression is not AOT compatible yet")]
    [RequiresUnreferencedCode("ArrayExpression is not AOT compatible yet")]
    public class ArrayExpressionConverter<T> : JsonConverter<ArrayExpression<T>>
    {
        private JsonTypeInfo<T> _valueTypeInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayExpressionConverter{T}"/> class.
        /// </summary>
        public ArrayExpressionConverter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayExpressionConverter{T}"/> class.
        /// </summary>
        /// <param name="typeInfo">TypeInfo for serializing entries.</param>
        public ArrayExpressionConverter(JsonTypeInfo<T> typeInfo)
        {
            _valueTypeInfo = typeInfo;
        }

        /// <summary>
        /// Reads and converts the JSON type to <typeparamref name="T"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override ArrayExpression<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new ArrayExpression<T>(reader.GetString());
            }
            else
            {
                // NOTE: This does not use the serializer because even we could deserialize here
                // expression evaluation has no idea about converters.
                return new ArrayExpression<T>(JsonValue.Parse(ref reader));
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
        public override void Write(Utf8JsonWriter writer, ArrayExpression<T> value, JsonSerializerOptions options)
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
