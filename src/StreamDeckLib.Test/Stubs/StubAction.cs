using Microsoft.Extensions.Logging;

namespace StreamDeckLib.Test
{
  [ActionUuid(Uuid = "Test UUID")]
  public class StubAction : BaseStreamDeckAction
  {

	public StubAction() : this("Test UUID")
	{

	}

	public StubAction(string uuid)
	{
	  this._uuid = uuid;
	}

	private string _uuid;


	public StubAction SetUUID(string uuid)
	{
	  this._uuid = uuid;

	  return this;
	}

	public ConnectionManager GetConnectionManager()
	{
	  return this.Manager;
	}

	public ILogger GetLogger()
	{
	  return this.Logger;
	}



  }

}
