using UnityEngine;
using System.Collections.Generic;
using Ui;
using Ai;
using Vis;
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
		InitializeLevelFrame (level);

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
		player.CheckLimits = false;
		gameCamera.Follower.CheckLimits = false;
		yield return StartCoroutine(MovePlayerToTheNextLevel());
		gameCamera.Follower.Follow = false;
		level++;

		yield return 0;

		Restart();

		yield return StartCoroutine(gameCamera.Follower.AllignWithLimit());

		player.CheckLimits = true;
		gameCamera.Follower.CheckLimits = true;
		gameCamera.Follower.Follow = true;
	}


	IEnumerator CheckForPlayerOnRightSide()
	{
		var dstMinPosX = limits.XMax - 2; // TODO

		nextLevelArrow.SetActive(true);
		nextLevelArrow.transform.SetPositionX(dstMinPosX);
	
		while (player.Position.x < dstMinPosX)
			yield return 0;
	
		nextLevelArrow.SetActive(false);
	}

	IEnumerator MovePlayerToTheNextLevel()
	{
		var dstPosX = limits.XMax + 4.0f; // TODO

		while (player.Position.x < dstPosX) {
			player.MoveH(1);
			yield return 0;
		}

	}

	IEnumerator MoveCameraToTheNextLevel()
	{
		var startCamPosX = gameCamera.GetPosition().x;
		var dstCamPosX = player.Position.x;
		yield return StartCoroutine(Utils.LerpWithEase (startCamPosX, 
			dstCamPosX, 
			2.0f, 
			gameCamera.SetPositionX, 
			Utils.Ease));

	}

	void InitializeLevelFrame (int level)
	{
		levelFrame = GameObject.Find (string.Format ("LevelFrame{0}", level)).GetComponent<LevelFrame> ();
		gameCamera.Follower.LevelFrame = levelFrame;

		limits = new Limits() { 
			XMin = levelFrame.GetXMin(),
			XMax = levelFrame.GetXMax(),
			YMin = levelFrame.GetMinY(),
			YMax = levelFrame.GetMaxY()
		};
	}

	void Initialize()
	{
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
			gameCamera.Follower.Target = player.transform;
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
		character.HeavyAttackAction = OnCharacterHeavyAttack;
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
