using System;
using UnityEngine;


namespace Battle.Comp
{
	public class Hit : CharacterComponent
	{
		[SerializeField]
		HitBlink hitBlink;

		int lastAttackHitHId;

		void Awake()
		{
			lastAttackHitHId = -1;
		}

		public bool CanBeHit(int attackId)
		{
			return (attackId ==-1 || lastAttackHitHId != attackId) && !GetComp<Moving>().Falling;
		}

		public void Perform(int attackId)
		{
			lastAttackHitHId = attackId;
		}
	}
}
