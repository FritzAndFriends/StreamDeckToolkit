﻿using Microsoft.Extensions.Logging;
using StreamDeckLib.Messages;
using System.Linq;
using System.Threading.Tasks;

namespace StreamDeckLib
{
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
	public string ActionUuid
	{
	  get
	  {
		return this.GetType().GetCustomAttributes(typeof(ActionUuidAttribute), true).FirstOrDefault() is ActionUuidAttribute attr && !string.IsNullOrWhiteSpace(attr.Uuid)
		  ? attr.Uuid
		  : string.Empty;
	  }
	}

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

	public virtual Task OnDidReceiveSettings(StreamDeckEventPayload args) => Task.CompletedTask;

	public virtual Task OnDidReceiveGlobalSettings(StreamDeckEventPayload args) => Task.CompletedTask;

	public virtual Task OnPropertyInspectorDidDisappear(StreamDeckEventPayload args) => Task.CompletedTask;

	public virtual Task OnPropertyInspectorDidAppear(StreamDeckEventPayload args) => Task.CompletedTask;
  }
}
