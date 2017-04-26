using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "NPC/Level"), Serializable]
public class Level : ScriptableObject
{
	public string Environment;
	public int Frame;
	public List<Ge.Wave> Waves;
}

