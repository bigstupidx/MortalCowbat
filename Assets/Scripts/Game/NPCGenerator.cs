using System;
using UnityEngine;
using Vis;


public class NPCGenerator : MonoBehaviour, IResetable, IPausable
{
	[SerializeField]
	GameLevels levels;

	public int WaveIndex { get { return waveIndex; }}

	public Action AllWavesFinishedAction;
	public Action<int, int>  WaveFinishedAction;
	public Action<int, int>  NextWaveAction;
	public Action<int> NPCLeftChagedAction;
	public Action<Character> CharacterGenerated;

	public bool Running { get; set; }
	public class Context
	{
		public LevelFrame LevelFrame;
		public InGameCamera GameCamera;
	}

	float timer;

	int generatedNPCCount;
	int killedNPCs;
	int waveIndex;
	int aliveNpcCount;
	bool paused;


	Context context;
	Level levelDef;
	Wave currentWave;

	public void Init(Context context, int level)
	{
		this.levelDef = levels.Levels[level - 1];
		this.context = context;
		Running = true;
		waveIndex = 0;
		currentWave = GameObject.Instantiate(levelDef.Waves[waveIndex]);
		timer = 0;

		// call actions
		NextWaveAction(waveIndex, levelDef.Waves.Count); // X/Y waves
		NPCLeftChagedAction(GetEnemiesLeft());
	}


	#region IResetable implementation
	public void Reset ()
	{
		//AllWavesFinishedAction = null;
		//NextWaveAction = null;
		//NPCLeftChagedAction = null;
		//WaveFinishedAction = null;
		//CharacterGenerated = null;
		waveIndex = 0;
		killedNPCs = 0;
		generatedNPCCount = 0;
		aliveNpcCount = 0;
		timer = 0;
	}
	#endregion


	public void OnNPCDeath()
	{
		bool wavesFinished = false;

		killedNPCs++;
		aliveNpcCount--;
		// all NPCs from wave killed
		if (currentWave.Events.Count == 0 && aliveNpcCount == 0) {
			WaveFinishedAction(waveIndex, WavesInCurrentLevel());
			killedNPCs = 0;
			generatedNPCCount = 0;
			aliveNpcCount = 0;
			waveIndex = Math.Min(waveIndex + 1, WavesInCurrentLevel());
			wavesFinished = waveIndex == WavesInCurrentLevel();
			if (!wavesFinished)
				currentWave = GameObject.Instantiate(levelDef.Waves[waveIndex]);
			else
				waveIndex--;
			
			NextWaveAction(waveIndex, WavesInCurrentLevel());
		}

		NPCLeftChagedAction(GetEnemiesLeft());

		if (wavesFinished) {
			Running = false;
			AllWavesFinishedAction();
		}
	}

	void Update()
	{
		if (Running) {
			var passedEvent = currentWave.Events.Find(x=>x.Time < timer);

			if (passedEvent != null) {
				GenerateNPCFromEvent(passedEvent);
				currentWave.Events.Remove(passedEvent);
			}
		}

		if (!paused) {
			timer += Time.deltaTime;
		}
	}

	void GenerateNPCFromEvent(Wave.TimeEvent evt)
	{
		var npc = Instantiate(evt.NPCPrefab);

		npc.transform.position = GetRandomPositionOutsideScreen(evt.Dir);

		if (CharacterGenerated != null) {
			CharacterGenerated(npc.GetComponent<Character>());
		}

		generatedNPCCount++;
		aliveNpcCount++;
	}

	Vector3 GetRandomPositionOutsideScreen(int side)
	{
		if (side == 0)
			side = Utils.GetRandomBool() ? -1 : 1;

		float camWidh = context.GameCamera.GetWidth();

		float rndY = UnityEngine.Random.Range(
			context.LevelFrame.GetMinY(),
			context.LevelFrame.GetMaxY());
		float rndX = context.GameCamera.GetPosition().x + camWidh * side;
		return new Vector3(rndX, rndY, 0);
	}

	int GetEnemiesLeft()
	{
		return currentWave.Events.Count + aliveNpcCount;
	}

	int WavesInCurrentLevel()
	{
		return levelDef.Waves.Count;
	}

	public void Pause ()
	{
		paused = true;
	}

	public void Resume ()
	{
		paused = false;
	}
}

