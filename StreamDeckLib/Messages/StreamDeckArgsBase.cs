using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamDeckLib.Messages
{
    public abstract class StreamDeckArgsBase
    {
        [JsonProperty(PropertyName = "event")]
        public abstract string Event
        {
            get;
        }

        public string context { get; set; }
    }
}
