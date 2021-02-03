using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class RoadScript : MonoBehaviour
{
    [SerializeField]
    private float radius; // this defines the radius of the path

    [SerializeField]
    private float segments;

    [SerializeField]
    private float lineWidth = 0.5f; // middle white line road marker

    [SerializeField]
    private float roadWidth = 10f; // width of the road on each side of the line

    [SerializeField]
    private float edgeWidth = 1f; // widht of our road barrier at the edge of our road
    [SerializeField]
    private float edgeHeight = 1f; // widht of our road barrier at the edge of our road

    [SerializeField]
    private int submeshSize = 6;

    [SerializeField]
    private float wavyness = 90f;

    [SerializeField]
    private float waveScale = 2.5f;

    [SerializeField]
    private Vector2 waveOffset;

    [SerializeField]
    private Vector2 waveStep = new Vector2(0.01f, 0.01f);

    [SerializeField]
    private bool stripeCheck = true;

    [SerializeField]
    private bool roadstripeCheck = true;

    [SerializeField]
    private GameObject car;

    private GameObject startWaypoint, waypoint, waypoints;

    public List<Vector3> points;

    private List<Material> materialList;

    private Color[] colors = { Color.green, Color.red, Color.white, Color.blue, Color.cyan, Color.black, Color.grey };

    private int randomStepSize;

    GameManager gm;


    public void GenerateRoad()
    {
        radius = Random.Range(90, 200);
        segments = Random.Range(300, 500);
        waveOffset.x = Random.Range(10, 100);
        waveOffset.y = Random.Range(10, 100);

        car = Resources.Load<GameObject>("StandardAssets/Vehicles/Car/Prefabs/Car");

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshCollider meshCollider = this.GetComponent<MeshCollider>();

        MeshGenerator meshGenerator = new MeshGenerator(submeshSize);


        //1. Divide the circular race track into segments denoted in degrees and each point is defined by each segment
        //   Create the points and store them in a list
        float segmentDegrees = 360f / segments;

        //List<Vector3> points = new List<Vector3>();
        points = new List<Vector3>();

        for (float degrees = 0; degrees < 360f; degrees += segmentDegrees)
        {
            Vector3 point = Quaternion.AngleAxis(degrees, Vector3.up) * Vector3.forward * radius;
            points.Add(point);
        }

        //generate random turns
        Vector2 wave = this.waveOffset;

        for (int i = 0; i < points.Count; i++)
        {
            wave += waveStep;

            Vector3 point = points[i];
            Vector3 centreDirection = point.normalized;

            float noise = Mathf.PerlinNoise(wave.x * waveScale, wave.y * waveScale);
            noise *= wavyness;

            float control = Mathf.PingPong(i, points.Count / 2f) / (points.Count / 2f);

            points[i] += centreDirection * noise * control;
        }




        //2. function to define the path - the path is defined by each segment
        for (int i = 1; i < points.Count + 1; i++)
        {
            Vector3 pPrev = points[i - 1];
            Vector3 pCurr = points[i % points.Count];
            Vector3 pNext = points[(i + 1) % points.Count];

            ExtrudeRoad(meshGenerator, pPrev, pCurr, pNext);
        }

        
        car.transform.position = points[points.Count - 15];
        car.transform.LookAt(points[0]);

        Instantiate(car, car.transform.position, Quaternion.LookRotation(new Vector3 (90f,0f,0f)));

        GenerateWaypoints();

     

        meshFilter.mesh = meshGenerator.CreateMesh();

        meshCollider.sharedMesh = meshFilter.mesh;
    }


    //3. This method will used to create the different segments for each segment we are going to draw the road marker 
        //   (i.e. white line in the middle), draw the road on each side of the line, draw the edges - all these are going 
        //   to be placed in different positions
        private void ExtrudeRoad(MeshGenerator meshGenerator, Vector3 pPrev, Vector3 pCurr, Vector3 pNext)
        {
            //Road Line
            Vector3 offset = Vector3.zero;
            Vector3 targetOffset = Vector3.forward * lineWidth;

            //THE SIDEWALLS

            int roadstripeSubmesh = 0;

            if (stripeCheck)
            {
            roadstripeSubmesh = 1;
            }

             roadstripeCheck = !roadstripeCheck;


            MakeRoadQuad(meshGenerator, pPrev, pCurr, pNext, offset, targetOffset, roadstripeSubmesh);

            //Road
            offset += targetOffset;
            targetOffset = Vector3.forward * roadWidth;

            MakeRoadQuad(meshGenerator, pPrev, pCurr, pNext, offset, targetOffset, 1);


            //THE SIDEWALLS

            int stripeSubmesh = 2;

            if (stripeCheck)
            {
                stripeSubmesh = 3;
            }

            stripeCheck = !stripeCheck;

            //Edge
            offset += targetOffset;
            //targetOffset = Vector3.forward * edgeWidth;
            targetOffset = Vector3.up * edgeHeight;

            MakeRoadQuad(meshGenerator, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

            //EdgeTop
            offset += targetOffset;
            //targetOffset = Vector3.forward * edgeWidth;
            targetOffset = Vector3.forward * edgeHeight;


            MakeRoadQuad(meshGenerator, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);


            //Edge
            offset += targetOffset;
            //targetOffset = Vector3.forward * edgeWidth;
            targetOffset = -Vector3.up * edgeHeight;


            MakeRoadQuad(meshGenerator, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

        }


    //4. Create each quad
    private void MakeRoadQuad(MeshGenerator meshGenerator, Vector3 pPrev, Vector3 pCurr, Vector3 pNext,
                              Vector3 offset, Vector3 targetOffset, int submesh)
    {
        Vector3 forward = (pNext - pCurr).normalized;
        Vector3 forwardPrev = (pCurr - pPrev).normalized;


        //Build Outer Track
        Quaternion perp = Quaternion.LookRotation(Vector3.Cross(forward, Vector3.up));
        Quaternion perpPrev = Quaternion.LookRotation(Vector3.Cross(forwardPrev, Vector3.up));

        Vector3 topLeft = pCurr + (perpPrev * offset);
        Vector3 topRight = pCurr + (perpPrev * (offset + targetOffset));

        Vector3 bottomLeft = pNext + (perp * offset);
        Vector3 bottomRight = pNext + (perp * (offset + targetOffset));

        meshGenerator.BuildTriangle(topLeft, topRight, bottomLeft, submesh);
        meshGenerator.BuildTriangle(topRight, bottomRight, bottomLeft, submesh);


        //Build Inner Track
        perp = Quaternion.LookRotation(Vector3.Cross(-forward, Vector3.up));
        perpPrev = Quaternion.LookRotation(Vector3.Cross(-forwardPrev, Vector3.up));

        topLeft = pCurr + (perpPrev * offset);
        topRight = pCurr + (perpPrev * (offset + targetOffset));

        bottomLeft = pNext + (perp * offset);
        bottomRight = pNext + (perp * (offset + targetOffset));

        meshGenerator.BuildTriangle(bottomLeft, bottomRight, topLeft, submesh);
        meshGenerator.BuildTriangle(bottomRight, topRight, topLeft, submesh);
    }



    //Generate Waypoints
    private void GenerateWaypoints() 
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        waypoints = new GameObject("Waypoints");
        waypoints.transform.position = new Vector3(0, 0, 0);

        GenerateStartWaypoints();
        GenerateOtherWaypoints();

        gm.waypointReference = GameObject.FindGameObjectsWithTag("Waypoints");
    }


    private void GenerateOtherWaypoints()
    {
        for (int x = 30; x < points.Count; x += 30) 
        {
            waypoint = new GameObject("Waypoint");
            waypoint.AddComponent<BoxCollider>();
            waypoint.AddComponent<WaypointsCollisionScript>();
            waypoint.GetComponent<BoxCollider>().size = new Vector3(20f, 1f, 20f);
            waypoint.GetComponent<BoxCollider>().isTrigger = true;
            waypoint.transform.position = new Vector3(points[x].x, 1, points[x].z);
            waypoint.tag = "Waypoints";
            waypoint.transform.SetParent(waypoints.transform);
        }
        
    }


    private void GenerateStartWaypoints() 
    {
        startWaypoint = new GameObject("StartWaypoint");
        startWaypoint.AddComponent<BoxCollider>();
        startWaypoint.AddComponent<WaypointsCollisionScript>();
        startWaypoint.GetComponent<BoxCollider>().size = new Vector3(20f, 1f, 20f);
        startWaypoint.GetComponent<BoxCollider>().isTrigger = true;
        startWaypoint.transform.position = new Vector3(points[0].x, 1, points[0].z);
        startWaypoint.tag = "Waypoints";
        startWaypoint.transform.SetParent(waypoints.transform);
    }
    

    public void AddMaterials()
    {
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();

        materialList = new List<Material>();


        Material roadLines = new Material(Shader.Find("Specular"));
        roadLines.color = colors[2];
        materialList.Add(roadLines);

        Material tarmac = new Material(Shader.Find("Specular"));
        tarmac.color = colors[5];
        materialList.Add(tarmac);

        Material edges = new Material(Shader.Find("Specular"));
        edges.color = colors[2];
        materialList.Add(edges);

        Material barrier = new Material(Shader.Find("Specular"));
        barrier.color = colors[1];
        materialList.Add(barrier);

        meshRenderer.materials = materialList.ToArray();
    }


   
}
