using System;

namespace StreamDeckLib
{
	/// <summary>
	/// An extension of the <see cref="StreamDeckActionAttribute"/>, so that we don't immediately break people who update
	/// their NuGet packages...
	/// </summary>
	[Obsolete("This is being replaced by StreamDeckActionAttribute, use that instead.")]
	public class ActionUuidAttribute : StreamDeckActionAttribute
	{

	}
}
