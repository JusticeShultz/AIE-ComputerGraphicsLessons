#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGenerator))]
[CanEditMultipleObjects]
public class MeshGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshGenerator meshGenerator = (MeshGenerator)target as MeshGenerator;

        meshGenerator.Shape = (MeshGenerator.MeshType)EditorGUILayout.EnumPopup("Shape", meshGenerator.Shape);

        meshGenerator.Width = EditorGUILayout.Slider("Width", meshGenerator.Width, -10, 10);
        meshGenerator.Height = EditorGUILayout.Slider("Height", meshGenerator.Height, -10, 10);

        if (meshGenerator.Shape == MeshGenerator.MeshType.Cube)
            meshGenerator.Depth = EditorGUILayout.Slider("Depth", meshGenerator.Depth, -10, 10);

        meshGenerator.Offset = EditorGUILayout.Vector3Field("Offset", meshGenerator.Offset);
    }
}
#endif