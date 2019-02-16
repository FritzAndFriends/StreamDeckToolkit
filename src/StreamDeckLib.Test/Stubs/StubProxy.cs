using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StreamDeckLib.Messages;

namespace StreamDeckLib.Test
{
	/// <summary>
	/// A StreamDeck proxy base class
	/// </summary>
	public class StubProxy : IStreamDeckProxy
	{

		public const int TEST_PORT = 3000;
		public const string TEST_UUID = "TEST_UUID";
		public const string TEST_EVENT = "TEST_EVENT_TYPE";

		public static readonly string[] ValidCommandLineArguments = new string[] {
			"-port", TEST_PORT.ToString(),							// Made up port number
			"-pluginUUID", TEST_UUID,
			"-registerEvent", TEST_EVENT,
			"-info", @"{
				""application"": {
					""language"": ""en"",
					""platform"": ""mac"",
					""version"": ""4.0.0""
				},
				""devices"": [
					{
						""id"": ""55F16B35884A859CCE4FFA1FC8D3DE5B"",
						""size"": {

							""columns"": 5,
							""rows"": 3

						}, 
						""type"": 0
					},
					{
						""id"": ""B8F04425B95855CF417199BCB97CD2BB"", 
						""size"": {
							""columns"": 3, 
							""rows"": 2
						}, 
						""type"": 1
					}
				]
			}"
		};

		public Action<Uri,CancellationToken> InspectConnectAsync { get; set; }
		public Action<string, string> InspectRegister { get; set; }

		public WebSocketState State { get; set; }

		public Task ConnectAsync(Uri uri, CancellationToken token)
		{
			InspectConnectAsync?.Invoke(uri, token);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public Task<string> GetMessageAsString(CancellationToken token)
		{
			return Task.FromResult("");
		}

		public Task Register(string registerEvent, string uuid)
		{
			InspectRegister?.Invoke(registerEvent, uuid);
			return Task.CompletedTask;
		}

		public Task SendStreamDeckEvent(BaseStreamDeckArgs args)
		{
			throw new NotImplementedException();
		}

	}

}
