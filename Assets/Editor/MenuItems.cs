using UnityEditor;
using UnityEditor.SceneManagement;

public class MenuItems
{
	[MenuItem("Tools/Character Preview")]
	static void NewMenuOption()
	{
		EditorSceneManager.OpenScene("Assets/Scenes/CharacterPreview.unity");
		CharacterPreview.PreviewCharactersInCurrentScene();
	}
}