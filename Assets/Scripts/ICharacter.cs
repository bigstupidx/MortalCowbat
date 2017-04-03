
public interface ICharacter
{
	void MoveH(int dir);
	void MoveV(int dir);
	void Stop();
	void FastAttack();
	void HeavyAttack();
	int GetFlip();

	void Pause();
	void Resume();
}

