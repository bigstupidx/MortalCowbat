using System;
using UnityEngine;
using System.Collections.Generic;
using Ai;


namespace Ge
{
	[CreateAssetMenu(menuName = "NPC/Wave"), Serializable]
	public class Wave : ScriptableObject
	{
		[Serializable]
		public class Event
		{
			public float Time;
			public enum Condition
			{
				Time,
				NPCKilled,
				NPCHit
			}

			public Condition Trigger = Condition.Time;
			public SpawnData SpawnData = new SpawnData();
			public RuntimeData RuntimeData = new RuntimeData();
			public List<Event> DependentEvents = new List<Event>();
	
			public Event Clone ()
			{
				return new Event {
					Trigger = this.Trigger,
					Time = this.Time,
					SpawnData = this.SpawnData.Clone()
				};
			}
		}


		public class RuntimeData
		{
			public bool Processed;
			public float ProcessedTime;
			public int Id;
		}

		[Serializable]
		public class SpawnData
		{
			public GameObject NPCPrefab;
			public AiPreset AiPreset;
			public int Dir;
			public int HP;
			public int Speed;
			public float Delay;
		
			public SpawnData Clone ()
			{
				return new SpawnData {
					NPCPrefab = this.NPCPrefab,
					AiPreset = this.AiPreset,
					Dir = this.Dir,
					HP = this.HP,
					Speed = this.Speed
				};
			}
		}


		public List<Event> Events = new List<Event>();
	}
}

