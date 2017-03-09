

public class CharacterContext
{
	public EffectManager EffectManager { get; private set; }

	public CharacterContext(EffectManager effectManager)
	{
		EffectManager = effectManager;
	}
}

