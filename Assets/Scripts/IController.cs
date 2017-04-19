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

	protected virtual void Init() {}
}


