using System;

public static class Defs
{
	public static class Animations
	{
		public static string Walk = "walk";
		public static string Idle = "idle";
		public static string Punch = "punch";
	}

	public static class Events
	{
		public static string PunchFinshed = "punchfinish";
	}

	public enum Direction 
	{
		Left = -1,
		Right = 1
	}

	public enum State
	{
		Moving,
		Idle
	}
}

