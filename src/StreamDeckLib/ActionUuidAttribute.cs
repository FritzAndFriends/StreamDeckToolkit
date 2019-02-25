using System;

namespace StreamDeckLib
{

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited =false)]
		
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
