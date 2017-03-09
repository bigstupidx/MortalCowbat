using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public static class BattleUtils
{
	public static bool IsPointInside(Vector3 position, List<Collider2D> colliders)
	{
		return colliders.Any(x=>x.OverlapPoint(position));
	}

	public static List<Character> GetCharactersInRange(List<Character> characters, List<Collider2D> colliders)
	{
		var charactersInRange = new List<Character>();
		for (int i = 0; i < characters.Count; ++i) {
			if (IsPointInside(characters[i].Position, colliders)) {
				charactersInRange.Add(characters[i]);
			}
		}
		return charactersInRange;
	}

	public static List<Character> SortCharactersByDistanceTo(List<Character> characters, Vector3 position)
	{
		var copy = new List<Character>(characters);

		copy.Sort((a, b) =>  {

			var sqrDistA = (a.Position - position).sqrMagnitude;
			var sqrDistB = (b.Position - position).sqrMagnitude;

			return sqrDistA.CompareTo(sqrDistB);
		});
		return copy;
	}
}

