using System;

namespace StreamDeckLib
{
  public class StreamDeckActionAttribute : Attribute
  {
	private string _uuid;
	public StreamDeckActionAttribute(string uuid = "")
	{
	  this.Uuid = uuid;
	}

	public string Uuid { get => _uuid; set => _uuid = value; }
  }
}
