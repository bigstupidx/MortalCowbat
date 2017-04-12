using UnityEngine;
using System.Collections.Generic;

namespace Ai
{
	public class AiStateMachine : MonoBehaviour
	{
		public AiPreset Preset { get { return aiPreset; } set { aiPreset = value; }}

		[SerializeField]
		AiPreset aiPreset;

		[SerializeField]
		Character character;

		[SerializeField]
		float DetectionRange;

		AiStateMachineContext context;
		AiStateContext stateContext;
		IAiState currentState;

		bool init;

		public void Init(AiStateMachineContext context)
		{
			this.context = context;
			this.stateContext = new AiStateContext() { 
				Character = character,
				Sm = this
			};

			init = true; 
			currentState = new ChasingState(stateContext);
		}

		void Update()
		{
			if (init) {
				currentState.Update();
			}
		}

		public void SetState(IAiState state)
		{
			currentState = state;
		}


		public List<Character> FindTargets(float range)
		{
			var players = context.CharacterList.FindAll(
				x=>x != character && 
				x.Type == Defs.CharacterType.Player &&
				BattleUtils.IsPointInRange(x.GetPosition(), character.GetPosition(), range)
			);

			var sortTargets = BattleUtils.SortCharactersByDistanceTo(players, character.GetPosition());
			return sortTargets;
		}
	}
}

