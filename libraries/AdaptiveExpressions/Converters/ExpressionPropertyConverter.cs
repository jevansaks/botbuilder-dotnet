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
    /// <typeparam name="T">The property type to construct.</typeparam>
    public class ExpressionPropertyConverter<T> : JsonConverter<ExpressionProperty<T>>
    {
        private JsonTypeInfo<T> _typeInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionPropertyConverter{T}"/> class.
        /// </summary>
        public ExpressionPropertyConverter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionPropertyConverter{T}"/> class.
        /// </summary>
        /// <param name="typeInfo">typeinfo for serializing values of type T.</param>
        public ExpressionPropertyConverter(JsonTypeInfo<T> typeInfo)
        {
            _typeInfo = typeInfo;
        }

        /// <summary>
        /// Reads and converts the JSON type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override ExpressionProperty<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new ExpressionProperty<T>(reader.GetString(), _typeInfo);
            }
            else
            {
                return new ExpressionProperty<T>(JsonValue.Parse(ref reader), _typeInfo);
            }
        }

        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
        public override void Write(Utf8JsonWriter writer, ExpressionProperty<T> value, JsonSerializerOptions options)
        {
            if (value.ExpressionText != null)
            {
                writer.WriteStringValue(value.ToString());
            }
            else
            {
                if (_typeInfo != null)
                {
                    JsonSerializer.SerializeToNode(value.Value, _typeInfo).AsValue().WriteTo(writer, options);
                }
                else
                {
                    JsonValue.Create(value.Value).WriteTo(writer, options);
                }
            }
        }
    }
}
