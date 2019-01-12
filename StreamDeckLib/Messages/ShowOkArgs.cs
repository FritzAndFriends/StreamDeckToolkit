using System;
using System.Collections.Generic;
using System.Text;

namespace StreamDeckLib.Messages
{
    public class ShowOkArgs : BaseStreamDeckArgs
    {
        public override string Event => "showOk";
    }
}
