using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Author { get; set; } = "";

        public string ISBN { get; set; } = "";

        [JsonPropertyName("publicationDate")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime PublicationDate { get; set; }
    }

    // Simple converter to format date as "YYYY-MM-DD"
    public class JsonDateConverter : JsonConverter<DateTime>
    {
        private readonly string _format = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // parse incoming string to DateTime
            return DateTime.Parse(reader.GetString() ?? "");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // write date as "YYYY-MM-DD"
            writer.WriteStringValue(value.ToString(_format));
        }
    }
}
