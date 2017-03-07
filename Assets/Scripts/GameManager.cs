using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class GameManager : MonoBehaviour
{

	List<Character> characters;

	public void Awake()
	{
		GatherCharacters();
		ConnectCharacters();
	}


	void GatherCharacters()
	{
		characters = GameObject.FindObjectsOfType<Character>().ToList();
	}

	void ConnectCharacters()
	{
		characters.ForEach(x=>x.AttackAction = OnCharacterAttack);
	}


	void OnCharacterAttack(Character attackingCharacter)
	{
		var hitCharacter = GetHitCharacter(attackingCharacter);
		if (hitCharacter != null) {
			hitCharacter.Hit(attackingCharacter.HDirection);
		}
	}

	float GetCharactersSqrDistance(Character character1, Character character2) 
	{
		return (character1.transform.position - character2.transform.position).sqrMagnitude;
	}

	Character GetHitCharacter(Character attackingCharacter)
	{
		var charactersInAttackDistance = new List<KeyValuePair<float, Character>>();
		for (int i = 0; i < characters.Count; ++i) {
			var otherCharacter = characters[i];
			if (otherCharacter != attackingCharacter) {
				float sqrDistance = GetCharactersSqrDistance(attackingCharacter, otherCharacter);
				if (sqrDistance < attackingCharacter.Settings.AttackDistance * attackingCharacter.Settings.AttackDistance) {
					charactersInAttackDistance.Add(new KeyValuePair<float, Character>(sqrDistance, otherCharacter));
				}
			}
		}
		charactersInAttackDistance.Sort((a, b) => a.Key < b.Key ? -1 : 1);
		return charactersInAttackDistance.Count == 0 ? null : charactersInAttackDistance[0].Value;
	}
}
