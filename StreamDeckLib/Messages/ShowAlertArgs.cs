using System;
using System.Collections.Generic;
using System.Text;

namespace StreamDeckLib.Messages
{
    public class ShowAlertArgs : BaseStreamDeckArgs
    {
        public override string Event => "showAlert";
    }
}
