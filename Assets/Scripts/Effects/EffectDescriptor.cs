using System.Collections.Generic;

[System.Serializable]
public class EffectDescriptor
{
	public Effect Effect;
	public string Container;
	public bool InWorldSpace;
	public List<string> CustomData;
}