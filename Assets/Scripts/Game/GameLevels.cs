using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "NPC/GameLevels"), Serializable]
public class GameLevels : ScriptableObject
{
	public List<Level> Levels;
}

