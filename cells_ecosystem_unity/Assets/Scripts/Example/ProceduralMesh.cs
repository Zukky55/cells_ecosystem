using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellsEcosystem
{
    [RequireComponent(typeof(MeshFilter))]
    //[AddComponentMenu("Custom/ProceduralMesh")]
    public class ProceduralMesh : MonoBehaviour
    {
        Mesh mesh;

        Vector3[] vertices;
        int[] triangles;

        private void Awake()
        {
            mesh = GetComponent<MeshFilter>().mesh;
        }

        void Start()
        {
            MakeMeshData();
            CreateMesh();
        }

        private void MakeMeshData()
        {
            // create an array of vertices
            vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(1, 0, 1) };
            // create an array of integers
            triangles = new int[] { 0, 1, 2, 2, 1, 3 };
        }
        private void CreateMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }
    }
}
