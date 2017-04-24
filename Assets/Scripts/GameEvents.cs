using System;


public class GameEvents : IResetable
{
	public Action<int> LevelStarted;
	public Action<int, int> WaveStarted;
	public Action<int, int> WaveFinished;
	public Action<int, int> PlayerDied;
	public Action<int> AllWavesFinished;
	public Action<int> NPCLeftChanged;
	public Action<Character> OnCharacterHit;
	#region IResetable implementation

	public void Reset ()
	{
		//LevelStarted = null;
		//WaveStarted = null;
		//WaveFinished = null;
		//PlayerDied = null;
		//AllWavesFinished = null;
		//NPCLeftChanged = null;
	}

	#endregion
}

