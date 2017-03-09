#if UNITY_EDITOR
using UnityEngine;

public partial class Character
{
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, settings.AttackDistance);
	}
}
#endif