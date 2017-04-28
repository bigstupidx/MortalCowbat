using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Attack : MonoBehaviour
{
	public bool Ranged { get { return ranged; }}
	public bool ShiftHitEnemy { get { return shiftHitEnemy; }}
	public float ShiftHitEnemyDistance { get { return shiftHitDistance; }}

	public bool EnemyFalls { get { return enemyFalls; }}

	public int AttackPoints { get { return attackPoints; }}
	public AudioClip Sfx { get { return attackSfx; }}
	public List<EffectDescriptor> Effects { get { return effects; }}
	public List<Collider2D> Colliders { get { return colliders; }}
	public List<EffectDescriptor> HitEffects { get { return hitEffects; }}

	[SerializeField]
	List<Collider2D> colliders;

	[SerializeField]
	bool ranged;

	[SerializeField]
	bool shiftHitEnemy;

	[SerializeField]
	float shiftHitDistance = 1.0f;

	[SerializeField]
	bool enemyFalls;

	[SerializeField]
	int attackPoints;

	[SerializeField]
	AudioClip attackSfx;


	[SerializeField]
	List<EffectDescriptor> effects;

	[SerializeField]
	List<EffectDescriptor> hitEffects;

	void Awake()
	{
		colliders = GetComponents<Collider2D>().ToList();
	}

}
