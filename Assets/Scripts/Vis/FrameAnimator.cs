using UnityEngine;
using System.Collections.Generic;

namespace Vis
{
	[ExecuteInEditMode]
	public class FrameAnimator : MonoBehaviour
	{
		public List<Sprite> Sprites { get { return sprites; }}
		public SpriteRenderer sprRenderer;

		[SerializeField]
		List<Sprite> sprites;
	
		public float SpriteIndex;
        public bool Active;


		public int GetSpriteIndex()
		{
			return (int) SpriteIndex;
		}

		public void RemoveFrame(int index)
		{
			Sprites.RemoveAt(index);
		}

		public void MoveUp(int index)
		{
			if (index > 0) {
				var sprite = Sprites[index];
				Sprites[index] = Sprites[index - 1];
				Sprites[index - 1] = sprite;
			}
		}

		public void MoveDown(int index)
		{
			if (index < Sprites.Count - 1) {
				var sprite = Sprites[index];
				Sprites[index] = Sprites[index + 1];
				Sprites[index + 1] = sprite;
			}
		}

		public void Add()
		{
			Sprites.Add(null);
		}

		void LateUpdate()
		{
            if (Active) {
			    if (sprRenderer != null && GetSpriteIndex() < sprites.Count) {
				    sprRenderer.sprite = sprites[GetSpriteIndex()];
                }
		    }
        }
	}
}

