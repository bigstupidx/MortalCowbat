using UnityEngine;


public class Moving : CharacterComponent
{
	public bool Paused { get; set; }

	float speedX;
	float speedY;

	public float SpeedX()
	{
		return speedX;
	}

	public float SpeedY()
	{
		return speedY;
	}

	public void SetSpeedX(float speed)
	{
		speedX = speed;
	}

	public void SetSpeedY(float speed)
	{
		speedY = speed;
	}

	public void Stop()
	{
		SetSpeedX(0);
		SetSpeedY(0);
	}

	public bool IsMoving()
	{
		return SpeedX() > 0 || SpeedY() > 0.0f;
	}

	public override void UpdateMe()
	{
		var pos = transform.position;
		pos.x += speedX * Time.deltaTime;
		pos.y += speedY * Time.deltaTime;
		transform.position = pos;
	}
}

