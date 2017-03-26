using UnityEngine;
using System.Collections.Generic;

public class Attack : MonoBehaviour
{
	public bool Ranged { get { return ranged; }}
	public bool ShiftHitEnemy { get { return shiftHitEnemy; }}

	public int AttackPoints { get { return attackPoints; }}
	public AudioClip Sfx { get { return attackSfx; }}
	public List<EffectDescriptor> Effects { get { return effects; }}
	public List<Collider2D> Colliders { get { return colliders; }}
	public EffectDescriptor HitEffect { get { return hitEffect; }}

	[SerializeField]
	List<Collider2D> colliders;

	[SerializeField]
	bool ranged;

	[SerializeField]
	bool shiftHitEnemy;

	[SerializeField]
	int attackPoints;

	[SerializeField]
	AudioClip attackSfx;


	[SerializeField]
	List<EffectDescriptor> effects;

	[SerializeField]
	EffectDescriptor hitEffect;

}
