
public interface ICharacter
{
	void MoveH(int dir);
	void MoveV(int dir);
	void FastAttack();
	void HeavyAttack();

	void Pause();
	void Resume();
}

