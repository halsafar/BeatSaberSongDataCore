using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace SongDataCore.Internal
{
    /// <summary>
    /// IEnumerable way to Deserialize a JsonStream.
    /// Taken from: https://stackoverflow.com/a/37819584/543700
    /// </summary>
    public static partial class JsonExtensions
    {
        public static IEnumerable<T> DeserializeValues<T>(Stream stream)
        {
            return DeserializeValues<T>(new StreamReader(stream));
        }

        public static IEnumerable<T> DeserializeValues<T>(TextReader textReader)
        {
            var serializer = JsonSerializer.CreateDefault();
            var reader = new JsonTextReader(textReader);
            reader.SupportMultipleContent = true;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartArray)
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.Comment)
                            continue;
                        else if (reader.TokenType == JsonToken.EndArray)
                            break;
                        else
                            yield return serializer.Deserialize<T>(reader);
                    }
                }
                else if (reader.TokenType == JsonToken.StartObject)
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.Comment)
                            continue;
                        else if (reader.TokenType == JsonToken.PropertyName)
                            continue;
                        else if (reader.TokenType == JsonToken.EndObject)
                            break;
                        else
                            yield return serializer.Deserialize<T>(reader);
                    }
                }
            }
        }
    }
}
