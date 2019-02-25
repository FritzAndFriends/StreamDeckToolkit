using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace StreamDeckLib
{
  [Serializable]
  internal class TypeDoesNotInheritFromBaseStreamDeckAction : Exception
  {
    private string fullName;
    private Assembly assembly;

    public TypeDoesNotInheritFromBaseStreamDeckAction()
    {
    }

    public TypeDoesNotInheritFromBaseStreamDeckAction(string message) : base(message)
    {
    }

    public TypeDoesNotInheritFromBaseStreamDeckAction(string fullName, Assembly assembly) : this($"The type \"{fullName}\" from assembly \"{assembly.GetName()}\" does not inherit from required base class \"{nameof(BaseStreamDeckAction)\"."})
    {
      this.fullName = fullName;
      this.assembly = assembly;
    }

    public TypeDoesNotInheritFromBaseStreamDeckAction(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected TypeDoesNotInheritFromBaseStreamDeckAction(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}