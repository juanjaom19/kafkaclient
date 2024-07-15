using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class DeleteResponse
{
    [JsonPropertyName("general_message")]
    public string? GeneralMessage { get; set; }
}
