using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace CellsEcosystem
{
    public static class MeshManager
    {
#if UNITY_EDITOR
        #region CreateMesh
        public static Mesh CreateFanMesh(float angle, int triangleCount)
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
            Debug.Log($"triangles is {triangles.Length }");
            Debug.Log($"mesh.triangles is {mesh.triangles.Length }");

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
        public static void SaveMesh(Mesh mesh)
        {
            var fileName = $"Assets/Resources/Mesh{mesh.name}.asset";
            AssetDatabase.CreateAsset(mesh, fileName);
        }
        public static void SaveGameObjectWithMesh(Mesh mesh)
        {
            var obj = new GameObject();
            obj.name = mesh.name;
            obj.AddComponent<MeshFilter>();
            obj.AddComponent<MeshRenderer>();
            obj.AddComponent<MeshCollider>();
            obj.GetComponent<MeshFilter>().sharedMesh = mesh;
            obj.GetComponent<MeshFilter>().sharedMesh.name = mesh.name;
            obj.GetComponent<MeshCollider>().sharedMesh = mesh;

            //PrefabUtility.CreatePrefab($"Assets/Resources/PieceParts/{ mesh.name}.prefab", obj);
            PrefabUtility.SaveAsPrefabAsset(obj, $"Assets/Resources/PieceParts/{ mesh.name}.prefab");
            AssetDatabase.SaveAssets();
        }
        #endregion
#endif
    }
}