using UnityEngine;
using UnityEditor;

public class TestWindow : SplitViewWindow
{
    [MenuItem("Window/Test Window")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<TestWindow>();
    }

    private void OnGUI()
    {
        SetSplitDirection(SplitDirection.Vertical);

        BeginSplit();

        GUILayout.Button("Press me");
        EditorGUILayout.LabelField("This is a label field");
        EditorGUILayout.Toggle("Follow The Author", true);
        EditorGUILayout.Toggle("Unimportant Toggle", false);
        EditorGUILayout.IntField("Int Field", 0);
        EditorGUILayout.Space();
        EditorGUILayout.Vector3Field("Vector", Vector3.one);

        Split();

        EditorGUILayout.ColorField("Pick Your Color", Color.red);
        EditorGUILayout.Slider("Cool Slider", 0, 0, 10);
        EditorGUILayout.TagField("Tag", "Untagged");
        EditorGUILayout.Space(65);
        GUILayout.Button("Small Button", GUILayout.Width(90));

        EndSplit();
    }
}
