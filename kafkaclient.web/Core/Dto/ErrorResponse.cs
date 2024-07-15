using System.Text.Json;
using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class ErrorResponse
{
    [JsonPropertyName("general_message")]
    public string? GeneralMessage { get; set; }

    [JsonPropertyName("method")]
    public string? Method { get; set; }
    
    [JsonPropertyName("endpoint")]
    public string? Endpoint { get; set; }
    
    [JsonPropertyName("errors_general")]
    public List<Error>? ErrorsGeneral { get; set; }

    [JsonPropertyName("errors")]
    public Dictionary<String, List<String>> ErrorsProps { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class Error
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    [JsonPropertyName("hints")]
    public string? Hints { get; set; }
    [JsonPropertyName("info")]
    public string? Info { get; set; }

}