using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamDeckLib.Messages
{
    public abstract class BaseStreamDeckArgs
    {
        [JsonProperty(PropertyName = "event")]
        public abstract string Event
        {
            get;
        }

        public string context { get; set; }
    }
}
