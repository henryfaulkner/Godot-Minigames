using Newtonsoft.Json;

public class PlayerConfigModel
{
    [JsonProperty("TapInputType")]
    public string TapInputType { get; set; }
    [JsonProperty("GrabInputType")]
    public string GrabInputType { get; set; }
    [JsonProperty("KnockInputType")]
    public string KnockInputType { get; set; }
}