using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Ui;
using Ai;

public partial class GameManager : MonoBehaviour
{
	[SerializeField]
	EffectManager effectManager;

	[SerializeField]
	NPCGenerator npcGenerator;

	[SerializeField]
	InGameUiRoot ui;

	[SerializeField]
	LevelFrame levelFrame;

	List<Character> characters;
	Character player;

	CharacterContext characterContext;
	AiStateMachineContext aiContext;
	Limits limits;

	public void Awake()
	{
		Initialize();
		GatherCharacters();
		InitializeCharacters();
	}

	public List<Character> Characters()
	{
		return characters;
	}

	void Initialize()
	{
		limits = new Limits() { 
			XMin = Camera.main.transform.position.x - (Camera.main.orthographicSize * 2 * Camera.main.aspect) * 0.5f,
			XMax = Camera.main.transform.position.x + (Camera.main.orthographicSize * 2 * Camera.main.aspect) * 0.5f,
			YMin = levelFrame.GetMinY(),
			YMax = levelFrame.GetMaxY()
		};

		npcGenerator.CharacterGenerated += OnCharacterGenerate;
		npcGenerator.Init(levelFrame);
		characterContext = CreateCharacterContext();
		aiContext = new AiStateMachineContext() { Characters = Characters };
	}

	void GatherCharacters()
	{
		characters = GameObject.FindObjectsOfType<Character>().ToList();
	}

	void InitializeCharacter(Character character)
	{
		if (character.Type == Defs.CharacterType.Player) {
			player = character;
			player.HealthChangedAction += ui.OnPlayerHealthChanged;
		} else {
			SetNpcStateMachine(character, aiContext);
		}
	
		character.Init(characterContext);
		character.AttackAction = OnCharacterAttack;
		character.SpecialAttackAction = OnCharacterSpecialAttack;
		character.DeathAction = OnCharacterDeath;
	}


	void InitializeCharacters()
	{
		characters.ForEach(InitializeCharacter);
	}

	void SetNpcStateMachine(Character character, AiStateMachineContext aiContext)
	{
		var aiStateMachine = character.GetComponent<AiStateMachine>();
		if (aiStateMachine != null) {
			if (aiContext == null) {
				aiContext = new AiStateMachineContext() { Characters = Characters };
			}
			aiStateMachine.Init(aiContext);
		}
	}

	float GetCharactersSqrDistance(Character character1, Character character2) 
	{
		return (character1.transform.position - character2.transform.position).sqrMagnitude;
	}

	List<Character> GetHitCharacters(Character attackingCharacter, Attack attack)
	{
		var charactersInRange = BattleUtils.GetCharactersInRange(characters, attack.Colliders);
		charactersInRange.Remove(attackingCharacter);
		charactersInRange.RemoveAll(x=>x.Type == attackingCharacter.Type);
		if (attack.Ranged) {
			return charactersInRange;
		} else {
			return charactersInRange.Count == 0 ? 
				new List<Character>() : 
				new List<Character>() { BattleUtils.SortCharactersByDistanceTo(charactersInRange, attackingCharacter.Position)[0]};
		}
	}

	bool IsCharacterInFronOfCharacter(Character who, Character from)
	{
		if (from.HDirection == Defs.HDirection.Left){
			return who.transform.position.x < from.transform.position.x;
		} else {
			return who.transform.position.x > from.transform.position.x;
		}
	}

	public CharacterContext CreateCharacterContext()
	{
		return new CharacterContext(effectManager, limits);
	}


}
