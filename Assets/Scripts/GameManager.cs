using UnityEngine;
using System.Collections.Generic;
using Ui;
using Ai;
using Vis;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public partial class GameManager : MonoBehaviour, IResetable
{
	[SerializeField]
	GameObject nextLevelArrow;

	[SerializeField]
	GameObject playerPrefab;

	[SerializeField]
	InGameCamera gameCamera;

	[SerializeField]
	EffectManager effectManager;

	[SerializeField]
	NPCGenerator npcGenerator;

	[SerializeField]
	InGameUiRoot ui;

	List<Character> characters;
	List<IResetable> resetables;

	Character player;
	CharacterContext characterContext;
	AiStateMachineContext aiContext;
	Limits limits;
	LevelFrame levelFrame;

	int level;

	void Awake()
	{
		nextLevelArrow.SetActive(false);
		level = 1;
		characterContext = CreateCharacterContext();
		StartCoroutine(MainInit());
	}

	IEnumerator MainInit()
	{
		yield return StartCoroutine(ui.Dialoger.ShowDialog("intro"));

		Initialize();
		GatherCharacters();
		InitializeCharacters();
	}

	public List<Character> Characters()
	{
		return characters;
	}

	#region IResetable implementation
	public void Reset()
	{
		//characters.For(x=>Destroy(x.gameObject));
		//characters.Clear();
	}
	#endregion

	public void Restart()
	{
		resetables.ForEach(x=>x.Reset());	
		StartCoroutine(MainInit());
	}

	public IEnumerator SetNextLevel()
	{
		yield return StartCoroutine(CheckForPlayerOnRightSide());
		yield return StartCoroutine(MoveCameraToTheNextLevel());
		level++;

		yield return 0;
		Restart();
	}


	IEnumerator CheckForPlayerOnRightSide()
	{
		var camPos = gameCamera.GetPosition();
		var dstMinPosX = camPos.x + (limits.XMax - limits.XMin) / 5;

		nextLevelArrow.SetActive(true);
		nextLevelArrow.transform.SetPositionX(dstMinPosX);
	
		while (player.Position.x < dstMinPosX)
			yield return 0;
	
		nextLevelArrow.SetActive(false);

	}

	IEnumerator MoveCameraToTheNextLevel()
	{
		var startCamPosX = gameCamera.GetPosition().x;
		var dstCamPosX = gameCamera.GetPosition().x + 8.0f;
		yield return StartCoroutine(Utils.LerpWithEase (startCamPosX, 
			dstCamPosX, 
			2.0f, 
			gameCamera.SetPositionX, 
			Utils.Ease));
	}

	void Initialize()
	{
		levelFrame = GameObject.Find (string.Format ("LevelFrame{0}", level)).GetComponent<LevelFrame>();

		limits = new Limits() { 
			XMin = gameCamera.GetPosition().x - gameCamera.GetWidth() * 0.5f,
			XMax = gameCamera.GetPosition().x + gameCamera.GetWidth() * 0.5f,
			YMin = levelFrame.GetMinY(),
			YMax = levelFrame.GetMaxY()
		};

		npcGenerator.CharacterGenerated += OnCharacterGenerate;
		npcGenerator.NextWaveAction += ui.OnWave;
		npcGenerator.NPCLeftChagedAction += ui.OnLeft;
		npcGenerator.AllWavesFinishedAction += OnAllWavesFinished;
		npcGenerator.Init(new NPCGenerator.Context {
			LevelFrame = levelFrame,
			GameCamera = gameCamera
		});
	
		aiContext = new AiStateMachineContext { 
			Characters = Characters
		};

		resetables = new List<IResetable>() {
			npcGenerator,
			this
		};
	}

	void GatherCharacters()
	{
		characters = new List<Character>();

		if (player == null) {
			player = PlacePlayer();
		}
		characters.Add(player);
	}

	void InitializeCharacter(Character character)
	{
		if (character.Type == Defs.CharacterType.Player) {
			player = character;
			player.HealthChangedAction += ui.OnPlayerHealthChanged;
			player.SpecialAttackCooldown.OnProgress += ui.OnPlayerSpecialAttackProgress;
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
				new List<Character>() { 
					BattleUtils.SortCharactersByDistanceTo(
					charactersInRange,
					attackingCharacter.Position)[0]
			};
		}
	}

	bool IsCharacterInFronOfCharacter(Character who, Character from)
	{
		if (from.GetFlip() == -1) {
			return who.transform.position.x < from.transform.position.x;
		} else {
			return who.transform.position.x > from.transform.position.x;
		}
	}

	CharacterContext CreateCharacterContext()
	{
		return new CharacterContext(effectManager, GetLimits);
	}


	Character PlacePlayer()
	{
		var player = Instantiate(playerPrefab);
		player.transform.position = Vector3.zero;
		return player.GetComponent<Character>();
	}

	Limits GetLimits()
	{
		return limits;
	}

}
