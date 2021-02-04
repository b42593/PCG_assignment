﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

[ExecuteInEditMode]
public class GroundScript : MonoBehaviour
{
    private List<Material> materialList;

    private Color[] colors = { Color.green, Color.red, Color.white, Color.blue, Color.cyan, Color.black, Color.grey };

    [SerializeField]
    public float cellSize = 1f;

    [SerializeField]
    public int width = 20;

    [SerializeField]
    public int height = 20;

    [SerializeField]
    private int subMeshSize = 1;


    public void GeneratePlane()
    {
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

        //create the quads

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

                
            }

            submesh++;
        }

        meshFilter.mesh = meshGenerator.CreateMesh();

        
    }


    public void AddMaterials(int colourPicked)
    {
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        
        Material matColor = new Material(Shader.Find("Specular"));
        matColor.color = colors[colourPicked];

        materialList = new List<Material>();
        materialList.Add(matColor);

        meshRenderer.materials = materialList.ToArray();
    }



}
