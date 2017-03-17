using System;
using UnityEngine;
using System.Collections.Generic;
using Vis;


public class NPCGenerator : MonoBehaviour, IResetable
{
	public Action<int, int>  NextWaveAction;
	public Action<int> NPCLeftChagedAction;
	public Action<Character> CharacterGenerated;
	public AnimationCurve spawnCurve;

	public class Context
	{
		public LevelFrame LevelFrame;
		public InGameCamera GameCamera;
	}

	[SerializeField]
	WaveSettings waves;

	[SerializeField]
	List<GameObject> NpcPrefabs;

	float generateTime = 0;
	float startTime;

	int generatedNPCCount;
	int killedNPCs;
	int waveIndex;

	Context context;

	public void Init(Context context)
	{
		this.context = context;

		NextWaveAction(waveIndex + 1, waves.EnemiesInWave.Count);
		NPCLeftChagedAction(GetEnemiesLeft());
	}


	#region IResetable implementation
	public void Reset ()
	{
		NextWaveAction = null;
		NPCLeftChagedAction = null;
		CharacterGenerated = null;
		waveIndex = 0;
		generateTime = 0;
		killedNPCs = 0;
		generatedNPCCount = 0;
		startTime = 0;
	}
	#endregion


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
		float camWidh = context.GameCamera.GetWidth();

		float rndY = UnityEngine.Random.Range(
			context.LevelFrame.GetMinY(),
			context.LevelFrame.GetMaxY());
		float rndX = context.GameCamera.GetPosition().x + camWidh * (Utils.GetRandomBool() ? -1 : 1);
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

