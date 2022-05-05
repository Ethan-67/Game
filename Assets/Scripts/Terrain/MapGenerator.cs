using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;

/* MAP GENERATOR 
 * Main driver code for terrain generation. Much of the functionality is so it can be used in editor. Calls all revelant 
 * methods from each class to create terrain. Noise to produce a noise map, Apply texture to apply the noise map to a 
 * plane so it can be seen in editor, Mesh Drawer so it can use the noise map to generate a 3D mesh. Furthermore 
 * RequestMesh allows EndlessTerrain to call this class to create a mesh at runtime.
 * SOURCE https://github.com/SebLague/Procedural-Landmass-Generation/blob/master/Proc%20Gen%20E13/Assets/Scripts/MapGenerator.cs
*/

public class MapGenerator : MonoBehaviour
{
    // draw mode test different use cases 
    public enum Drawmode { Noise, Color, Mesh }
    public Drawmode drawmode;

    // seed of terrain 
    public int seed;
    // dimensions 
    public int width;
    public int height;

    // shift in noisemap 
    public Vector2 offset;

    // params for extension do not change anything at this time
    public int octaves;
    public float frequency;
    public float lacunarity;
    public float amplitude;
    public float persistance;
    // how zoomed in noise should be 
    public float scale;
    // height of final 3d mesh 
    public int heightMultiplier;
    // how much terrain should be flat 
    public AnimationCurve meshCurve;
    // editor variables
    public bool autoUpdate;
    // all regions of 3d mesh 
    public Region[] regions; 
    // size of each terrain plot 
    public static int chunkSize = 256;
    // should details be added on mesh 
    public bool proceduralPlacedObjects = true;

    // stores all objects to add on top of terrain 
    public ProceduralObject[] proceduralObjects;

    private ProceduralLayer proceduralLayer;
    // coordinates where terrain should not be generated like at the start 
    private Vector2[] unvalidCoordinates;

    [SerializeField]
    private Material cloudMat; 

    // main method to generate all terrain depending on draw mode calls corrosponding methods
    public void Generate()
    {
        // get procedural layer attatched to 3d mesh if there is one 
        if (proceduralLayer == null)
        {
            proceduralLayer = GameObject.Find("MapGenerator").GetComponent<ProceduralLayer>();
        }

        // initialise noise and colour map 
        float[,] noiseMap = Noise.GenerateNoiseMap(seed, width, height, scale, offset, octaves, frequency, lacunarity, persistance, amplitude);
        Color[] colorMap = ApplyTexture.CreateColorMapFromNoiseMap(noiseMap, regions, width, height);
        // if testing noise map, apply noise map to place 
        if (drawmode == Drawmode.Noise)
        { 
            ApplyTexture.ApplyNoiseMap(noiseMap, width, height);
        }
        else if (drawmode == Drawmode.Color) /* if testing colour map convert noise map to colour map and apply to plance*/
        {
            // create a colormap with noisemap 
            ApplyTexture.ApplyColorMap(GameObject.Find("NoiseTest"), noiseMap, regions, width, height);
        }
        else if (drawmode == Drawmode.Mesh) /* if testing mesh generate a 3d mesh */
        {
            // creates mesh from noise map
            MeshTile meshTile = MeshDrawer.CreateMeshFromNoiseMap(noiseMap, meshCurve, heightMultiplier, width, height);
            // creates mesh 
            GameObject meshObj = GameObject.Find("Mesh");
            MeshFilter meshFilter = meshObj.GetComponent<MeshFilter>();
            meshFilter.sharedMesh = meshTile.CreateMesh();
            // apply colour map to mesh 
            ApplyTexture.ApplyColorMapToMesh(GameObject.Find("Mesh"), noiseMap, regions, width, height);

            // if procedural objects is true destroy last procedural object layer and create a new one 
            if (proceduralPlacedObjects)
            {
                // destroy last procedural object layer 
                meshObj.transform.DetachChildren();
                for (int i = 0; i < proceduralObjects.Length; i++)
                {
                    GameObject g = GameObject.Find(proceduralObjects[i].nameKey);
                    DestroyImmediate(g);
                }

                // generate new layer and attatch to mesh 
                meshTile.prefabsLayers = proceduralLayer.CreatePrefabLayers(meshTile, proceduralObjects, noiseMap, heightMultiplier, meshObj, width);
                foreach (GameObject layer in meshTile.prefabsLayers)
                {
                    layer.transform.parent = meshObj.transform;
                    layer.transform.position = meshObj.transform.position;
                    layer.SetActive(true);
                }
            }
            
        }
    }

    // returns a 3d mesh dependent on world coordinate 
    public MeshTile RequestMesh(GameObject meshObject, int chunkSize, Vector2 coord)
    {
        // calculate offset noise map should have based on world coord
        Vector2 tileOffset = CalculateOffset(coord);

        // initalise noise map and meshInfo struct 
        MeshInfo meshInfo = new MeshInfo();
        float[,] noiseMap = null;   

        // if coordinate invalid generate a blank map (used in starting area)
        if (IsUnvalidCoordinate(coord))
        {
            
            noiseMap = Noise.GenerateBlankMap(chunkSize, chunkSize);
        }
        else 
        {
            noiseMap = Noise.GenerateNoiseMap(seed, chunkSize, chunkSize, scale, tileOffset, octaves, frequency, lacunarity, persistance, amplitude);
        }
        // save noise map and 3d mesh 
        meshInfo.noiseMap = noiseMap; 
        meshInfo.meshTile = MeshDrawer.CreateMeshFromNoiseMap(meshInfo.noiseMap, meshCurve, heightMultiplier, chunkSize, chunkSize); 
        
        ApplyTexture.ApplyColorMapToMesh(meshObject, meshInfo.noiseMap, regions, chunkSize, chunkSize);

        // if procedural objects are true, then add them to the meshs procedural layer 
        if (proceduralPlacedObjects)
        {
            meshInfo.meshTile.prefabsLayers = proceduralLayer.CreatePrefabLayers(meshInfo.meshTile, proceduralObjects, noiseMap, heightMultiplier, meshObject, width);
            foreach (GameObject layer in meshInfo.meshTile.prefabsLayers)
            {
                layer.transform.parent = meshObject.transform;
                layer.transform.position = meshObject.transform.position;
                layer.SetActive(true);
            }
        }

        return meshInfo.meshTile; 
    }

    // calculates offset dependent on the world coordinate for chunk 
    private Vector2 CalculateOffset(Vector2 coord)
    {
        Vector2 tileOffset = new Vector2();
        float Ascale = scale; 

        if (coord.x > 0)
            tileOffset.y = Mathf.Abs(coord.x) * -Ascale;
        else
            tileOffset.y = Mathf.Abs(coord.x) * Ascale;
        
        if (coord.y > 0)
            tileOffset.x = Mathf.Abs(coord.y) * -Ascale;
        else
            tileOffset.x = Mathf.Abs(coord.y) * Ascale;
        
        return tileOffset;
    }

    // called before update method, used to initialise various variables 
    private void Start()
    {
        proceduralLayer = GameObject.Find("MapGenerator").GetComponent<ProceduralLayer>();

        unvalidCoordinates = new Vector2[5 * 5 + 1];
        for (int i = -1, r = 0; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                unvalidCoordinates[r++] = new Vector2(i, j);
            }
        }
    }

    // check whether coordinate is valid or not (used in starting area) 
    private bool IsUnvalidCoordinate(Vector2 coord)
    {
        for (int i = 0; i < unvalidCoordinates.Length; i++)
        {
            if (coord.x == unvalidCoordinates[i].x && coord.y == unvalidCoordinates[i].y)
            {
                return true; 
            }
        }
        return false; 
    }

    public Material GetCloudMat()
    {
        return cloudMat; 
    }
}

public struct MeshInfo
{
    public float[,] noiseMap;
    public MeshTile meshTile;
}

// regions used to create colour map
[System.Serializable]
public struct Region    
{
    public string name;
    public float range;
    public Color color;
};

// procedural object used for Poisson disk sample of meshes 
[System.Serializable]
public struct ProceduralObject
{
    public string nameKey;

    public GameObject[] prefabVariations;

    // Sampling settings 
    public float radius;
    public int sampleSize;

    // bounds of height which this prefab can be placed at
    [Range(0,1)]
    public float maxSpawnHeight;
    [Range(0,1)]
    public float minSpawnHeight;

   
}




