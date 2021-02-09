using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
class TerrainTextureData
{
    public Texture2D terrainTexture;
    public float minHeight;
    public float maxHeight;
    public Vector2 tileSize;
}

[System.Serializable]
class TreeData
{
    public GameObject treeMesh;
    public float minHeight;
    public float maxHeight;
}


public class LoadHeightmapImage : MonoBehaviour
{

    private Terrain terrain;
    private TerrainData terrainData;

    [SerializeField] private Texture2D[] heightMapImages;
    private Texture2D heightMapImage;
    public int imageChosen;


    [SerializeField] private Vector3 heightMapScale = new Vector3(1, 1, 1);


    //variables for generating terrain using perlin noise
    [SerializeField] private float perlinNoiseWidthScale = 0.01f;

    [SerializeField] private float perlinNoiseHeightScale = 0.01f;



    //variables for adding textures to our terrain
    [SerializeField]
    private List<TerrainTextureData> terrainTextureDataList;

    [SerializeField]
    private float terrainTextureBlendOffset = 0.01f;

    [SerializeField]
    private List<TreeData> treeDataList;

    [SerializeField]
    private int maxTrees = 2000;

    [SerializeField]
    private int treeSpacing = 10;

    [SerializeField]
    private int terrainLayerIndex = 8;

    [SerializeField]
    private float randomXRange = 5.0f;

    [SerializeField]
    private float randomZRange = 5.0f;


    [SerializeField] private GameObject water;
    [SerializeField] private GameObject fog;
    [SerializeField] private GameObject clouds;

    [SerializeField] private float minWaterHeight;
    [SerializeField] private float maxWaterHeight;
    [SerializeField] private float waterHeight;

    GameObject fogParent, cloudParent;

    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        terrain = this.GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;

        imageChosen = Random.Range(0, heightMapImages.Length);
        heightMapImage = heightMapImages[imageChosen];

        fogParent = new GameObject("FogParent");
        cloudParent = new GameObject("CloudParent");

        waterHeight = Random.Range(minWaterHeight, maxWaterHeight);

        UpdateHeightmap();
    }

    void UpdateHeightmap()
    {
        //creates a new empty 2D array of float based on the dimensions of heightmap resolution set in the settings
        //float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        //gets the height map data that already exists in the terrain and loads it into a 2D array then addstextures, trees,water and other effects and finally the player
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {

                int heightMapWidth = (int)(width * heightMapScale.x);
                int heightMapHeight = (int)(height * heightMapScale.z);


                heightMap[width, height] = heightMapImage.GetPixel(heightMapWidth, heightMapHeight).grayscale * heightMapScale.y;


                heightMap[width, height] = Mathf.PerlinNoise(heightMap[width, height] * perlinNoiseWidthScale, heightMap[width, height] * perlinNoiseHeightScale);
            }
        }

        terrainData.SetHeights(0, 0, heightMap);

        TerrainTexture();
        AddTree();
        AddWater();
        AddFog();
        AddClouds();

        SpawnPlayer();

    }

    //this method is going to add textures to the terrain
    void TerrainTexture()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureDataList.Count];

        for (int i = 0; i < terrainTextureDataList.Count; i++)
        {
            terrainLayers[i] = new TerrainLayer();
            terrainLayers[i].diffuseTexture = terrainTextureDataList[i].terrainTexture;
            terrainLayers[i].tileSize = terrainTextureDataList[i].tileSize;
        }

        terrainData.terrainLayers = terrainLayers;

        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        float[,,] alphaMapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] splatmap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureDataList.Count; i++)
                {
                    float minHeight = terrainTextureDataList[i].minHeight - terrainTextureBlendOffset;
                    float maxHeight = terrainTextureDataList[i].maxHeight + terrainTextureBlendOffset;

                    if (heightMap[width, height] >= minHeight && heightMap[width, height] <= maxHeight)
                    {
                        splatmap[i] = 1;
                    }
                }

                NormaliseSplatMap(splatmap);

                for (int j = 0; j < terrainTextureDataList.Count; j++)
                {
                    alphaMapList[width, height, j] = splatmap[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMapList);
    }

    void NormaliseSplatMap(float[] splatmap)
    {
        float total = 0;

        for (int i = 0; i < splatmap.Length; i++)
        {
            total += splatmap[i];
        }

        for (int i = 0; i < splatmap.Length; i++)
        {
            splatmap[i] = splatmap[i] / total;
        }
    }

    //this method is going to add trees to the terrain
    void AddTree()
    {
        TreePrototype[] trees = new TreePrototype[treeDataList.Count];

        for (int i = 0; i < treeDataList.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeDataList[i].treeMesh;
        }

        terrainData.treePrototypes = trees;

        List<TreeInstance> treeInstanceList = new List<TreeInstance>();

        
        for (int z = 0; z < terrainData.size.z; z += treeSpacing)
        {
            for (int x = 0; x < terrainData.size.x; x += treeSpacing)
            {
                for (int treePrototypeIndex = 0; treePrototypeIndex < trees.Length; treePrototypeIndex++)
                {
                    if (treeInstanceList.Count < maxTrees)
                    {
                        float currentHeight = terrainData.GetHeight(x,z) / terrainData.size.y;

                        if (currentHeight >= treeDataList[treePrototypeIndex].minHeight &&
                            currentHeight <= treeDataList[treePrototypeIndex].maxHeight)
                        {
                            float randomX = (x + Random.Range(-randomXRange, randomXRange)) / terrainData.size.x;
                            float randomZ = (z + Random.Range(-randomZRange, randomZRange)) / terrainData.size.z;

                            TreeInstance treeInstance = new TreeInstance();

                            treeInstance.position = new Vector3(randomX, currentHeight, randomZ);

                            Vector3 treePosition = new Vector3(treeInstance.position.x * terrainData.size.x,
                                                                treeInstance.position.y * terrainData.size.y,
                                                                treeInstance.position.z * terrainData.size.z) + this.transform.position;

                            

                            RaycastHit raycastHit;
                            int layerMask = 1 << terrainLayerIndex;

                            if (Physics.Raycast(treePosition, Vector3.down, out raycastHit, 100, layerMask) ||
                                Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                            {
                                float treeHeight = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                treeInstance.position = new Vector3(treeInstance.position.x, treeHeight, treeInstance.position.z);

                                treeInstance.rotation = Random.Range(0, 360);
                                treeInstance.prototypeIndex = treePrototypeIndex;
                                treeInstance.color = Color.white;
                                treeInstance.lightmapColor = Color.white;
                                treeInstance.heightScale = 0.95f;
                                treeInstance.widthScale = 0.95f;

                                treeInstanceList.Add(treeInstance);
                            }
                        }
                    }
                }
            }
            
        }

        terrainData.treeInstances = treeInstanceList.ToArray();
    }

    //this method is going to add water to the terrain
    void AddWater()
    {
        GameObject waterGameObject = GameObject.Find("water");

        if (!waterGameObject)
        {
            waterGameObject = Instantiate(water, this.transform.position, this.transform.rotation);
            waterGameObject.name = "water";
        }

        waterGameObject.transform.position = this.transform.position + new Vector3(
            terrainData.size.x / 2,
            waterHeight * terrainData.size.y,
            terrainData.size.z / 2);

        waterGameObject.transform.localScale = new Vector3(terrainData.size.x, 1, terrainData.size.z);
    }

    //this method is going to add fog to the terrain at random parts
    void AddFog()
    {
        GameObject fogObject = GameObject.Find("Fog");

        if (!fogObject)
        {
            for(int counter = 0; counter < 10; counter++) 
            {
                float xPos = Random.Range(0, terrainData.size.x);
                float yPos = Random.Range(240, 287);
                float zPos = Random.Range(0, terrainData.size.z);

                fogObject = Instantiate(fog, this.transform.position, this.transform.rotation);
                fogObject.transform.position = new Vector3(xPos, yPos, zPos);
                fogObject.transform.localScale = new Vector3(2f, 10f, 2f);
                fogObject.name = "Fog";
                fogObject.transform.SetParent(fogParent.transform);
            }
        }
        
    }

    //this method is going to add fog to the terrain at random parts some with rain some without
    void AddClouds()
    {
        GameObject cloud = GameObject.Find("Clouds");
        

        if (!cloud)
        {
            for (int counter = 0; counter < 10; counter++)
            {
                float xPos = Random.Range(0, terrainData.size.x);
                float yPos = Random.Range(300, 350);
                float zPos = Random.Range(0, terrainData.size.z);

                cloud = Instantiate(clouds, this.transform.position, this.transform.rotation);

                cloud.transform.position = new Vector3(xPos, yPos, zPos);
                cloud.transform.localScale = new Vector3(2f, 10f, 2f);
                
                cloud.name = "Cloud";
                cloud.transform.SetParent(cloudParent.transform);
            }
        }

    }

    void SpawnPlayer()
    {
        float xPos = Random.Range(0, terrainData.size.x);
        float yPos = Random.Range(652, terrainData.size.y);
        float zPos = Random.Range(0, terrainData.size.z);

        Vector3 playerPos = new Vector3(xPos, yPos, zPos);
        Instantiate(player, playerPos, Quaternion.identity);
    }
}
