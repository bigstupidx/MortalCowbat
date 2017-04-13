using UnityEngine;
using System.Collections.Generic;
using Ui;
using Ai;
using Vis;
using System.Collections;
using System;
using Battle.Comp;
using UnityEngine.SceneManagement;

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

	List<Character> characters = new List<Character>();
	List<IResetable> resetables;

	Character player { get { return characters.Find(x=>x.Type == Defs.CharacterType.Player); }}
	CharacterContext characterContext;
	AiStateMachineContext aiContext;
	Limits limits;
	LevelFrame levelFrame;

	GameEvents events;
	const int LevelCount = 4;

	int level {
		get;
		set;
	}

	void Awake()
	{
		events = new GameEvents();

		aiContext = new AiStateMachineContext { 
			CharacterList = characters
		};

		resetables = new List<IResetable>() {
			npcGenerator,
			events,
			this
		};
	}


	void Start()
	{
		ui.DialogController.startAction = Pause;
		ui.DialogController.finishAction = Resume;

		nextLevelArrow.SetActive(false);
		level = 1;
		characterContext = CreateCharacterContext();
		InitCallbacks();
		InitLevel();
		CallLevelStarted(level);
	}

	void InitLevel()
	{
		InitLevelFrame ();
		InitNPCGenerator ();
		InitPlayer();
		InitCharacters();
	}

	public List<Character> Characters()
	{
		return characters;
	}

	#region IResetable implementation
	public void Reset()
	{
		//characters.ForEach(x=>Destroy(x.gameObject));
		//characters.Clear();
	}
	#endregion

	public void Restart(bool killNPC)
	{
		resetables.ForEach(x=>x.Reset());	
		if (killNPC) {
			KillNPCs();
		}
		InitLevel();
	}

	public IEnumerator SetNextLevel()
	{
		
		yield return StartCoroutine(CheckForPlayerOnRightSide());
		player.CheckLimits = false;
		gameCamera.Follower.CheckLimits = false;
		yield return StartCoroutine(MovePlayerToTheNextLevel());
		gameCamera.Follower.Follow = false;
		level++;

		if (level > LevelCount) {
			SceneManager.LoadScene(0);
		}

		CallLevelStarted(level);

		yield return 0;

		Restart(false);

		yield return StartCoroutine(gameCamera.Follower.AllignWithLimit());

		player.CheckLimits = true;
		gameCamera.Follower.CheckLimits = true;
		gameCamera.Follower.Follow = true;
	}

	public void Pause()
	{
		characters.ForEach(x=>x.GetComp<Pause>().Perform());	
		npcGenerator.Pause();
	}

	public void Resume()
	{
		characters.ForEach(x=>x.GetComp<Pause>().Cancel());	
		npcGenerator.Resume();
	}

	void KillNPCs ()
	{
		for (int i = 0; i < characters.Count; ++i) {
			if (characters[i].Type == Defs.CharacterType.NPC) {
				Destroy(characters[i].gameObject);
			}
		}		
		characters.RemoveAll(x=>x.Type == Defs.CharacterType.NPC);
	}

	IEnumerator CheckForPlayerOnRightSide()
	{
		var dstMinPosX = limits.XMax - 2; // TODO

		nextLevelArrow.SetActive(true);
		nextLevelArrow.transform.SetPositionX(dstMinPosX);
	
		while (player.GetPosition().x < dstMinPosX)
			yield return 0;
	
		nextLevelArrow.SetActive(false);
	}

	IEnumerator MovePlayerToTheNextLevel()
	{
		var dstPosX = limits.XMax + 4.0f; // TODO

		while (player.GetPosition().x < dstPosX) {
			player.MoveH(1);
			yield return 0;
		}

	}

	IEnumerator MoveCameraToTheNextLevel()
	{
		var startCamPosX = gameCamera.GetPosition().x;
		var dstCamPosX = player.GetPosition().x;
		yield return StartCoroutine(Utils.LerpWithEase (startCamPosX, 
			dstCamPosX, 
			2.0f, 
			gameCamera.SetPositionX, 
			Utils.Ease));

	}

	void InitLevelFrame ()
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

	void InitNPCGenerator ()
	{
		npcGenerator.Init (new NPCGenerator.Context {
			LevelFrame = levelFrame,
			GameCamera = gameCamera,
		}, level);
	}

	void InitCallbacks()
	{
		npcGenerator.CharacterGenerated += OnCharacterGenerate;
		npcGenerator.AllWavesFinishedAction += OnAllWavesFinished;
		npcGenerator.NextWaveAction += OnWaveStarted;
		npcGenerator.WaveFinishedAction += OnWaveFinished;
		npcGenerator.NPCLeftChagedAction += OnNPCLeftChanged;

		events.WaveStarted += ui.OnWave;
		events.WaveStarted += ui.DialogController.OnWaveStarted;
		events.WaveFinished += ui.DialogController.OnWaveFinished;
		events.AllWavesFinished += ui.DialogController.OnAllWavesFinished;
		events.LevelStarted += ui.DialogController.OnLevelStarted;

		events.NPCLeftChanged += ui.OnLeft;
	}

	void InitPlayer()
	{
		if (player == null) {
			PlacePlayer();
			gameCamera.Follower.Target = player.transform;
		}
	}

	void InitializeCharacter(Character character)
	{
		if (character.Type == Defs.CharacterType.Player) {
			player.GetComp<Health>().HealthChangedAction += ui.OnPlayerHealthChanged;
			player.GetComp<Attacking>().SpecialAttackCooldown.OnProgress += ui.OnPlayerSpecialAttackProgress;
		} else {
			SetNpcStateMachine(character, aiContext);
		}
	
		character.Init(characterContext);
		character.GetComp<Attacking>().AttackAction = OnCharacterAttack;
		character.GetComp<Attacking>().SpecialAttackAction = OnCharacterSpecialAttack;
		character.GetComp<Attacking>().HeavyAttackAction = OnCharacterHeavyAttack;
		character.GetComp<Attacking>().JumpAttackAction = OnCharacterJumpAttack;
		character.GetComp<Death>().DeathAction = OnCharacterDeath;
	}


	void InitCharacters()
	{
		characters.ForEach(InitializeCharacter);
	}

	void SetNpcStateMachine(Character character, AiStateMachineContext aiContext)
	{
		var aiStateMachine = character.GetComponent<AiStateMachine>();
		if (aiStateMachine != null) {
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
					attackingCharacter.GetPosition())[0]
			};
		}
	}

	bool IsCharacterInFronOfCharacter(Character who, Character from)
	{
		if (from.GetComp<Moving>().GetFlip() == -1) {
			return who.transform.position.x < from.transform.position.x;
		} else {
			return who.transform.position.x > from.transform.position.x;
		}
	}

	CharacterContext CreateCharacterContext()
	{
		return new CharacterContext(effectManager, GetLimits);
	}


	void PlacePlayer()
	{
		var player = Instantiate(playerPrefab);
		player.transform.position = Vector3.zero;
		var playerScript = player.GetComponent<Character>();
		characters.Add(playerScript);
		ui.VirtualKeyboardController.AttachCharacter(playerScript);
	}

	Limits GetLimits()
	{
		return limits;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P)) {
			Pause();
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			Resume();
		}
	}

}
