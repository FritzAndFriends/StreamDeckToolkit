using System;
using Xunit;
using FluentAssertions;

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
  }
}
