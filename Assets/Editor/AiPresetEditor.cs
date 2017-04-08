using UnityEngine;
using UnityEditor;
using Ai;


[CustomEditor(typeof(AiPreset))]
[CanEditMultipleObjects]
public class AiPresetEditor : Editor 
{
	SerializedProperty firstAttackDelay;
	SerializedProperty attackInterval;

	SerializedProperty attackRangeCoeficient;
	SerializedProperty heavyAttackProbability;
	SerializedProperty fastAttackProbability;

	SerializedProperty heavyAttackMinCharge;
	SerializedProperty heavyAttackMaxCharge;

	void OnEnable()
	{
		attackRangeCoeficient = serializedObject.FindProperty("AttackRangeCoeficient");
		heavyAttackProbability = serializedObject.FindProperty("HeavyAttackProbability");
		fastAttackProbability = serializedObject.FindProperty("FastAttackProbability");
		heavyAttackMinCharge = serializedObject.FindProperty("HeavyAttackMinCharge");
		heavyAttackMaxCharge = serializedObject.FindProperty("HeavyAttackMaxCharge");
		firstAttackDelay = serializedObject.FindProperty("FirstAttackDelay");
		attackInterval = serializedObject.FindProperty("AttackInterval");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(firstAttackDelay);
		EditorGUILayout.PropertyField(attackInterval);


		EditorGUILayout.Slider(attackRangeCoeficient, 0.0f, 1.0f);
		EditorGUILayout.Slider(fastAttackProbability, 0.0f, 1.0f);
		if(GUI.changed) {
			heavyAttackProbability.floatValue = 1 - fastAttackProbability.floatValue;
		}

		EditorGUILayout.Slider(heavyAttackProbability, 0.0f, 1.0f);
		if(GUI.changed) {
			fastAttackProbability.floatValue = 1 - heavyAttackProbability.floatValue;
		}

		EditorGUILayout.Space();

		float min = heavyAttackMinCharge.floatValue;
		float max = heavyAttackMaxCharge.floatValue;
		EditorGUILayout.LabelField("Min Charge:", min.ToString("F1"));
		EditorGUILayout.LabelField("Max Charge:", max.ToString("F1"));

		EditorGUILayout.MinMaxSlider("Min Max charge", ref min, ref max, 0.0f, 1.0f);

		heavyAttackMinCharge.floatValue = min;
		heavyAttackMaxCharge.floatValue = max;

		serializedObject.ApplyModifiedProperties();
	}
}