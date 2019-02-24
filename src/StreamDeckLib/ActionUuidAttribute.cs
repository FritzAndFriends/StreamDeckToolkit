using System;

namespace StreamDeckLib
{
  public class ActionUuidAttribute : Attribute
  {
	private string _uuid;
	public ActionUuidAttribute(string uuid = "")
	{
	  _uuid = uuid;
	}

	public string Uuid { get => _uuid; set => _uuid = value; }
  }
}
