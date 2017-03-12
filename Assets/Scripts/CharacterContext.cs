

public class CharacterContext
{
	public EffectManager EffectManager { get; private set; }
	public Limits Limits { get; private set; }

	public CharacterContext(EffectManager effectManager, Limits limits)
	{
		EffectManager = effectManager;
		Limits = limits;
	}
}

