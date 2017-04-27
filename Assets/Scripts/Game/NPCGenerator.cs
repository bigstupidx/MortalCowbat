using System;
using UnityEngine;
using Vis;
using Ai;
using System.Collections.Generic;
using Ge;
using System.Collections;


public class NPCGenerator : MonoBehaviour, IResetable, IPausable
{
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
	WaveManager waveManager;

	float timer;
	int generatedNPCCount;
	int killedNPCs;
	int waveIndex;
	int aliveNpcCount;
	int eventId;
	bool paused;


	Context context;
	Level levelDef;

	float waveEndCheckTimer;
	const float WaveEndCheckInterval = 1.0f;

	public void Init(Context context, int level, Level levelDef)
	{
		this.levelDef = levelDef;
		this.context = context;
		Running = true;
		waveIndex = 0;
		var currentWave = GameObject.Instantiate(levelDef.Waves[waveIndex]);
		waveManager = new WaveManager(currentWave);

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
		waveIndex = 0;
		killedNPCs = 0;
		generatedNPCCount = 0;
		aliveNpcCount = 0;
		timer = 0;
	}
	#endregion


	public void OnNPCHit(Character character)
	{
		var dependentEvents = waveManager.GetDependentEventsOnEvent(character.Id, Wave.Event.Condition.NPCHit);
		GenerateNPCFromEvents(dependentEvents);
	}

	public void OnNPCDeath(Character character)
	{
		var dependentEvents = waveManager.GetDependentEventsOnEvent(character.Id, Wave.Event.Condition.NPCKilled);
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
			var passedEvents = waveManager.GetTimePassedEvents(timer);
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
			evts[i].RuntimeData.Processed = true;
			StartCoroutine(CreateNPC (evts[i], evts[i].SpawnData.Delay));
			generatedNPCCount++;
			aliveNpcCount++;
		}
	}

	IEnumerator CreateNPC (Wave.Event evt, float delay)
	{
		if (delay > 0) {
			yield return new WaitForSeconds(delay);
		}

		evt.RuntimeData.Id = eventId;
		evt.RuntimeData.ProcessedTime = Time.time;
		var npc = Instantiate (evt.SpawnData.NPCPrefab);
		npc.transform.position = GetRandomPositionOutsideScreen (evt.SpawnData.Dir);
		npc.GetComponent<AiStateMachine> ().Preset = evt.SpawnData.AiPreset;
		npc.GetComponent<CharacterSettings> ().MovingSpeed = evt.SpawnData.Speed;
		npc.GetComponent<CharacterSettings> ().Health = evt.SpawnData.HP;
		npc.GetComponent<Character> ().Id = eventId;
		eventId++;
		if (CharacterGenerated != null) {
			CharacterGenerated (npc.GetComponent<Character> ());
		}

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
			if (!wavesFinished) {
				//currentWave = GameObject.Instantiate (levelDef.Waves [waveIndex]);
			}
			else
				waveIndex--;
			NextWaveAction (waveIndex, WavesInCurrentLevel ());
		}
		return wavesFinished;
	}

	int GetTotalEnemiesLeft()
	{
		var notProcessedEventsCount = waveManager.GetTotalNotProcessedEventsCount();
		return notProcessedEventsCount + aliveNpcCount;
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

