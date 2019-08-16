using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class MeshGenerator : MonoBehaviour
{
    public enum MeshType { Triangle, Square, Pentagon, Cube}
    public MeshType Shape;
    private Mesh customMesh;
    private float TimeDrawn;
    private Color CurrentGizmoColor;

    public float Width = 1.0f;
    public float Height = 1.0f;
    public float Depth = 1.0f;
    public Vector3 Offset = Vector3.zero;

    void Update()
    {
        if (Shape == MeshType.Triangle)
        {
            var mesh = new Mesh();
            var verts = new Vector3[3];

            verts[0] = new Vector3(Offset.x, Offset.y, Offset.z);
            verts[1] = new Vector3(Offset.x, 1 * Height + Offset.y, Offset.z);
            verts[2] = new Vector3(1 * Width + Offset.x, Offset.y, Offset.z);
            mesh.vertices = verts;

            var indices = new int[3];

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            mesh.triangles = indices;

            var norms = new Vector3[3];

            norms[0] = -Vector3.forward;
            norms[1] = -Vector3.forward;
            norms[2] = -Vector3.forward;

            mesh.normals = norms;

            var UVs = new Vector2[3];

            UVs[0] = new Vector2(0, 0);
            UVs[1] = new Vector2(0, 1);
            UVs[2] = new Vector2(1, 0);

            mesh.uv = UVs;

            var filter = GetComponent<MeshFilter>();
            filter.mesh = mesh;
            customMesh = mesh;
        }
        else if (Shape == MeshType.Square)
        {
            var mesh = new Mesh();
            var verts = new Vector3[6];

            verts[0] = new Vector3(Offset.x, Offset.y, Offset.z);
            verts[1] = new Vector3(Offset.x, 1 * Height + Offset.y, Offset.z);
            verts[2] = new Vector3(1 * Width + Offset.x, 1 * Height + Offset.y, Offset.z);
            verts[3] = new Vector3(Offset.x, Offset.y, Offset.z);
            verts[4] = new Vector3(1 * Width + Offset.x, 1 * Height + Offset.y, Offset.z);
            verts[5] = new Vector3(1 * Width + Offset.x, Offset.y, Offset.z);
            mesh.vertices = verts;

            var indices = new int[6];

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 3;
            indices[4] = 4;
            indices[5] = 5;

            mesh.triangles = indices;

            var norms = new Vector3[6];

            norms[0] = -Vector3.forward;
            norms[1] = -Vector3.forward;
            norms[2] = -Vector3.forward;
            norms[3] = -Vector3.forward;
            norms[4] = -Vector3.forward;
            norms[5] = -Vector3.forward;

            mesh.normals = norms;

            var UVs = new Vector2[6];

            UVs[0] = new Vector2(0, -1);
            UVs[1] = new Vector2(0, 0);
            UVs[2] = new Vector2(1, 0);
            UVs[3] = new Vector2(-1, 0);
            UVs[4] = new Vector2(0, 1);
            UVs[5] = new Vector2(0, 0);

            mesh.uv = UVs;

            var filter = GetComponent<MeshFilter>();
            filter.mesh = mesh;
            customMesh = mesh;
        }
        else if (Shape == MeshType.Pentagon)
        {
            var mesh = new Mesh();
            var verts = new Vector3[7];

            verts[0] = new Vector3(Offset.x, Offset.y, Offset.z);
            verts[1] = new Vector3(Offset.x, 2.5f * Height + Offset.y, Offset.z);
            verts[2] = new Vector3(2 * Width + Offset.x, 1 * Height + Offset.y, Offset.z);
            verts[4] = new Vector3(1 * Width + Offset.x, -1 * Height + Offset.y, Offset.z);
            verts[5] = new Vector3(-1 * Width + Offset.x, -1 * Height + Offset.y, Offset.z);
            verts[6] = new Vector3(-2 * Width + Offset.x, 1 * Height + Offset.y, Offset.z);

            mesh.vertices = verts;

            var indices = new int[15];

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 0;
            indices[4] = 4;
            indices[5] = 5;
            indices[6] = 0;
            indices[7] = 6;
            indices[8] = 1;
            indices[9] = 2;
            indices[10] = 4;
            indices[11] = 0;
            indices[12] = 5;
            indices[13] = 6;
            indices[14] = 0;

            mesh.triangles = indices;

            var norms = new Vector3[7];

            norms[0] = -Vector3.forward;
            norms[1] = -Vector3.forward;
            norms[2] = -Vector3.forward;
            norms[3] = -Vector3.forward;
            norms[4] = -Vector3.forward;
            norms[5] = -Vector3.forward;
            norms[6] = -Vector3.forward;

            mesh.normals = norms;

            var UVs = new Vector2[7];

            UVs[0] = new Vector2(0, 0);
            UVs[1] = new Vector2(0, 1);
            UVs[2] = new Vector2(1, 0);
            UVs[3] = new Vector2(0, 0);
            UVs[4] = new Vector2(0, 1);
            UVs[5] = new Vector2(1, 0);
            UVs[6] = new Vector2(1, 0);

            mesh.uv = UVs;

            var filter = GetComponent<MeshFilter>();
            filter.mesh = mesh;
            customMesh = mesh;
        }
        else if (Shape == MeshType.Cube)
        {
            var mesh = new Mesh();
            var verts = new Vector3[12];

            verts[0] = new Vector3(0, 0, 0);
            verts[1] = new Vector3(0, 1 * Height, 0);
            verts[2] = new Vector3(1 * Width, 1 * Height, 0);
            verts[3] = new Vector3(0, 0, 0);
            verts[4] = new Vector3(1 * Width, 1 * Height, 0);
            verts[5] = new Vector3(1 * Width, 0, 0);

            verts[10] = new Vector3(0, 0, -1 * Depth);
            verts[6] = new Vector3(0, 1 * Height, -1 * Depth);
            verts[7] = new Vector3(1 * Width, 1 * Height, -1 * Depth);
            verts[8] = new Vector3(0, 0, -1 * Depth);
            verts[9] = new Vector3(1 * Width, 1 * Height, -1 * Depth);
            verts[10] = new Vector3(1 * Width, 0, -1 * Depth);

            mesh.vertices = verts;

            var indices = new int[42];

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 3;
            indices[4] = 4;
            indices[5] = 1;
            indices[6] = 2;
            indices[7] = 9;
            indices[8] = 6;
            indices[9] = 1;
            indices[10] = 2;
            indices[11] = 11;
            indices[12] = 4;
            indices[13] = 6;
            indices[14] = 1;
            indices[15] = 8;
            indices[16] = 0;
            indices[17] = 1;
            indices[18] = 8;
            indices[19] = 10;
            indices[20] = 0;
            indices[21] = 10;
            indices[22] = 5;
            indices[23] = 0;
            indices[24] = 1;
            indices[25] = 6;
            indices[26] = 8;
            indices[27] = 6;
            indices[28] = 7;
            indices[29] = 8;
            indices[30] = 9;
            indices[31] = 10;
            indices[32] = 8;
            indices[33] = 2;
            indices[34] = 0;
            indices[35] = 5;
            indices[36] = 2;
            indices[37] = 5;
            indices[38] = 10;
            indices[39] = 9;
            indices[40] = 2;
            indices[41] = 10;

            mesh.triangles = indices;

            var norms = new Vector3[12];

            norms[0] = -Vector3.forward;
            norms[1] = -Vector3.forward;
            norms[2] = -Vector3.forward;
            norms[3] = -Vector3.forward;
            norms[4] = -Vector3.forward;
            norms[5] = -Vector3.forward;
            norms[6] = -Vector3.forward;
            norms[7] = -Vector3.forward;
            norms[8] = -Vector3.forward;
            norms[9] = -Vector3.forward;
            norms[10] = -Vector3.forward;
            norms[11] = -Vector3.forward;

            mesh.normals = norms;

            var UVs = new Vector2[12];

            UVs[0] = new Vector2(0, 0);
            UVs[1] = new Vector2(0, 1);
            UVs[2] = new Vector2(1, 0);
            UVs[3] = new Vector2(1, 0);
            UVs[4] = new Vector2(0, 1);
            UVs[5] = new Vector2(0, 0);
            UVs[6] = new Vector2(0, 0);
            UVs[7] = new Vector2(0, 0);
            UVs[8] = new Vector2(0, 0);
            UVs[9] = new Vector2(0, 0);
            UVs[10] = new Vector2(0, 0);
            UVs[11] = new Vector2(0, 0);

            mesh.uv = UVs;

            var filter = GetComponent<MeshFilter>();
            filter.mesh = mesh;
            customMesh = mesh;
        }
    }

    void OnDestroy()
    {
        if (customMesh != null)
        {
            DestroyImmediate(customMesh);
        }
    }

    private void OnDrawGizmos()
    {
        TimeDrawn += Time.deltaTime;

        if (TimeDrawn > 0.25f)
        {
            TimeDrawn = 0;
            CurrentGizmoColor = Random.ColorHSV(0.7f, 1f, 0.7f, 1f, 0.6f, 1f, 1f, 1f);
        }

        Gizmos.color = CurrentGizmoColor;
        Handles.color = CurrentGizmoColor;

        if (customMesh)
        {
            for (int i = 0; i < customMesh.vertices.Length; i++)
            {
                Handles.Label(transform.TransformPoint(customMesh.vertices[i]), "Vert" + i);
                Gizmos.DrawSphere(transform.TransformPoint(customMesh.vertices[i]), 0.025f);
            }
        }
    }
}
