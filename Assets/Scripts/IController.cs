using UnityEngine;

public class Controller : MonoBehaviour
{
	public bool Enabled { get; set; }
	void Awake()
	{
		Enabled = true;
	}
}


