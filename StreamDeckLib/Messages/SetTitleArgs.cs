using Newtonsoft.Json;

namespace StreamDeckLib.Messages
{
	public class SetTitleArgs
	{

		[JsonProperty(PropertyName = "event")]
		public string Event { get { return "setTitle"; } }
		public string context { get; set; }
		public Payload payload { get; set; }

		public class Payload
		{
			public string title { get; set; }

			public int target { get { return (int)TargetType; } }

			[JsonIgnore]
			public TargetType TargetType { get; set; }

		}

		public enum TargetType {

			HardwareAndSoftware = 0,
			Hardware = 1,
			Software = 2

		}

	}


}
