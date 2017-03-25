using System;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class KeyboadController : MonoBehaviour
{
	[SerializeField]
	KeyCode left;

	[SerializeField]
	KeyCode right;

	[SerializeField]
	KeyCode up;

	[SerializeField]
	KeyCode down;

	[SerializeField]
	KeyCode specialAttack;

	[SerializeField]
	KeyCode fastAtack;

	[SerializeField]
	KeyCode jump;


	[SerializeField]
	Character character;

	void Update()
	{
		if (Input.GetKey(left)) {
			character.MoveH(-1);
		} else if (Input.GetKey(right)) {
			character.MoveH(1);	
		}
		if (Input.GetKey(up)) {
			character.MoveV(1);
		} else if (Input.GetKey(down)) {
			character.MoveV(-1);	
		}
		if (Input.GetKeyDown(jump)) {
			character.Jump();			
		}
		if (Input.GetKeyDown(fastAtack)) {
			character.FastAttack();			
		}
			if (Input.GetKeyDown(specialAttack)) {
			character.AttackSpecial();			
		}
	}
}

