using UnityEngine;

public class BloodyScreen : MonoBehaviour 
{
	[SerializeField]
	Vector2 scaleFor1136x640;

	void Start ()
	{
		var scale = transform.localScale;

		scale.x = Screen.currentResolution.width / 1136 * scaleFor1136x640.x;
		scale.y = Screen.currentResolution.height / 640 * scaleFor1136x640.y;
		transform.localScale = scale;

		var pos = Camera.main.transform.position;
		pos.z = 0;
		transform.position = pos;
	}	

	void Update () {
		
	}
}
