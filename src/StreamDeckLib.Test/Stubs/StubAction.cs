using StreamDeckLib.Messages;
using System.Threading.Tasks;

namespace StreamDeckLib.Test
{

  [StreamDeckAction(Uuid = "Test UUID")]
  public class StubAction : BaseStreamDeckAction
  {
	public int Counter = 0;

	internal ConnectionManager GetConnectionManager()
	{
	  return this.Manager;
	}

	public override Task OnDidReceiveGlobalSettings(StreamDeckEventPayload args)
	{
	  Counter = 1;
	  return Task.CompletedTask;
	}

  }

}
