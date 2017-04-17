using UnityEngine;
using System.Collections.Generic;
using Vis;
using System;

public class NPCSpawnPositionGenerator
{
	class Interval
	{
		public Vector3 Position;
		public float LastSpawnTime;
	}

	enum Side {
		Left,
		Right
	}

	List<Interval> leftIntervals;
	List<Interval> rightIntervals;


	public NPCSpawnPositionGenerator(int intervalCount, LevelFrame levelFrame, InGameCamera gameCamera)
	{
		float pathHeight = levelFrame.GetMaxY() - levelFrame.GetMinY();
		float intervalHeight = pathHeight / intervalCount;

		leftIntervals = new List<Interval>(intervalCount);
		rightIntervals = new List<Interval>(intervalCount);

	
		float actualY = levelFrame.GetMinY() + intervalHeight * 0.5f;
		for (int i = 0; i < intervalCount; ++i) {
			float posLeftX = gameCamera.GetPosition().x - gameCamera.GetWidth() * -1;
			float posRightX = gameCamera.GetPosition().x + gameCamera.GetWidth() *  1;
			float posY = actualY;
			actualY += intervalHeight;

			leftIntervals.Add(new Interval() {
				Position = new Vector3(posLeftX, posY, 0),
				LastSpawnTime = -1
			});

			rightIntervals.Add(new Interval() {
				Position = new Vector3(posRightX, posY, 0),
				LastSpawnTime = -1
			});
		}
	}

	public Vector3 Get(int side)
	{
		var intervals = side == -1 ? leftIntervals : rightIntervals;
		int rndIndex = UnityEngine.Random.Range(0, intervals.Count);
		return intervals[rndIndex].Position;
	}
}

