#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryItemData))]
public class InventoryItemDataEditor : Editor
{
    private bool showShapePreview = true;

    public override void OnInspectorGUI()
    {
        InventoryItemData itemData = (InventoryItemData)target;

        // Draw default inspector
        DrawDefaultInspector();

        // Preview toggle
        showShapePreview = EditorGUILayout.Foldout(showShapePreview, "Shape Preview");

        if (showShapePreview)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Item Shape Preview", EditorStyles.boldLabel);

            // Get the shape pattern
            bool[,] shapePattern = itemData.GetShapePattern();

            // Draw the shape pattern
            GUILayout.BeginVertical(EditorStyles.helpBox);
            for (int y = 0; y < itemData.Height; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < itemData.Width; x++)
                {
                    bool isFilled = shapePattern[y, x];
                    GUI.backgroundColor = isFilled ? Color.green : Color.red;

                    GUILayout.Box("", GUILayout.Width(20), GUILayout.Height(20));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            // Reset background color
            GUI.backgroundColor = Color.white;

            EditorGUILayout.HelpBox("Green = Occupied, Red = Empty", MessageType.Info);

            // Add example button
            if (GUILayout.Button("Generate Default L Shape"))
            {
                // L-shaped example
                string lShape = "11\n10\n10";
                SerializedProperty shapeProp = serializedObject.FindProperty("shape");
                shapeProp.stringValue = lShape;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif