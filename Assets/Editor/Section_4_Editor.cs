using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Section_4))]
public class Section_4_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Section_4 section = target as Section_4;

        if (GUILayout.Button("Get Correct Positions"))
        {
            section.correctPositions.Clear();
            for (int i = 0; i < section.wordAndWordMap.Count; i++)
            {
                section.correctPositions.Add(section.wordAndWordMap[i].word.transform.position);
            }
        }

        if (GUILayout.Button("Get Incorrect Positions"))
        {
            section.incorrectPositions.Clear();
            for (int k = 0; k < section.wordAndWordMap.Count; k++)
            {
                section.incorrectPositions.Add(section.wordAndWordMap[k].word.transform.position);
            }
        }
    }
}
