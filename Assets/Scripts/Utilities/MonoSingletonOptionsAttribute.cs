using System;

namespace JTUtility
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class MonoSingletonOptions : Attribute
	{
		public bool AutoCreateIfNotFound { get; set; } = false;
	}
}


