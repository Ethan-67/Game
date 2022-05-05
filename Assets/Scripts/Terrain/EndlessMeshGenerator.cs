using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ENDLESS MESH GENERATOR 
 * Responsible for loading in new chunks when the player steps over the playable area, whilst setting the old chunks to inactive. 
 * Source https://github.com/SebLague/Procedural-Landmass-Generation/blob/master/Proc%20Gen%20E13/Assets/Scripts/EndlessTerrain.cs
*/
public class EndlessMeshGenerator : MonoBehaviour
{
    // max view distance 
    public const float maxViewDst = 770f;

    // player location 
    public Transform viewer;

    // material of map
    public Material mapMat;

    // map generator instance
    static MapGenerator mapGenerator; 

    // player position 
    public static Vector2 playerPosition;
    // chunk size and chunks visible in view using the view distance
    int chunkSize;
    int chunksVisibleInViewDst;

    // all chunks that have been generated 
    Dictionary<Vector2, Chunk> chunkDictionary = new Dictionary<Vector2, Chunk>();
    // store all chunks last visible 
    List<Chunk> chunksVisibleLastUpdate = new List<Chunk>(); 

    // initialise valluse 
    private void Start()
    {
        chunkSize = MapGenerator.chunkSize;
        chunksVisibleInViewDst = (int) (maxViewDst / chunkSize);
        mapGenerator = FindObjectOfType<MapGenerator>();
    }

    // called every frame, update player position and update visble chunks 
    private void Update()
    {
        playerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks(); 
    }

    // sets chunks that should be visible in map 
    public void UpdateVisibleChunks()
    {
        // iterate through chunks that were visble and set to false 
        for (int i = 0; i < chunksVisibleLastUpdate.Count; i++)
        {
            chunksVisibleLastUpdate[i].SetVisible(false);  
        }
        chunksVisibleLastUpdate.Clear(); 

        // get player position translated into world coordinate 
        int currentChunkCoordX = Mathf.RoundToInt(viewer.position.x / chunkSize);
        int currentChunkCoordZ = Mathf.RoundToInt(viewer.position.z / chunkSize);

        // iterate through chunks visble in view 
        for (int x = -chunksVisibleInViewDst; x <= chunksVisibleInViewDst; x++)
        {
            for (int z = -chunksVisibleInViewDst; z <= chunksVisibleInViewDst; z++)
            {
                // if chunk has already been generated load the dictionary version of chunk to avoid generating new terrain when
                // terrain was already generated 
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + x, currentChunkCoordZ + z);
                if (chunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    chunkDictionary[viewedChunkCoord].UpdateChunk(); 
                    if (chunkDictionary[viewedChunkCoord].IsVisible())
                    {
                        chunksVisibleLastUpdate.Add(chunkDictionary[viewedChunkCoord]); 
                    }
                    else
                    {

                    }
                }
                else /* otherwise generate more terrain */
                {
                    chunkDictionary.Add(viewedChunkCoord, new Chunk(viewedChunkCoord, chunkSize, GameObject.Find("MapGenerator").transform, mapMat));
                }
            }
        }
    }

    // holds all mesh information for a chunk in endless terrain 
    public class Chunk
    {
        // mesh attributes 
        MeshTile meshTile; 
        GameObject meshObject;
        GameObject fogObject; 
        Vector2 position;
        Bounds bounds;

        // create a new chunk using coordinate 
        public Chunk(Vector2 coord, int size, Transform parent, Material mapMat)
        {
            position = coord * size ;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            //meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject = CreateChunk(coord, size, playerPosition, mapMat); 
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one;
            meshObject.transform.parent = parent;
            meshObject.transform.eulerAngles = new Vector3(0, 180f, 0);

            meshObject.layer = LayerMask.NameToLayer("Ground");
            
            SetVisible(false);
        }

        // create the chunk object 
        public GameObject CreateChunk(Vector2 coord, int size, Vector2 playerPosition, Material mapMat)
        {
            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);

            MeshRenderer renderer = meshObject.GetComponent<MeshRenderer>();
            renderer.material = CreateMaterial(mapMat); 

            MeshFilter meshFilter = meshObject.GetComponent<MeshFilter>();
            meshTile = mapGenerator.RequestMesh(meshObject, size, coord);
            meshFilter.mesh = meshTile.CreateMesh();

            MeshCollider meshCollider = meshObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = meshFilter.mesh;

            
            fogObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            fogObject.transform.localScale = new Vector3(26.5f, 1.1f, 26.5f);
                
            fogObject.GetComponent<MeshRenderer>().sharedMaterial = mapGenerator.GetCloudMat();
            fogObject.transform.parent = meshObject.transform;
            

            return meshObject; 
        }

        // checks whether chunk is too far from player, if so set itself to inactive 
        public void UpdateChunk()
        {
            float playerDstToNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPosition));
            bool visible = playerDstToNearestEdge <= maxViewDst;
            SetVisible(visible); 
        }

        public Material CreateMaterial(Material mapMat)
        {
            Material material = new Material(mapMat);
            return material; 
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);  
        }

        public bool IsVisible()
        {
            return meshObject.activeSelf; 
        }    

        public void OnNotVisible()
        {
            
        }
    }
}
