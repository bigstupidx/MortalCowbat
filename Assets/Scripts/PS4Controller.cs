using UnityEngine;

[RequireComponent(typeof(Character))]
public class PS4Controller : Controller
{
	KeyCode left;
	KeyCode right;
	KeyCode up;
	KeyCode down;

	KeyCode specialAttack;
	KeyCode fastAtack;
	KeyCode heavyAttack;
	KeyCode jump;

	protected override void Init()
	{
		Enabled = false;
		jump = KeyCode.JoystickButton3; 		 // TRIANGLE
		fastAtack = KeyCode.JoystickButton2; 	 // CIRCLE
		heavyAttack = KeyCode.JoystickButton1; 	 // CROSS
		specialAttack = KeyCode.JoystickButton0; //SQUARE
	}

	void Update()
	{
		if (!Enabled)
			return;

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
		if (Input.GetKeyDown(heavyAttack)) {
			character.HeavyAttack();			
		}

		if (Input.GetKeyUp(heavyAttack)) {
			character.ChargedAttackReleased();			
		}

		if (Input.GetKeyDown(specialAttack)) {
			character.AttackSpecial();			
		}


		var dir = GetAnalogueJoystickDirection();
		if (dir.sqrMagnitude > 0.01f) {
			character.AiMove(dir);
		}
	}

	Vector2 GetAnalogueJoystickDirection()
	{
		Vector2 dir = Vector2.zero;
		dir.x = Input.GetAxis("Horizontal");
		dir.y = Input.GetAxis("Vertical");

		return dir;
	}
}

