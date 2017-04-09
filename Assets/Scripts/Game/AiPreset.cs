using System;
using UnityEngine;

namespace Ai
{
	[CreateAssetMenu(menuName = "NPC/Preset"), Serializable]
	public class AiPreset : ScriptableObject
	{
		public float FirstAttackDelay;
		public float AttackInterval;

		[Range (0.0f, 1.0f)]
		public float AttackRangeCoeficient;
		[Range (0.0f, 1.0f)]
		public float FastAttackProbability;
		[Range (0.0f, 1.0f)]
		public float HeavyAttackProbability;
		[Range (0.0f, 1.0f)]
		public float KickAttackProbability;

		[Range (0.0f, 1.0f)]
		public float HeavyAttackMinCharge;
		[Range (0.0f, 1.0f)]
		public float HeavyAttackMaxCharge;
	}
}