using System;
using UnityEngine;
using System.Collections;
using Battle.Comp;

public class StopEnemies : Effect
{
	
	public override void Run(GameObject go)
	{
		base.Run(go);
		StartCoroutine(PlayCoroutine(go));
	}

	IEnumerator PlayCoroutine(GameObject go)
	{
		var npcs = GameObject.FindGameObjectsWithTag("NPC");
		Array.ForEach(npcs, x=>x.GetComponent<Character>().GetComp<Pause>().Perform());
		yield break;
	}

	void OnDestroy()
	{
		var npcs = GameObject.FindGameObjectsWithTag("NPC");
		Array.ForEach(npcs, x=>x.GetComponent<Character>().GetComp<Pause>().Cancel());
	}
}

