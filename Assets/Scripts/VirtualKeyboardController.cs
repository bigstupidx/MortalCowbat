using UnityEngine;


public class VirtualKeyboardController : Controller
{
	[SerializeField]
	VirtualJoypad joypad;

	public void AttachCharacter(Character controllerCharacter)
	{
		character = controllerCharacter;
	
		#if UNITY_EDITOR
		gameObject.SetActive(false);
		#endif

	}

	public void OnJumpButtonDown()
	{
		if (!Enabled)
			return;
		
		character.Jump();	
	}

	public void OnBasicAttackButtonDown()
	{
		if (!Enabled)
			return;
		
		character.FastAttack();	
	}

	public void OnHeavyAttackButtonDown()
	{
		if (!Enabled)
			return;
		
		character.HeavyAttack();
	}

	public void OnHeavyAttackButtonUp()
	{
		if (!Enabled)
			return;
		
		character.ChargedAttackReleased();			
	}

	public void OnSpecialAttackButtonDown()
	{
		if (!Enabled)
			return;
		
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
		if (!Enabled)
			return;
	
		var dir = joypad.Direction();
		if (dir.sqrMagnitude > 0.01f)
			character.AiMove(dir);
	}
}

