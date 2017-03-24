using UnityEngine;
using System.Collections.Generic;
using Ui;
using Ai;
using Vis;
using UnityEngine.SceneManagement;
using System.Collections;

public partial class GameManager : MonoBehaviour, IResetable
{
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

	void Awake()
	{
		MainInit();

		ui.Dialoger.ShowDialog("intro");;

	}


	void MainInit()
	{
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
		characters.ForEach(x=>Destroy(x.gameObject));
		characters.Clear();
	}
	#endregion

	public void Restart()
	{
		resetables.ForEach(x=>x.Reset());	
		MainInit();
	}

	public IEnumerator SetNextLevel()
	{
		Destroy(GameObject.Find("Level").gameObject);
		SceneManager.LoadScene("Level2", LoadSceneMode.Additive);
		yield return 0;
		Restart();
	}

	void Initialize()
	{
		levelFrame = GameObject.FindObjectOfType<LevelFrame>();

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
		characterContext = CreateCharacterContext();
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
		characters.Add(PlacePlayer());
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
		return new CharacterContext(effectManager, limits);
	}


	Character PlacePlayer()
	{
		var player = Instantiate(playerPrefab);
		player.transform.position = Vector3.zero;
		return player.GetComponent<Character>();
	}
}
