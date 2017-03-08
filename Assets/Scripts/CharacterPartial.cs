#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public partial class Character : MonoBehaviour
{
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, settings.AttackDistance);
	}
}
#endif