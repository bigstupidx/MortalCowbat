using System;
using UnityEngine;
using System.Collections.Generic;
using Ai;

[CreateAssetMenu(menuName = "NPC/Wave"), Serializable]
public class Wave : ScriptableObject
{
	[Serializable]
	public class TimeEvent
	{
		public TimeEvent(TimeEvent evt)
		{
			Time = evt.Time	;
			NPCPrefab = evt.NPCPrefab;
			AiPreset = evt.AiPreset;
			Dir = evt.Dir;
		}

		public TimeEvent()
		{}

		public float Time;
		public GameObject NPCPrefab;
		public AiPreset AiPreset;
		public int Dir;
		public int HP;
		public int Speed;
	}

	public List<TimeEvent> Events;
}

