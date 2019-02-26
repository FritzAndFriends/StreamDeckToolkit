using System;

namespace StreamDeckLib
{

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited =false)]	
  public class ActionUuidAttribute : Attribute
  {
	
	public ActionUuidAttribute(string uuid = "")
	{
	  Uuid = uuid;
	}

	public string Uuid { get; set; }
  }
}
