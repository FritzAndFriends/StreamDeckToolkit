using System;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

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
					                             Assert.Equal(uuid,                 uuid);
				                             }
			           };

			// Act
			var tokenSource = new CancellationTokenSource();
			var task = ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments, null, stub)
			                       .StartAsync();

			// Assert
			Assert.Null(task.Exception);
			tokenSource.Cancel();

		}

		//[Fact]
		//public async Task ShouldThrowArgumentNullException_WhenRegisteringANullAction()
		//{
		//	//
		//	// Arrange
		//	//
			

		//	Func<ConnectionManager> action = () => ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
		//	                                                        .RegisterAction(null);

		//	// 
		//	// Act
		//	//
		//	action.

		//	//
		//	// Assert
		//	//
		//	Should().Throw<ArgumentNullException>("A null BaseStreamDeckAction can not and should not be registered.");
		//}


		//[Fact]
		//public async Task ShouldThrowArgumentException_WhenRegisteringAnActionWithoutAUUID()
		//{
		//	//
		//	// Arrange
		//	//

		//	Func<ConnectionManager> action = () => ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
		//	                                                        .RegisterAction(new StubAction(string.Empty));

		//	//
		//	// Act
		//	//
		//	action.

		//	//
		//	// Assert
		//	//
		//	Should().Throw<IncompleteActionDefinitionException>("An action must specify a non-blank or empty UUID");
		//}

		//[Fact]
		//public async Task ShouldThrowDuplicateActionRegistrationException_WhenRegisteringMultipleInstancesOfTheSameAction()
		//{
		//	//
		//	// Arrange
		//	//


		//	Func<ConnectionManager> action = () => ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
		//	                                                        .RegisterAction(new StubAction("UniqueID"))
		//	                                                        .RegisterAction(new StubAction("UniqueID"));

		//	//
		//	// Assert
		//	//

		//	//TODO: Wording? "unique UUID" is like saying "PIN number". It's "just" a test, but still syntactically incorrect. 
		//	action.Should()
		//	.Throw<DuplicateActionRegistrationException>("each BaseStreamDeckAction type should have its own unique UUID.");
		//}


		//[Fact]
		//public async Task ShouldNotThrowAnyExceptions_WhenRegistringMultipleUniqueActions()
		//{
		//	//
		//	// Arrange
		//	//


		//	Func<ConnectionManager> action = () => ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
		//		.RegisterAction(new StubAction("Unique_Action_ID_1"))
		//		.RegisterAction(new StubAction("Unique_Action_ID_2"));


		//		//
		//		// Assert
		//		//

		//		//TODO: Wording? "unique UUID" is like saying "PIN number". It's "just" a test, but still syntactically incorrect. 
		//		action.Should()
		//		.NotThrow("registering multiple unique actions is valid");
		//}

		//[Fact]
		//public async Task ShouldRegisterAllActions_WhenRegisteringAllActions()
		//{

		//	Func<ConnectionManager> action = () => ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments)
		//.RegisterAllActions(this.GetType().Assembly);

		//	action.Should()
		//		.NotThrow("No problems when registering All Actions");


		//}


	}

}
