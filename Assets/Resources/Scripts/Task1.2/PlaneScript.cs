using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlaneScript : MonoBehaviour
{
    
    private float cellSize;


    private int subMeshSize;


    private int width;
    private int height;
    public void GeneratePlane() 
    {
        subMeshSize = this.gameObject.GetComponent<MeshController>().subMeshsize;

        cellSize = this.gameObject.GetComponent<MeshController>().size;

        if (this.gameObject.GetComponent<MeshController>().dimensions.x <= 1) 
        {
            this.gameObject.GetComponent<MeshController>().dimensions.x = 2;
        }

        if (this.gameObject.GetComponent<MeshController>().dimensions.y <= 1)
        {
            this.gameObject.GetComponent<MeshController>().dimensions.y = 2;
        }
            
        width = ((int)this.gameObject.GetComponent<MeshController>().dimensions.x);
        height = ((int)this.gameObject.GetComponent<MeshController>().dimensions.y);

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshGenerator meshGenerator = new MeshGenerator(subMeshSize);

        //create points of our plane
        Vector3[,] points = new Vector3[width, height];


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                points[x, y] = new Vector3(
                        cellSize * x,
                        0,
                        cellSize * y);
            }
        }




        //create the quads if genPlane is true else remove if false
        if (this.gameObject.GetComponent<MeshController>().genPlane) 
        {
            int submesh = 0;

            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    Vector3 br = points[x, y];
                    Vector3 bl = points[x + 1, y];
                    Vector3 tr = points[x, y + 1];
                    Vector3 tl = points[x + 1, y + 1];

                    //create 2 triangles that make up a quad
                    meshGenerator.BuildTriangle(bl, tr, tl, submesh % subMeshSize);
                    meshGenerator.BuildTriangle(bl, br, tr, submesh % subMeshSize);

                    this.gameObject.GetComponent<MeshController>().AddMaterials();
                }

                submesh++;
            }
        }


        meshFilter.mesh = meshGenerator.CreateMesh();
       
    }
}
