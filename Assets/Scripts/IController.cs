using UnityEngine;

public class Controller : MonoBehaviour
{
	public bool Enabled { get; set; }
	protected Character character;

	void Awake()
	{
		character = GetComponent<Character>();
		Enabled = true;
		Init();
	}

	public virtual void UpdateMe() {}
	protected virtual void Init() {}

}


