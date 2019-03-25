using FluentAssertions;
using StreamDeckLib.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StreamDeckLib.Test
{

  public class ConnectionManagerShould
  {
	[Fact]
	public void ThrowAnArgumentException_WhenInitializedWithEmptyArgs()
	{
	  // Arrange
	  var commandLineArgs = new string[0];

	  // Act
	  Func<ConnectionManager> action = () => ConnectionManager.Initialize(commandLineArgs);

	  // Assert
	  action.Should().Throw<ArgumentException>();
	}

	[Fact]
	public async Task ShouldRegisterEvent_WhenInitializedWithCorrectArgs()
	{
	  // Arrange
	  var stub = new StubProxy()
	  {
		InspectRegister = (e, uuid) =>
											{
											  Assert.Equal(StubProxy.TEST_EVENT, e);
											  Assert.Equal(uuid, uuid);
											}
	  };

	  // Act
	  var tokenSource = new CancellationTokenSource();
	  var task = ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments, null, stub)
												   .StartAsync(tokenSource.Token);

	  // Assert
	  Assert.Null(task.Exception);
	  tokenSource.Cancel();

	}


	[Fact]
	public async Task ShouldRegisterAllActions_WhenRegisteringAllActions()
	{

	  Func<ConnectionManager> action = () => ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
  .RegisterAllActions(this.GetType().Assembly);

	  action.Should()
		  .NotThrow("No problems when registering All Actions");


	}

	[Fact]
	public async Task ShouldHaveConnectionManagerAssigned_WhenGettingAnActionInstance()
	{
	  var cm = ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
							.RegisterActionType("Unique_Action_ID_1", typeof(StubAction));
	  var action = cm.GetInstanceOfAction("FAKECONTEXT", "Unique_Action_ID_1") as StubAction;
	  action.GetConnectionManager().Should().NotBeNull("An action must have a connection manager assigned");

	}

	[Fact]
	public async Task ShouldHaveLoggerAssigned_WhenGettingAnActionInstance()
	{
	  var cm = ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
					 .RegisterActionType("Unique_Action_ID_1", typeof(StubAction));
	  var action = cm.GetInstanceOfAction("FAKECONTEXT", "Unique_Action_ID_1") as StubAction;
	  action.Logger.Should().NotBeNull("An action must have a logger assigned");
	}


	[Fact]
	public async Task ShouldThrowArgumentException_WhenRegisteringAnActionWithoutAUUID()
	{
	  //
	  // Arrange
	  //

	  Func<ConnectionManager> action = () => ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
															  .RegisterActionType("", typeof(StubAction));

	  //
	  // Act
	  //
	  action.

	  //
	  // Assert
	  //
	  Should().Throw<IncompleteActionDefinitionException>("An action must specify a non-blank or empty UUID");
	}

	[Fact]
	public async Task ShouldThrowDuplicateActionRegistrationException_WhenRegisteringMultipleInstancesOfTheSameAction()
	{
	  //
	  // Arrange
	  //


	  Func<ConnectionManager> action = () => ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
															  .RegisterActionType("UniqueID", typeof(StubAction))
															  .RegisterActionType("UniqueID", typeof(StubAction));

	  //
	  // Assert
	  //

	  //TODO: Wording? "unique UUID" is like saying "PIN number". It's "just" a test, but still syntactically incorrect.
	  action.Should()
	  .Throw<DuplicateActionRegistrationException>("each BaseStreamDeckAction type should have its own unique UUID.");
	}


	[Fact]
	public async Task ShouldNotThrowAnyExceptions_WhenRegistringMultipleUniqueActions()
	{
	  //
	  // Arrange
	  //


	  Func<ConnectionManager> action = () => ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
		  .RegisterActionType("com.csharpfritz.samplePlugin.action", typeof(StubAction))
		  .RegisterActionType("com.csharpfritz.samplePlugin.action2", typeof(StubAction));


	  //
	  // Assert
	  //

	  //TODO: Wording? "unique UUID" is like saying "PIN number". It's "just" a test, but still syntactically incorrect.
	  action.Should()
	  .NotThrow("registering multiple unique actions is valid");
	}


	[Fact]
	public async Task ActionsShouldReceiveGlobalSetting_WhenGlobalSettingsAreBroadcast()
	{
	  //make sure the global settings are accessible in the actions
	  var mgr = ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
			.RegisterActionType("com.csharpfritz.samplePlugin.action", typeof(StubAction))
			.RegisterActionType("com.csharpfritz.samplePlugin.action2", typeof(StubAction));

	  var dummyPayload = new StreamDeckEventPayload();
	  dummyPayload.Event = "didReceiveGlobalSettings";
	  dummyPayload.payload = new StreamDeckEventPayload.Payload();
	  dummyPayload.payload.settings = new { Counter = 1 };

	  mgr.Invoking(x => x.BroadcastGlobalSettings(dummyPayload));
	  var actions = mgr.GetAllActions();

	  var result = true;

	  foreach (var entry in actions)
	  {

		result = result && (entry.Value as StubAction).Counter == 1;
	  }
	  Assert.True(result);
	}

  }

}
