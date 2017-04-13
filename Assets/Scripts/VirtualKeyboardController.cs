using UnityEngine;
using UnityEngine.UI;


public class VirtualKeyboardController : MonoBehaviour
{
	[SerializeField]
	VirtualJoypad joypad;

	Character character;

	public void AttachCharacter(Character controllerCharacter)
	{
		character = controllerCharacter;
	}

	public void OnJumpButtonDown()
	{
		character.Jump();	
	}

	public void OnBasicAttackButtonDown()
	{
		character.FastAttack();	
	}

	public void OnHeavyAttackButtonDown()
	{
		character.HeavyAttack();
	}

	public void OnHeavyAttackButtonUp()
	{
		character.ChargedAttackReleased();			
	}

	public void OnSpecialAttackButtonDown()
	{
		character.AttackSpecial();	
	}

	public void OnSpecialAttackButtonUp()
	{}

	public void OnBasicAttackButtonUp()
	{}

	public void OnJumpButtonUp()
	{}


	void Update()
	{
		var dir = joypad.Direction();
		if (dir.sqrMagnitude > 0.01f)
			character.AiMove(dir);
//		if (joypad.LeftPressed()) {
//			character.MoveH(-1);
//		} else if (joypad.RightPressed()) {
//			character.MoveH(1);	
//		}
//		if (joypad.UpPressed()) {
//			character.MoveV(1);
//		} else if (joypad.DownPressed()) {
//			character.MoveV(-1);	
//		}
	}
}

