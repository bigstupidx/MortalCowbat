using UnityEngine;

namespace Battle.Comp
{
	public class Sound : CharacterComponent
	{
		[SerializeField]
		AudioSource audioSource;

		public void Play(AudioClip clip)
		{
			audioSource.PlayOneShot(clip);		
		}
	}
}