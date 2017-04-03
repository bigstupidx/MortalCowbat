using System;


public class GameEvents
{
	public Action<int> LevelStarted;
	public Action<int, int> WaveStarted;
	public Action<int, int> WaveFinished;
	public Action<int, int> PlayerDied;
	public Action<int> AllWavesFinished;
	public Action<int> NPCLeftChanged;
}

