using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    private List<Vector3> vertices = new List<Vector3>(); //list of vertices - store our points in our mesh

    private List<int> indices = new List<int>(); //list of indices that point to the index location in our vertices list

    private List<int>[] submeshIndices = new List<int>[] { }; // an array of submesh indices

    public MeshGenerator(int submeshCount)
    {
        submeshIndices = new List<int>[submeshCount];

        for (int i = 0; i < submeshCount; i++)
        {
            submeshIndices[i] = new List<int>();
        }
    }

    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, int subMesh)
    {
        // 2. index of each vertex within the list of vertices
        int p0Index = vertices.Count;
        int p1Index = vertices.Count + 1;
        int p2Index = vertices.Count + 2;

        //3. add the index of each vertex to the indices
        indices.Add(p0Index);
        indices.Add(p1Index);
        indices.Add(p2Index);

        submeshIndices[subMesh].Add(p0Index);
        submeshIndices[subMesh].Add(p1Index);
        submeshIndices[subMesh].Add(p2Index);


        //1. add each point to our vertices list
        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);

    }

    public Mesh CreateMesh() // build our mesh
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();

        mesh.triangles = indices.ToArray();

        mesh.subMeshCount = submeshIndices.Length;

        for (int i = 0; i < submeshIndices.Length; i++)
        {
            if (submeshIndices[i].Count < 3)
            {
                mesh.SetTriangles(new int[3] { 0, 0, 0 }, i);
            }
            else
            {
                mesh.SetTriangles(submeshIndices[i].ToArray(), i);
            }
        }

        mesh.RecalculateNormals();

        return mesh;
    }

}
