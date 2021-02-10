using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeController : MonoBehaviour
{
    public int colorchoice;

    [SerializeField] int startPos;
    [SerializeField] int endPos;

    GameObject maze, borders, plane, walls, horizontalWall, verticalWall, wall;

    [SerializeField] GameObject player;

    private void Start()
    {
        GenerateMaze();     
    }

    void GenerateMaze() 
    {
        //Create Parent Game Object Maze
        maze = new GameObject("Maze");
        maze.tag = "Maze";

        GenerateGround();

        //Create Borders GameObject as child of Maze
        borders = new GameObject("Borders");
        borders.tag = "Borders";

        GenerateBorders();

        borders.transform.SetParent(maze.transform);

        walls = new GameObject("Walls");
        walls.tag = "Walls";

        horizontalWall = new GameObject("Horizontal");
        horizontalWall.tag = "Walls";

        verticalWall = new GameObject("Vertical");
        verticalWall.tag = "Walls";

        //Horizontal Walls
        GenerateHorizontalWalls();

        //Vertical Walls
        GenerateVerticalWalls();

        walls.transform.SetParent(maze.transform);
        horizontalWall.transform.SetParent(walls.transform);
        verticalWall.transform.SetParent(walls.transform);

        //Generate a random number to assign a spawn point from a set of 4 spawn points for the Start and End Position.
        randomPos();

        //Start Position
        GenerateStartPosition();


        //Finish Position
        GenerateFinishPosition();

        SpawnPlayer();


    }


    void GenerateGround() 
    {
        //Create Ground GameObject as child of Maze
        plane = new GameObject("Ground");
        plane.transform.position = Vector3.zero;
        plane.tag = "Ground";
        plane.AddComponent<GroundScript>();
        plane.GetComponent<GroundScript>().GeneratePlane();
        plane.GetComponent<GroundScript>().AddMaterials(6);
        plane.AddComponent<MeshCollider>();
        plane.transform.SetParent(maze.transform);
    }

    void GenerateBorders() 
    {
        //Create each border as child of Borders
        //Left Vertical Border
        wall = new GameObject("Border");
        wall.transform.position = new Vector3(0.5f, 0.5f, 9.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 9.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Borders";
        wall.transform.SetParent(borders.transform);

        //Right Vertical Border
        wall = new GameObject("Border");
        wall.transform.position = new Vector3(19.5f, 0.5f, 9.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 9.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Borders";
        wall.transform.SetParent(borders.transform);

        //Top Border
        wall = new GameObject("Border");
        wall.transform.position = new Vector3(10f, 0.5f, 19.5f);
        wall.AddComponent<WallScript>(); 
        wall.GetComponent<WallScript>().size = new Vector3(10f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Borders";
        wall.transform.SetParent(borders.transform);

        //Bottom Border
        wall = new GameObject("Border");
        wall.transform.position = new Vector3(10f, 0.5f, -0.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(10f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Borders";
        wall.transform.SetParent(borders.transform);
    }

    void GenerateHorizontalWalls() 
    {
        wall = new GameObject("Horizontal Wall");
        wall.transform.position = new Vector3(17.5f, 0.5f, 2f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(1.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(horizontalWall.transform);

        wall = new GameObject("Horizontal Wall");
        wall.transform.position = new Vector3(6.5f, 0.5f, 2f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(5.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(horizontalWall.transform);

        wall = new GameObject("Horizontal Wall");
        wall.transform.position = new Vector3(6.5f, 0.5f, 11.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(5.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(horizontalWall.transform);

        wall = new GameObject("Horizontal Wall");
        wall.transform.position = new Vector3(13.5f, 0.5f, 5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(5.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(horizontalWall.transform);

        wall = new GameObject("Horizontal Wall");
        wall.transform.position = new Vector3(13.5f, 0.5f, 7.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(3.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(horizontalWall.transform);

        wall = new GameObject("Horizontal Wall");
        wall.transform.position = new Vector3(14.5f, 0.5f, 11.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(1.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(horizontalWall.transform);

        wall = new GameObject("Horizontal Wall");
        wall.transform.position = new Vector3(11.5f, 0.5f, 14f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(7.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(horizontalWall.transform);

        wall = new GameObject("Horizontal Wall");
        wall.transform.position = new Vector3(8.5f, 0.5f, 16.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(7.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(horizontalWall.transform);


    }

    void GenerateVerticalWalls()
    {
        wall = new GameObject("Vertical Wall");
        wall.transform.position = new Vector3(14f, 0.5f, 3f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 1.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(verticalWall.transform);

        wall = new GameObject("Vertical Wall");
        wall.transform.position = new Vector3(7.5f, 0.5f, 7f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 2.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(verticalWall.transform);

        wall = new GameObject("Vertical Wall");
        wall.transform.position = new Vector3(4f, 0.5f, 8f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 3.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(verticalWall.transform);

        wall = new GameObject("Vertical Wall");
        wall.transform.position = new Vector3(12.5f, 0.5f, 10.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 1.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(verticalWall.transform);

        wall = new GameObject("Vertical Wall");
        wall.transform.position = new Vector3(16.5f, 0.5f, 10f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 2f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(verticalWall.transform);

        wall = new GameObject("Vertical Wall");
        wall.transform.position = new Vector3(13f, 0.5f, 18.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(verticalWall.transform);

        wall = new GameObject("Vertical Wall");
        wall.transform.position = new Vector3(11f, 0.5f, 17.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(verticalWall.transform);


        wall = new GameObject("Vertical Wall");
        wall.transform.position = new Vector3(8.5f, 0.5f, 18.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(verticalWall.transform);

        wall = new GameObject("Vertical Wall");
        wall.transform.position = new Vector3(6f, 0.5f, 17.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(verticalWall.transform);

        wall = new GameObject("Vertical Wall");
        wall.transform.position = new Vector3(3.5f, 0.5f, 18.5f);
        wall.AddComponent<WallScript>();
        wall.GetComponent<WallScript>().size = new Vector3(0.5f, 3f, 0.5f);
        wall.GetComponent<WallScript>().GenerateWall();
        wall.GetComponent<WallScript>().AddMaterials(1);
        wall.AddComponent<MeshCollider>();
        wall.tag = "Walls";
        wall.transform.SetParent(verticalWall.transform);

    }


    void randomPos() 
    {
        do
        {
            startPos = UnityEngine.Random.Range(0, 4);
            endPos = UnityEngine.Random.Range(0, 4);
        }
        while (startPos == endPos || (startPos == 2 && endPos == 0));       
    }


    void GenerateStartPosition() 
    {
        //Create Ground GameObject as child of Maze
        plane = new GameObject("Start");
        plane.tag = "Start";
        plane.AddComponent<GroundScript>();
        plane.GetComponent<GroundScript>().width = 2;
        plane.GetComponent<GroundScript>().height = 2;
        plane.GetComponent<GroundScript>().GeneratePlane();
        plane.GetComponent<GroundScript>().AddMaterials(0);
        plane.AddComponent<MeshCollider>();
        plane.transform.SetParent(maze.transform);

        if (startPos == 0) 
        {
            plane.transform.position = new Vector3(1f, 0.01f, 0.25f);
        }
        if (startPos == 1)
        {
            plane.transform.position = new Vector3(1f, 0.01f, 17.5f);
        }
       
        if (startPos == 2)
        {
            plane.transform.position = new Vector3(17.5f, 0.01f, 0.25f);
        }

        if (startPos == 3)
        {
            plane.transform.position = new Vector3(1.58f, 0.01f, 9.7f);
        }
    }

    void GenerateFinishPosition()
    {

        plane = new GameObject("Finish");
        
        plane.tag = "End";
        plane.AddComponent<GroundScript>();
        plane.GetComponent<GroundScript>().width = 2;
        plane.GetComponent<GroundScript>().height = 2;
        plane.GetComponent<GroundScript>().GeneratePlane();
        plane.GetComponent<GroundScript>().AddMaterials(4);
        plane.AddComponent<MeshCollider>();
        plane.AddComponent<MazeFinishScript>();
        plane.GetComponent<MeshCollider>().convex = true;
        plane.GetComponent<MeshCollider>().isTrigger = true;
        plane.transform.SetParent(maze.transform);

        if (endPos == 0) 
        {
            plane.transform.position = new Vector3(1f, 0.01f, 0.25f); 
        }

        if (endPos == 1)
        {
            plane.transform.position = new Vector3(1f, 0.01f, 17.5f);

        }

        if (endPos == 2)
        {
            plane.transform.position = new Vector3(17.5f, 0.01f, 0.25f);

        }

        if (endPos == 3)
        {
            plane.transform.position = new Vector3(1.58f, 0.01f, 9.7f);
        }
    }


    void SpawnPlayer() 
    {
        Vector3 spawnPoint = GameObject.FindGameObjectWithTag("Start").transform.position;
        Vector3 playerPos = new Vector3(spawnPoint.x, 1f, spawnPoint.z);
        Instantiate(player, playerPos, Quaternion.identity);
    }
}
