using System;
using UnityEngine;
using System.Collections.Generic;

public class NPCGenerator : MonoBehaviour
{
	public Action<Character> CharacterGenerated;
	public AnimationCurve spawnCurve;

	[SerializeField]
	List<GameObject> NpcPrefabs;


	float generateTime = 0;
	float startTime;

	void Awake()
	{
		startTime = Time.time;
	}


	void Update()
	{
		if (Time.time > generateTime) {
			GenerateRandomNPC();

			float delay = GetSpawnDelay(Time.time - startTime);
			Debug.Log(delay);
			generateTime = Time.time + delay;
		}	
	}

	void GenerateRandomNPC()
	{
		var prefab = NpcPrefabs[UnityEngine.Random.Range(0, NpcPrefabs.Count)];
		var npc = Instantiate(prefab);

		npc.transform.position = GetRandomPositionOutsideScreen();

		if (CharacterGenerated != null) {
			CharacterGenerated(npc.GetComponent<Character>());
		}
	}

	Vector3 GetRandomPositionOutsideScreen()
	{
		float camHeight = Camera.main.orthographicSize * 2;
		float camWidh = Camera.main.aspect * camHeight;

		float rndY = UnityEngine.Random.Range(
			Camera.main.transform.position.y - camHeight * 0.5f, 
			Camera.main.transform.position.y + camHeight * 0.5f);
		float rndX = Camera.main.transform.position.x + camWidh * (UnityEngine.Random.Range(0,2) == 0 ? -1 : 1);
		return new Vector3(rndX, rndY, 0);
	}

	float GetSpawnDelay(float elapsedTime)
	{
		return spawnCurve.Evaluate(Time.time - startTime);
	}

}

