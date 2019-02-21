using System;
using Xunit;

namespace StreamDeckLib.Test
{

	public class ActionManagerShould
	{

		[Fact]
		public void ThrowActionNotRegisteredException_WhenRetrievingAnUnregisteredAction()
		{
			// Arrange
			using (var SUT = new ActionManager(null))
			{
				// Act

				// Assert
				Assert.Throws<ActionNotRegisteredException>(() => SUT.GetAction("UUID1"));

			}
		}

		[Fact]
		public void  ShowOneSingleActionRegistered_WhenRegisteringOnlyOneAction()
		{
			//
			// Arrange
			//


			using (var SUT = new ActionManager(null))
			{
				// 
				// Act
				//

				SUT.RegisterActionInstance("UUID1", new StubAction());

				var act = SUT.GetActionInstance("UUID1");

				Assert.NotNull(act);
			}

			//
			// Assert
			//

		}

	}
}
