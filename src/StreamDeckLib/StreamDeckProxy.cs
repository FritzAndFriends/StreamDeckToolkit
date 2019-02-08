using Newtonsoft.Json;
using StreamDeckLib.Messages;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamDeckLib
{

	internal class StreamDeckProxy : IStreamDeckProxy
	{

		/*
		 * Cheer 100 Auth0bobby January 27, 2019
		 * Cheer 100 cpayette January 27, 2019
		 * Cheer 100 RamblingGeek January 27, 2019
		 * Cheer 100 PharEwings January 27, 2019
		 * */

		private readonly ClientWebSocket _Socket = new ClientWebSocket();

		public WebSocketState State { get { return _Socket.State; } }

		public Task ConnectAsync(Uri uri, CancellationToken token)
		{
			return _Socket.ConnectAsync(uri, token);
		}
		public Task Register(string registerEvent, string uuid)
		{
			return _Socket.SendAsync(GetPluginRegistrationBytes(registerEvent, uuid), WebSocketMessageType.Text, true, CancellationToken.None);
		}

		public Task SendStreamDeckEvent(BaseStreamDeckArgs args)
		{
			var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(args));
			return _Socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
		}

		public async Task<string> GetMessageAsString(CancellationToken token)
		{
			var buffer = new byte[65536];
			var segment = new ArraySegment<byte>(buffer, 0, buffer.Length);
			await _Socket.ReceiveAsync(segment, token);
			var jsonString = Encoding.UTF8.GetString(buffer);
			return jsonString;
		}

		private ArraySegment<byte> GetPluginRegistrationBytes(string registerEvent, string uuid)
		{
			var registration = new Messages.Info.PluginRegistration
			{
				@event = registerEvent,
				uuid = uuid
			};

			var outString = JsonConvert.SerializeObject(registration);
			var outBytes = Encoding.UTF8.GetBytes(outString);

			return new ArraySegment<byte>(outBytes);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{           // TODO: dispose managed state (managed objects).
				}

				_Socket.Dispose();
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		~StreamDeckProxy()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			GC.SuppressFinalize(this);
		}
		#endregion

	}

}
