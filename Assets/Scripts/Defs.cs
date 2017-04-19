using System;

public static class Defs
{
	public const float AnimationSpeed = 15.0f;

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
		public static string Kick = "kick";
		public static string SpecialAttack = "specialattack";
		public static string HeavyAttack = "heavyattack";
		public static string Play = "play";
		public static string Die = "die";
		public static string Hit = "hit";
		public static string Jump = "jump";
		public static string Fall = "fall";
	}

	public static class Events
	{
		public const string SpecialAttackFinished = "specialattackfinished";
		public const string SpecialAttackHit = "specialattackhit";
		public const string HeavyAttackHit = "heavyattackhit";
		public const string FastAttackHit = "fastattackhit";
		public const string AttackFinished = "attackfinished";
		public const string DieFinished = "diefinished";
		public const string JumpFinished = "jumpfinished";
		public const string FallFinished = "fallfinished";
		public const string HitFinished = "hitfinished";
		public const string AttackCharged = "attackcharged";
		public const string JumpStarted = "jumpstarted";
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

