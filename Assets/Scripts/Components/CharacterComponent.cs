using UnityEngine;


public class CharacterComponent : MonoBehaviour
{
	ComponentHolder componentHolder;

	public void Init(ComponentHolder componentHolder)
	{
		this.componentHolder = componentHolder;
	}

	protected T GetComp<T>() where T : CharacterComponent
	{
		return componentHolder.Get<T>();
	}

	public virtual void UpdateMe()
	{}
}

