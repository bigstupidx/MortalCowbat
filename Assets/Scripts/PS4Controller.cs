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

	public int Index = 1;

	protected override void Init()
	{
		//Enabled = false;
//		jump = KeyCode.JoystickButton3; 		 // TRIANGLE
//		fastAtack = KeyCode.JoystickButton2; 	 // CIRCLE
//		heavyAttack = KeyCode.JoystickButton1; 	 // CROSS
//		specialAttack = KeyCode.JoystickButton0; //SQUARE
	}

	public void Update()
	{
		if (!Enabled)
			return;

//		if (Input.GetKey(left)) {
//			character.MoveH(-1);
//		} else if (Input.GetKey(right)) {
//			character.MoveH(1);	
//		}
//		if (Input.GetKey(up)) {
//			character.MoveV(1);
//		} else if (Input.GetKey(down)) {
//			character.MoveV(-1);	
//		}


		if (Input.GetButtonDown("Triangle"+Index)) {
			character.Jump();			
		}
		if (Input.GetButtonDown("Circle"+Index)) {
			character.FastAttack();			
		}
		if (Input.GetButtonDown("Square"+Index)) {
			character.HeavyAttack();			
		}

		if (Input.GetButtonUp("Square" + Index)) {
			character.ChargedAttackReleased();			
		}

		if (Input.GetButtonDown("Cross"+Index)) {
			character.AttackSpecial();			
		}


		var dir = GetAnalogueJoystickDirection();
		if (dir.sqrMagnitude > 0.01f * 0.01f) {
			character.AiMove(dir);
		}
	}

	Vector2 GetAnalogueJoystickDirection()
	{
		Vector2 dir = Vector2.zero;
		dir.x = Input.GetAxis("HorizontalJoy"+Index);
		dir.y = Input.GetAxis("VerticalJoy"+Index);
		//dir.x = Input.GetAxis("Horizontal");
		//dir.y = Input.GetAxis("Vertical");
		return dir;
	}
}

