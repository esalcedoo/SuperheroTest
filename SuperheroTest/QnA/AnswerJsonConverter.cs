using Newtonsoft.Json;
using System;
using System.IO;

namespace SuperheroTest.QnA
{
    internal class AnswerJsonConverter : JsonConverter<Answer>
    {
        public override Answer ReadJson(JsonReader reader, Type objectType, Answer existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string content = (string)reader.Value;
            if (IsJson(content))
            {
                return serializer.Deserialize(new StringReader((string)reader.Value), objectType) as Answer;
            }
            return new Answer()
            {
                Text = content
            };
        }

        public override void WriteJson(JsonWriter writer, Answer value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private static bool IsJson(string value)
        {
            return value.Contains('{');
        }
    }
}