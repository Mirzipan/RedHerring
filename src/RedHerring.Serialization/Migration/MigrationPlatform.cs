using System;

namespace Migration
{
	public static class MigrationPlatform
	{
#if UNITY_2019_1_OR_NEWER // any unity version, this is just check if we are in unity environment
		public static Action<string> Log = UnityEngine.Debug.Log;
		public static Action<string> LogWarning = UnityEngine.Debug.LogWarning;
		public static Action<string> LogError = UnityEngine.Debug.LogError;

		public static Action<bool, string> AssertIsTrue = (condition, message) => UnityEngine.Assertions.Assert.IsTrue(condition, message);
		public static Action<bool, string> AssertIsFalse = (condition, message) => UnityEngine.Assertions.Assert.IsFalse(condition, message);
#else
		public static Action<string> Log        = Console.WriteLine;
		public static Action<string> LogWarning = message => Console.WriteLine($"Warning: {message}");
		public static Action<string> LogError   = message => Console.WriteLine($"Error: {message}");

		public static Action<bool, string> AssertIsTrue  = (condition, message) => System.Diagnostics.Trace.Assert(condition, message);
		public static Action<bool, string> AssertIsFalse = (condition, message) => System.Diagnostics.Trace.Assert(!condition, message);
#endif
	}
}