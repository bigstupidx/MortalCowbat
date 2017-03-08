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
	KeyCode attack;

	[SerializeField]
	KeyCode specialAttack;

	[SerializeField]
	Character character;

	void Update()
	{
		if (Input.GetKeyDown(left)) {
			character.MoveHorizontally(Defs.HDirection.Left);	
		}
		else if (Input.GetKeyDown(right)) {
			character.MoveHorizontally(Defs.HDirection.Right);	
		}
		if (Input.GetKeyDown(up)) {
			character.MoveVertically(Defs.VDirection.Up);	
		}
		else if (Input.GetKeyDown(down)) {
			character.MoveVertically(Defs.VDirection.Down);	
		}
		if ((Input.GetKeyUp(right) && !Input.GetKey(left)) || (Input.GetKeyUp(left) && !Input.GetKey(right))) {
			character.StopMovingHorizontally();	
		}
		if ((Input.GetKeyUp(up) && !Input.GetKey(down)) || (Input.GetKeyUp(down) && !Input.GetKey(up))) {
			character.StopMovingVertically();	
		}
		if (Input.GetKeyDown(attack)) {
			character.Attack();
		}
		else if (Input.GetKeyDown(specialAttack)) {
			character.SpecialAttack();
		}
	}
}

