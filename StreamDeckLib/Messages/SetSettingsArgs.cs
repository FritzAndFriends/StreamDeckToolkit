using System;
using System.Collections.Generic;
using System.Text;

namespace StreamDeckLib.Messages
{
    public class SetSettingsArgs : StreamDeckArgsBase
    {
        public override string Event => "setSettings";
        public dynamic payload { get; set; }
    }
}
