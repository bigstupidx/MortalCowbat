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
	SerializedProperty kickAttackProbability;

	SerializedProperty heavyAttackMinCharge;
	SerializedProperty heavyAttackMaxCharge;

	void OnEnable()
	{
		attackRangeCoeficient = serializedObject.FindProperty("AttackRangeCoeficient");
		heavyAttackProbability = serializedObject.FindProperty("HeavyAttackProbability");
		fastAttackProbability = serializedObject.FindProperty("FastAttackProbability");
		kickAttackProbability = serializedObject.FindProperty("KickAttackProbability");
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
		EditorGUILayout.PropertyField(attackRangeCoeficient);

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Fast Attack:", fastAttackProbability.floatValue.ToString("F2"));
		EditorGUILayout.LabelField("Heavy Attack:", heavyAttackProbability.floatValue.ToString("F2"));
		EditorGUILayout.LabelField("Kick Attack:", kickAttackProbability.floatValue.ToString("F2"));


		if ( (fastAttackProbability.floatValue + heavyAttackProbability.floatValue + kickAttackProbability.floatValue) > 1.01f) {
			fastAttackProbability.floatValue = 0.33f;
			heavyAttackProbability.floatValue = 0.33f;
			kickAttackProbability.floatValue = 0.33f;
		}

		float fastAttack = fastAttackProbability.floatValue;
		float heavyAttack = heavyAttackProbability.floatValue + fastAttack;

		EditorGUILayout.MinMaxSlider("Attacks", ref fastAttack, ref heavyAttack, 0.0f, 1.0f);

		if(GUI.changed) {
			fastAttackProbability.floatValue = fastAttack;
			heavyAttackProbability.floatValue = heavyAttack - fastAttack;
			kickAttackProbability.floatValue = 1 - (heavyAttackProbability.floatValue + fastAttackProbability.floatValue);
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