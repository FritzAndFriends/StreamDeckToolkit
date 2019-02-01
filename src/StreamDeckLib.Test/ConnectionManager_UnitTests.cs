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
		public async Task ShouldRegisterEvent_WhenInitializedWithCorrectArgs() {

			// Arrange 
			var stub = new StubProxy() {
				InspectRegister = (e, uuid) => {
					Assert.Equal(StubProxy.TEST_EVENT, e);
					Assert.Equal(uuid, uuid);
				} 
			};

			// Act
				await ConnectionManager.Initialize(StubProxy.ValidCommandLineArguments, null, stub)
					.StartAsync(CancellationToken.None);

			// Assert

		}

  }
}
