using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamDeck_First
{
	public class StreamDeckEventPayload
	{
		public string action { get; set; }

		[JsonProperty(PropertyName = "event")]
		public string _event { get; set; }
		public string context { get; set; }
		public string device { get; set; }
		public Payload payload { get; set; }
	}

	public class Payload
	{
		public dynamic settings { get; set; }
		public Coordinates coordinates { get; set; }
		public int state { get; set; }
		public int userDesiredState { get; set; }
		public bool isInMultiAction { get; set; }
	}

	public class Coordinates
	{
		public int column { get; set; }
		public int row { get; set; }
	}

}
