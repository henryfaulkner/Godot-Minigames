using Newtonsoft.Json;

public class PlayerConfigModel
{

	[JsonProperty("CommanderId")]
	public int CommanderId { get; set; }

	[JsonProperty("Name")]
	public string Name { get; set; }

	[JsonProperty("TapInputKey")]
	public string TapInputKey { get; set; }

	[JsonProperty("GrabInputKey")]
	public string GrabInputKey { get; set; }

	[JsonProperty("KnockInputKey")]
	public string KnockInputKey { get; set; }
}
