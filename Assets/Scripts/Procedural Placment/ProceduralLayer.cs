using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* PROCEDURAL LAYER 
 * Handles creating the layers for prefabs on mesh. Gets the vertices from procedural placement and checks whether the height falls 
 * in specified range of editor. If it does not then remove vertex for mesh 
 * 
*/
public class ProceduralLayer : MonoBehaviour
{
    private const float resizeValue = 0.75f;

    public List<GameObject> CreatePrefabLayers(MeshTile meshTile, ProceduralObject[] proceduralObjects, float[,] noiseMap, float heightMultiplier, GameObject parentObj, int size)
    {
        // store all prefabs in list of layers 
        List<GameObject> prefabLayers = new List<GameObject>();

        // get the vector3 positions for prefabs from procedural placement which it gets from poisson disk sample 
        List<Vector3[]> prefabPositions = ProceduralPlacement.CreateVerticesForPrefabs(meshTile, proceduralObjects, noiseMap, heightMultiplier, size);

        // iterate through all procedural objects, check if there vector position falls in correct height range, if so instantiate the prefab 
        // and parent it to the the prefab layers 
        for (int i = 0; i < proceduralObjects.Length; i++)
        {
            GameObject prefabLayer = new GameObject(proceduralObjects[i].nameKey);
            for (int j = 0; j < prefabPositions[i].Length; j++)
            {
                float prefabHeight = prefabPositions[i][j].y;    
                if (prefabHeight > proceduralObjects[i].minSpawnHeight * heightMultiplier && prefabHeight < proceduralObjects[i].maxSpawnHeight * heightMultiplier)
                {
                
                    int randIndex = Random.Range(0, proceduralObjects[i].prefabVariations.Length);

                    Instantiate(proceduralObjects[i].prefabVariations[randIndex], 
                        prefabPositions[i][j], 
                        new Quaternion(), 
                        prefabLayer.transform);
                }
            }
            prefabLayer.transform.parent = parentObj.transform;
            prefabLayer.transform.rotation = new Quaternion(0, 0, 0, 0);
            prefabLayers.Add(prefabLayer); 
        }
        return prefabLayers;
    }
}
