using Newtonsoft.Json;

public class PlayerConfigModel
{
	[JsonProperty("TapInputKey")]
	public string TapInputKey { get; set; }
	[JsonProperty("GrabInputKey")]
	public string GrabInputKey { get; set; }
	[JsonProperty("KnockInputKey")]
	public string KnockInputKey { get; set; }
}
