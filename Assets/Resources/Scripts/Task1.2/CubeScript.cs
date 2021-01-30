using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CubeScript : MonoBehaviour
{

    private Vector3 cubeSize;

    [SerializeField]
    public int meshSize = 6;
    public void GenerateCube()
    {
        cubeSize = this.gameObject.GetComponent<MeshController>().size;

        //1. initialise MeshFilter
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        //2. initialise mesh builder
        MeshGenerator meshGenerator = new MeshGenerator(meshSize);


        //3. build our cube if true else remove if false

        if (this.gameObject.GetComponent<MeshController>().genCube)
        {
            //top vertices
            Vector3 t0 = new Vector3(cubeSize.x, cubeSize.y, -cubeSize.z); // top left point
            Vector3 t1 = new Vector3(-cubeSize.x, cubeSize.y, -cubeSize.z); // top right point
            Vector3 t2 = new Vector3(-cubeSize.x, cubeSize.y, cubeSize.z); //bottom right point of top square
            Vector3 t3 = new Vector3(cubeSize.x, cubeSize.y, cubeSize.z); // bottom left point of top square

            //bottom vertices
            Vector3 b0 = new Vector3(cubeSize.x, -cubeSize.y, -cubeSize.z); // bottom left point
            Vector3 b1 = new Vector3(-cubeSize.x, -cubeSize.y, -cubeSize.z); // bottom right point
            Vector3 b2 = new Vector3(-cubeSize.x, -cubeSize.y, cubeSize.z); //bottom right point of bottom square
            Vector3 b3 = new Vector3(cubeSize.x, -cubeSize.y, cubeSize.z); // bottom left point of bottom square


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

            this.gameObject.GetComponent<MeshController>().AddMaterials();
        }
        else if (this.gameObject.GetComponent<MeshController>().removeMesh)
        {
            //top vertices
            Vector3 t0 = new Vector3(0, 0, 0); // top left point
            Vector3 t1 = new Vector3(0, 0, 0); // top right point
            Vector3 t2 = new Vector3(0, 0, 0); //bottom right point of top square
            Vector3 t3 = new Vector3(0, 0, 0); // bottom left point of top square

            //bottom vertices
            Vector3 b0 = new Vector3(0, 0, 0); // bottom left point
            Vector3 b1 = new Vector3(0, 0, 0); // bottom right point
            Vector3 b2 = new Vector3(0, 0, 0 ); //bottom right point of bottom square
            Vector3 b3 = new Vector3(0, 0, 0); // bottom left point of bottom square


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
        }

        //4. set mesh filter's mesh to the mesh generated from our mesh builder
        meshFilter.mesh = meshGenerator.CreateMesh();
    }


}
