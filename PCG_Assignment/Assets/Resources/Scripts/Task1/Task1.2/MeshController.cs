using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

[ExecuteInEditMode]
public class MeshController : MonoBehaviour
{
    [Header("Disabler")]
    [SerializeField] public bool removeMesh = false;

    [Header("Enabler")]
    [SerializeField] public bool genCube = false;
    [SerializeField] public bool genPlane = false;
    [SerializeField] public bool genPyramid = false;

    [Header("SubMesh Size")]
    [SerializeField] public int subMeshsize;

    [Header("Mesh Dimensions")]
    [SerializeField] public Vector3 dimensions = Vector3.one;

    [Header("Size for Plane and Pyramid")]
    [SerializeField] public float size = 1f;

    [Header("Material Color")]
    [SerializeField] public int[] faces;
    [SerializeField] private Color[] colors = { Color.green, Color.red, Color.white, Color.blue, Color.cyan, Color.black, Color.grey };

    
    private int colorChoice;


    private List<Material> materialList;
      

    // Update is called once per frame
    void OnValidate()
    {
        if (removeMesh)
        {
            genCube = false;
            genPlane = false;
            genPyramid = false;

            if (!genCube || !genPlane || !genPyramid) 
            {
                subMeshsize = 6;
                this.gameObject.GetComponent<CubeScript>().GenerateCube();   
            }
            dimensions = Vector3.one;
            size = 1f;
            Array.Resize(ref faces, 0);
        }

        if (genCube)
        {
            subMeshsize = 6;
            this.gameObject.GetComponent<CubeScript>().GenerateCube();
            Array.Resize(ref faces, subMeshsize);
        }

        if (genPlane)
        {
            this.gameObject.GetComponent<PlaneScript>().GeneratePlane();
            Array.Resize(ref faces, subMeshsize);
        }

        if (genPyramid)
        {
            subMeshsize = 4;
            this.gameObject.GetComponent<PyramidScript>().GeneratePyramid();   
            Array.Resize(ref faces, subMeshsize);
        }
    }

    public void AddMaterials()
    {
        //Assign materials to the mesh renderer's materials list
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        materialList = new List<Material>();
        for (int faceCounter = 0; faceCounter < faces.Length; faceCounter++)
        {
            if (faces[faceCounter] < colors.Length)
            {
                colorChoice = faces[faceCounter];
                Material matColor = new Material(Shader.Find("Specular"));
                matColor.color = colors[colorChoice];

                materialList.Add(matColor);
                meshRenderer.materials = materialList.ToArray();
            }
            else
            {
                faces[faceCounter] = colors.Length - 1;
            }
        }
       
    }
}
