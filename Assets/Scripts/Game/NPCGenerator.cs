using System;
using UnityEngine;
using Vis;
using Ai;
using System.Collections.Generic;
using Ge;
using System.ComponentModel;


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

	NPCSpawnPositionGenerator spawnPositionGenerator;

	float timer;
	int generatedNPCCount;
	int killedNPCs;
	int waveIndex;
	int aliveNpcCount;
	int eventId;
	bool paused;


	Context context;
	Level levelDef;
	Wave currentWave;

	float waveEndCheckTimer;
	const float WaveEndCheckInterval = 1.0f;

	public void Init(Context context, int level)
	{
		this.levelDef = levels.Levels[level - 1];
		this.context = context;
		Running = true;
		waveIndex = 0;
		currentWave = GameObject.Instantiate(levelDef.Waves[waveIndex]);
		timer = 0;
		waveEndCheckTimer = Time.time + WaveEndCheckInterval;
		// call actions
		NextWaveAction(waveIndex, levelDef.Waves.Count); // X/Y waves
		NPCLeftChagedAction(GetTotalEnemiesLeft());
		spawnPositionGenerator = new NPCSpawnPositionGenerator(5, context.LevelFrame, context.GameCamera);
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


	public void OnNPCDeath(Character character)
	{
		var dependentEvents = GetDependentEventOnEvent(character.Id, Wave.Event.Condition.NPCKilled);
		GenerateNPCFromEvents(dependentEvents);

		killedNPCs++;
		aliveNpcCount--;
		// all NPCs from wave killed
		bool wavesFinished = IsWaveFinished ();

		NPCLeftChagedAction(GetTotalEnemiesLeft());

		if (wavesFinished) {
			Running = false;
			AllWavesFinishedAction();
		}
	}

	void Update()
	{
		if (Running) {
			var passedEvents = GetTimePassedEvents();
			GenerateNPCFromEvents(passedEvents);
		
			if (Time.time > waveEndCheckTimer) {
				waveEndCheckTimer = Time.time + WaveEndCheckInterval;
				bool wavesFinished = IsWaveFinished ();
				if (wavesFinished) {
					Running = false;
					AllWavesFinishedAction();
				}
			}
		}

		if (!paused) {
			timer += Time.deltaTime;
		}
	}

	void GenerateNPCFromEvents(List<Wave.Event> evts)
	{
		for (int i = 0; i < evts.Count; ++i) {
			var evt = evts[i];
			evt.RuntimeData.ProcessedTime = Time.time;
			evt.RuntimeData.Processed = true;
			evt.RuntimeData.Id = eventId;
			var npc = Instantiate(evt.SpawnData.NPCPrefab);
			npc.transform.position = GetRandomPositionOutsideScreen(evt.SpawnData.Dir);
			npc.GetComponent<AiStateMachine>().Preset = evt.SpawnData.AiPreset;
			npc.GetComponent<CharacterSettings>().MovingSpeed = evt.SpawnData.Speed;
			npc.GetComponent<CharacterSettings>().Health = evt.SpawnData.HP;
			npc.GetComponent<Character>().Id = eventId;
			eventId++;

			if (CharacterGenerated != null) {
				CharacterGenerated(npc.GetComponent<Character>());
			}

			generatedNPCCount++;
			aliveNpcCount++;
		}
	}


	public Wave.Event FindEvent(int id)
	{
		var  eventList = new List<Wave.Event>();
		for (int i = 0; i < currentWave.Events.Count; ++i) {
			eventList.Add(currentWave.Events[i]);
		}

		while (eventList.Count > 0) {
			if (eventList[0].RuntimeData.Processed && eventList[0].RuntimeData.Id == id)
				return eventList[0];

			eventList.AddRange(eventList[0].DependentEvents);
			eventList.RemoveAt(0);
		}
		return null;
	}

	public List<Wave.Event> GetDependentEventOnEvent(int id, Wave.Event.Condition trigger)
	{
		var result = new List<Wave.Event>();
		var evt = FindEvent(id);
		result = evt.DependentEvents.FindAll(x=>!x.RuntimeData.Processed && x.Trigger == trigger);
		return result;
	}

	Vector3 GetRandomPositionOutsideScreen(int side)
	{
		if (side == 0)
			side = Utils.GetRandomBool() ? -1 : 1;
		return spawnPositionGenerator.Get(side);
//		float camWidh = context.GameCamera.GetWidth();
//
//		float rndY = UnityEngine.Random.Range(
//			context.LevelFrame.GetMinY(),
//			context.LevelFrame.GetMaxY());
//		float rndX = context.GameCamera.GetPosition().x + camWidh * side;
//		return new Vector3(rndX, rndY, 0);
	}

	bool IsWaveFinished ()
	{
		bool wavesFinished = false;
		if (GetTotalEnemiesLeft() == 0) {
			WaveFinishedAction (waveIndex, WavesInCurrentLevel ());
			killedNPCs = 0;
			generatedNPCCount = 0;
			aliveNpcCount = 0;
			waveIndex = Math.Min (waveIndex + 1, WavesInCurrentLevel ());
			wavesFinished = waveIndex == WavesInCurrentLevel ();
			if (!wavesFinished)
				currentWave = GameObject.Instantiate (levelDef.Waves [waveIndex]);
			else
				waveIndex--;
			NextWaveAction (waveIndex, WavesInCurrentLevel ());
		}
		return wavesFinished;
	}



	int GetTotalEnemiesLeft()
	{
		int enemiesLeft = 0;
		for (int i = 0; i < currentWave.Events.Count; ++i) {
			GetEnemiesLeft(currentWave.Events[i], ref enemiesLeft);
		}
		return enemiesLeft + aliveNpcCount;
	}

	void GetEnemiesLeft(Wave.Event evt, ref int enemiesLeft)
	{
		if (!evt.RuntimeData.Processed) {
			enemiesLeft++;
		}
		for (int i = 0; i < evt.DependentEvents.Count; ++i) {
			GetEnemiesLeft(evt.DependentEvents[i], ref enemiesLeft);
		}
	}

	int WavesInCurrentLevel()
	{
		return levelDef.Waves.Count;
	}

	List<Wave.Event> GetTimePassedEvents ()
	{
		return currentWave.Events.FindAll(x => x.Time < timer && !x.RuntimeData.Processed);
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

