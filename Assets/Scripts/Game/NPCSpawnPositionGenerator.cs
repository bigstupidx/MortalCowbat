using UnityEngine;
using System.Collections.Generic;
using Vis;

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

	List<List<Interval>> intervals;

	public NPCSpawnPositionGenerator(int intervalCount, LevelFrame levelFrame, InGameCamera gameCamera)
	{
		float pathHeight = levelFrame.GetMaxY() - levelFrame.GetMinY();
		float intervalHeight = pathHeight / intervalCount;

		intervals = new List<List<Interval>> {
			new List<Interval>(intervalCount),
			new List<Interval>(intervalCount)
		};


		float actualY = levelFrame.GetMinY() + intervalHeight * 0.5f;
		for (int i = 0; i < intervalCount; ++i) {
			float posLeftX = gameCamera.GetPosition().x + gameCamera.GetWidth() * -1;
			float posRightX = gameCamera.GetPosition().x + gameCamera.GetWidth() * 1;
			float posY = actualY;
			actualY += intervalHeight;

			intervals[(int)Side.Left].Add(new Interval() {
				Position = new Vector3(posLeftX, posY, 0),
				LastSpawnTime = -1
			});

			intervals[(int)Side.Right].Add(new Interval() {
				Position = new Vector3(posRightX, posY, 0),
				LastSpawnTime = -1
			});
		}
	}

	public Vector3 Get(int side)
	{
		int rndIndex = Random.Range(0, intervals[(int)Side.Left].Count);
		return intervals[side == -1 ? (int)Side.Left : (int)Side.Right ][rndIndex].Position;
	}
}

