using System;

public static class Defs
{
	public enum CharacterType
	{
		Player,
		NPC
	}

	public static class Animations
	{
		public static string Walk = "walk";
		public static string Idle = "idle";
		public static string Attack = "attack";
		public static string SpecialAttack = "specialattack";
		public static string Play = "play";
		public static string Die = "die";
		public static string Hit = "hit";
		public static string Jump = "jump";
	}

	public static class Events
	{
		public const string SpecialAttackFinished = "specialattackfinished";
		public const string SpecialAttackHit = "specialattackhit";
		public const string AttackFinished = "attackfinished";
		public const string DieFinished = "dieFinished";
	}

	public static class Pois
	{
		public const string SpecialAttackEffect = "SpecialAttackEffect";
	}

	public enum HDirection 
	{
		Left = -1,
		Right = 1
	}

	public enum VDirection 
	{
		Up = 1,
		Down = -1
	}

	public enum State
	{
		Moving,
		Idle
	}
}

