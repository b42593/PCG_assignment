using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PyramidScript : MonoBehaviour
{

    private float pyramidSize;

    [SerializeField] public int subMeshSize = 4;

    public void GeneratePyramid() 
    {
        pyramidSize = this.gameObject.GetComponent<MeshController>().size;

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshGenerator meshGenerator = new MeshGenerator(subMeshSize);

        //Add Points
        Vector3 top = new Vector3(0, pyramidSize, 0);

        Vector3 base0 = Quaternion.AngleAxis(0f, Vector3.up) * Vector3.forward * pyramidSize;

        Vector3 base1 = Quaternion.AngleAxis(240f, Vector3.up) * Vector3.forward * pyramidSize;

        Vector3 base2 = Quaternion.AngleAxis(120f, Vector3.up) * Vector3.forward * pyramidSize;


        //Build the triangles for our pyramid
        meshGenerator.BuildTriangle(base0, base1, base2, 0);

        meshGenerator.BuildTriangle(base1, base0, top, 1);

        meshGenerator.BuildTriangle(base2, top, base0, 2);

        meshGenerator.BuildTriangle(top, base2, base1, 3);

        this.gameObject.GetComponent<MeshController>().AddMaterials();

        meshFilter.mesh = meshGenerator.CreateMesh();
    }
}
