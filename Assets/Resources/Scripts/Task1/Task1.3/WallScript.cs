using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class WallScript : MonoBehaviour
{
    [SerializeField]
    public Vector3 size = Vector3.one;

    [SerializeField]
    private int meshSize = 6;

    private List<Material> materialsList;

    private Color[] colors = { Color.green, Color.red, Color.white, Color.blue, Color.cyan, Color.black, Color.grey };


    // Update is called once per frame
    public void GenerateWall()
    {
        //1. initialise MeshFilter
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        //2. initialise MeshBuilder
        MeshGenerator meshGenerator = new MeshGenerator(meshSize);


        //4. build our cube

        //top vertices
        Vector3 t0 = new Vector3(size.x, size.y, -size.z); // top left point
        Vector3 t1 = new Vector3(-size.x, size.y, -size.z); // top right point
        Vector3 t2 = new Vector3(-size.x, size.y, size.z); //bottom right point of top square
        Vector3 t3 = new Vector3(size.x, size.y, size.z); // bottom left point of top square

        //bottom vertices
        Vector3 b0 = new Vector3(size.x, -size.y, -size.z); // bottom left point
        Vector3 b1 = new Vector3(-size.x, -size.y, -size.z); // bottom right point
        Vector3 b2 = new Vector3(-size.x, -size.y, size.z); //bottom right point of bottom square
        Vector3 b3 = new Vector3(size.x, -size.y, size.z); // bottom left point of bottom square


        //top square
        meshGenerator.BuildTriangle(t0, t1, t2, 0);
        meshGenerator.BuildTriangle(t0, t2, t3, 0);

        //bottom square
        meshGenerator.BuildTriangle(b2, b1, b0, 1);
        meshGenerator.BuildTriangle(b3, b2, b0, 1);


        //back square
        meshGenerator.BuildTriangle(b0, t1, t0, 2);
        meshGenerator.BuildTriangle(b0, b1, t1, 2);


        //left-side square
        meshGenerator.BuildTriangle(b1, t2, t1, 3);
        meshGenerator.BuildTriangle(b1, b2, t2, 3);


        //right-side square
        meshGenerator.BuildTriangle(b2, t3, t2, 4);
        meshGenerator.BuildTriangle(b2, b3, t3, 4);

        //front square
        meshGenerator.BuildTriangle(b3, t0, t3, 5);
        meshGenerator.BuildTriangle(b3, b0, t0, 5);


        //3. set mesh filter's mesh to the mesh generated from our mesh builder
        meshFilter.mesh = meshGenerator.CreateMesh();

    }

    public void AddMaterials(int colourPicked)
    {

        //5. Assign materials to the mesh renderer's materials list

        materialsList = new List<Material>();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();

        for (int faceCounter = 0; faceCounter < meshSize; faceCounter++)
        {
            Material matColor = new Material(Shader.Find("Specular"));
            matColor.color = colors[colourPicked];
            materialsList.Add(matColor);
            meshRenderer.materials = materialsList.ToArray();

        }
        
    }


}
