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


    [Header("Mesh Dimensions")]
    [SerializeField]
    public Vector3 size = Vector3.one;


    [Header("Material Color")]
    //[SerializeField] public int colorChoice;

    [SerializeField] public int[] faces;
    [SerializeField] private Color[] colors = { Color.green, Color.red, Color.white, Color.blue, Color.cyan, Color.black, Color.grey };

    private int colorChoice;


    private List<Material> materialList;
    private int subMeshsize;    

    // Update is called once per frame
    void OnValidate()
    {
        if (removeMesh)
        {
            genCube = false;
        }
        if (removeMesh || genCube)
        {
            this.gameObject.GetComponent<CubeScript>().GenerateCube();
            subMeshsize = this.gameObject.GetComponent<CubeScript>().meshSize;
        }
        
    }

    public void AddMaterials()
    {
        //5. Assign materials to the mesh renderer's materials list
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
