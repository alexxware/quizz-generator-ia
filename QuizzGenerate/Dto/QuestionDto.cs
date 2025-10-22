using System.Text.Json.Serialization;

namespace QuizzGenerate.Dto;

public class QuestionDto
{
    [JsonPropertyName("mainQuestion")]
    public string MainQuestion { get; set; }
    [JsonPropertyName("answare1")]
    public string Answare1 { get; set; }
    [JsonPropertyName("idAnsware1")]
    public string IdAnsware1 { get; set; }
    [JsonPropertyName("answare2")]
    public string Answare2 { get; set; }
    [JsonPropertyName("idAnsware2")]
    public string IdAnsware2 { get; set; }
    [JsonPropertyName("answare3")]
    public string Answare3 { get; set; }
    [JsonPropertyName("idAnsware3")]
    public string IdAnsware3 { get; set; }
    [JsonPropertyName("answare4")]
    public string Answare4 { get; set; }
    [JsonPropertyName("idAnsware4")]
    public string IdAnsware4 { get; set; }
    [JsonPropertyName("idAnswareCorrect")]
    public string IdAnswareCorrect { get; set; }
    [JsonPropertyName("explication")]
    public string Explication { get; set; }
    [JsonPropertyName("error")]
    public bool Error { get; set; }
}