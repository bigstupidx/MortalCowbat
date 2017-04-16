using System;
using System.Collections.Generic;

public class CharacterContext
{
	public EffectManager EffectManager { get; private set; }
	public Func<Limits> Limits { get; private set; }
	public List<Character> Characters { get; private set; }

	public CharacterContext(EffectManager effectManager, Func<Limits> limits, List<Character> characters)
	{
		Characters = characters;
		EffectManager = effectManager;
		Limits = limits;
	}
}

