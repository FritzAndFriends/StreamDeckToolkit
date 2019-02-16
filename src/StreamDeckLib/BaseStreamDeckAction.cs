using StreamDeckLib.Messages;
using Microsoft.Extensions.Logging;
using StreamDeckLib.Messages;
using System;
using System.Threading.Tasks;

namespace StreamDeckLib
{
  public abstract class BaseStreamDeckAction 
  {
	  protected internal ConnectionManager Manager { get; set; }

		public abstract string ActionUuid { get; }

		public virtual Task OnKeyDown(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnKeyUp(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnWillAppear(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnWillDisappear(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnTitleParametersDidChange(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnDeviceDidConnect(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnDeviceDidDisconnect(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnApplicationDidLaunch(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnApplicationDidTerminate(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnDidReceiveSettings(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnDidReceiveGlobalSettings(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnPropertyInspectorDidAppear(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnPropertyInspectorDidDisappear(StreamDeckEventPayload args) => Task.CompletedTask;
	[Obsolete("This class has been superseded by the \"BaseStreamDeckAction\" class. Please update your references accordingly", false)]
	public abstract class BaseStreamDeckPlugin : BaseStreamDeckAction
	{

	}

  public abstract class BaseStreamDeckAction
  {
    /// <summary>
    /// The <seealso cref="ConnectionManager"/> with which this instance
    /// of the <seealso cref="BaseStreamDeckAction"/> is registered.
    /// </summary>
    /// <value>The manager.</value>
    protected internal ConnectionManager Manager { get; set; }
		
    /// <summary>
    /// Gets the UUID which uniquely identifies the individual actions within a plugin.
    /// </summary>
    /// <value>The UUID representing the action, which matches the value in the &quot;manifest.json&quot; file.</value>
    public abstract string UUID { get; }

    /// <summary>
    /// Gets the value of <seealso cref="UUID"/> in a format which can be used for 
    /// lookups and comparison without concern for letter casing.
    /// </summary>
    /// <value>The registration key.</value>
    protected internal string RegistrationKey => UUID;

		public ILogger Logger { get; internal set; }

    public virtual Task OnKeyDown(StreamDeckEventPayload args) => Task.CompletedTask;

    public virtual Task OnKeyUp(StreamDeckEventPayload args) => Task.CompletedTask;

    public virtual Task OnWillAppear(StreamDeckEventPayload args) => Task.CompletedTask;

    public virtual Task OnWillDisappear(StreamDeckEventPayload args) => Task.CompletedTask;

    public virtual Task OnTitleParametersDidChange(StreamDeckEventPayload args) => Task.CompletedTask;

    public virtual Task OnDeviceDidConnect(StreamDeckEventPayload args) => Task.CompletedTask;

    public virtual Task OnDeviceDidDisconnect(StreamDeckEventPayload args) => Task.CompletedTask;

    public virtual Task OnApplicationDidLaunch(StreamDeckEventPayload args) => Task.CompletedTask;

    public virtual Task OnApplicationDidTerminate(StreamDeckEventPayload args) => Task.CompletedTask;

    public virtual Task OnPropertyInspectorMessageReceived(PropertyInspectorEventPayload args) => Task.CompletedTask;

    public virtual Task OnPropertyInspectorConnected(PropertyInspectorEventPayload args) => Task.CompletedTask;

    public virtual Task OnPropertyInspectorDisconnected(PropertyInspectorEventPayload args) => Task.CompletedTask;
		
  }
}
