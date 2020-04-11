using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace CellsEcosystem
{
    public static class RangeObjectGizmosEditor
    {
        static readonly int TRIANGLE_COUNT = 12;
        static readonly Color MESH_COLOR = new Color(1f, 1f, 0f, .7f);

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        static void DrawPointGizmos(GizmoObject rangeObj, GizmoType gizmoType)
        {
            if (rangeObj.Length <= 0.0f)
            {
                return;
            }

            Gizmos.color = rangeObj.Color;

            Transform transform = rangeObj.transform;
            Vector3 pos = transform.position + Vector3.up * 0.01f; // 0.01fは地面と高さだと見づらいので調整用。
            Quaternion rot = transform.rotation;
            Vector3 scale = Vector3.one * rangeObj.Length;


            if (rangeObj.HeightAngle > 0.0f)
            {
                Mesh fanMesh = CreateFanMesh(rangeObj.HeightAngle, TRIANGLE_COUNT);

                Gizmos.DrawMesh(fanMesh, pos, rot * Quaternion.AngleAxis(90.0f, Vector3.forward), scale);
                Gizmos.DrawMesh(fanMesh, pos, rot * Quaternion.AngleAxis(270.0f, Vector3.forward), scale);
            }

            if (rangeObj.WidthAngle > 0.0f)
            {
                Mesh fanMesh = CreateFanMesh(rangeObj.WidthAngle, TRIANGLE_COUNT);

                Gizmos.DrawMesh(fanMesh, pos, rot, scale);
                Gizmos.DrawMesh(fanMesh, pos, rot * Quaternion.AngleAxis(180.0f, Vector3.forward), scale);
            }
        }
        static Mesh CreateFanMesh(float angle, int triangleCount)
        {
            var mesh = new Mesh();
            var vertices = CreateFanVerticles(angle, triangleCount);
            var triangleIndexes = new List<int>(triangleCount * 3);
            for (int i = 0; i < triangleCount; ++i)
            {
                triangleIndexes.Add(0);
                triangleIndexes.Add(i + 1);
                triangleIndexes.Add(i + 2);
            }
            mesh.vertices = vertices;
            var triangles = triangleIndexes.ToArray();
            mesh.triangles = triangles;
            //Debug.Log($"triangles is {triangles.Length }");
            //Debug.Log($"mesh.triangles is {mesh.triangles.Length }");

            mesh.RecalculateNormals();
            return mesh;
        }
        static Vector3[] CreateFanVerticles(float angle, int triangleCount)
        {
            if (angle <= 0f)
            {
                throw new System.ArgumentOutOfRangeException($"angle = {angle}");
            }
            if (triangleCount <= 0)
            {
                throw new System.ArgumentOutOfRangeException($"trigleCount = {triangleCount}");
            }
            angle = Mathf.Min(angle, 360f);
            var vertices = new List<Vector3>(triangleCount + 2);
            vertices.Add(Vector3.zero);

            var radian = angle * Mathf.Deg2Rad;
            var startRad = -radian / 2;
            var incRad = radian / triangleCount;

            for (int i = 0; i < triangleCount + 1; ++i)
            {
                var currentRad = startRad + (incRad * i);

                var vertex = new Vector3(Mathf.Sin(currentRad), 0f, Mathf.Cos(currentRad));
                vertices.Add(vertex);
            }
            return vertices.ToArray();
        }
    }
}