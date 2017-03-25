using System;

public class CharacterContext
{
	public EffectManager EffectManager { get; private set; }
	public Func<Limits> Limits { get; private set; }

	public CharacterContext(EffectManager effectManager, Func<Limits> limits)
	{
		EffectManager = effectManager;
		Limits = limits;
	}
}

