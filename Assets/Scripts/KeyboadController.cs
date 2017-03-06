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
	KeyCode punch;


	[SerializeField]
	Character character;

	void Update()
	{
		if (Input.GetKeyDown(left)) {
			character.Move(Defs.Direction.Left);	
		}
		else if (Input.GetKeyDown(right)) {
			character.Move(Defs.Direction.Right);	
		}
		else if (Input.GetKeyUp(left) && !Input.GetKey(right)) {
			character.StopMoving();	
		}
		else if (Input.GetKeyUp(right) && !Input.GetKey(left)) {
			character.StopMoving();	
		}
		else if (Input.GetKeyDown(punch)) {
			character.Punch();
		}
	}
}

