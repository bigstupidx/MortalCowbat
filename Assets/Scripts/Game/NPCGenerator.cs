using System;
using UnityEngine;
using System.Collections.Generic;


public class NPCGenerator : MonoBehaviour
{
	public Action<int, int>  NextWaveAction;
	public Action<int> NPCLeftChagedAction;


	public Action<Character> CharacterGenerated;
	public AnimationCurve spawnCurve;

	[SerializeField]
	WaveSettings waves;

	[SerializeField]
	List<GameObject> NpcPrefabs;

	float generateTime = 0;
	float startTime;
	LevelFrame levelFrame;

	int generatedNPCCount;
	int killedNPCs;
	int waveIndex;

	void Awake()
	{
		startTime = Time.time;
	}

	public void Init(LevelFrame levelFrame)
	{
		this.levelFrame = levelFrame;

		NextWaveAction(waveIndex + 1, waves.EnemiesInWave.Count);
		NPCLeftChagedAction(GetEnemiesLeft());
	}

	public void OnNPCDeath()
	{
		killedNPCs++;
		if (killedNPCs == waves.EnemiesInWave[waveIndex]) {
			killedNPCs = 0;
			generatedNPCCount = 0;
			waveIndex++;
			NextWaveAction(waveIndex + 1, waves.EnemiesInWave.Count);
		}
		NPCLeftChagedAction(GetEnemiesLeft());
	}

	void Update()
	{
		if (Time.time > generateTime) {
			GenerateRandomNPC();

			float delay = GetSpawnDelay(Time.time - startTime);
			generateTime = Time.time + delay;
		}	
	}

	void GenerateRandomNPC()
	{
		if (generatedNPCCount < waves.EnemiesInWave[waveIndex]) {
			var prefab = NpcPrefabs[UnityEngine.Random.Range(0, NpcPrefabs.Count)];
			var npc = Instantiate(prefab);

			npc.transform.position = GetRandomPositionOutsideScreen();

			if (CharacterGenerated != null) {
				CharacterGenerated(npc.GetComponent<Character>());
			}

			generatedNPCCount++;
		}
	}

	Vector3 GetRandomPositionOutsideScreen()
	{
		float camHeight = Camera.main.orthographicSize * 2;
		float camWidh = Camera.main.aspect * camHeight;

		float rndY = UnityEngine.Random.Range(
			levelFrame.GetMinY(),
			levelFrame.GetMaxY());
		float rndX = Camera.main.transform.position.x + camWidh * (UnityEngine.Random.Range(0,2) == 0 ? -1 : 1);
		return new Vector3(rndX, rndY, 0);
	}

	float GetSpawnDelay(float elapsedTime)
	{
		return spawnCurve.Evaluate(Time.time - startTime);
	}

	int GetEnemiesLeft()
	{
		return waves.EnemiesInWave[waveIndex] - killedNPCs;
	}
}

