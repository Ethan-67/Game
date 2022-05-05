using System;
using System.Collections.Generic;
using UnityEngine;

/* PROCEDURAL PLACEMENT 
 * Create a list of coordinate to place each prefab, uses Poisson Disk Sampling to create sample. Gets the vertices in the mesh 
 * finds the closest one to sample coordinate to get the height value.
*/
public static class ProceduralPlacement
{
    private const float resizeValue = 0.75f;

    public static List<Vector3[]> CreateVerticesForPrefabs(MeshTile meshTile, ProceduralObject[] proceduralObjects, float[,] noiseMap, float heightMultipler, int size)
    {
        // store list of vector3 to corrosponding prefab procedural object 
        List<Vector3[]> prefabPositions = new List<Vector3[]>(); 
        // iterate through each prefab convert vector2 returned by poisson disk to vector3 using the height of the mesh 
        foreach (ProceduralObject proceduralObj in proceduralObjects)
        {
            // get vector2 
            Vector2[] proceduralCoords = PoissonDiskSampling.SamplePoints(size, proceduralObj.sampleSize, proceduralObj.radius);

            int halfSize = size / 2; 

            // store corrosponding vector3's
            Vector3[] proceduralCoords3D = new Vector3[proceduralCoords.Length];
            
            // iterate through each vector2 
            for (int i = 0; i < proceduralCoords.Length; i++)
            {
                // remove points close to border of mesh 
                /*
                if ((int)proceduralCoords[i].y > halfSize - 1 || (int)proceduralCoords[i].x > halfSize - 1 
                    || (int)proceduralCoords[i].x < -halfSize + 1 || (int)proceduralCoords[i].y < halfSize + 1)
                    continue;
                */

                // convert coords so they are centred over 0 instead of size / 2 
                float xCoord = proceduralCoords[i].x - halfSize;
                float zCoord = proceduralCoords[i].y - halfSize;

                // fill vector3 position with remapped x,z coords and calculate height using the mesh 
                // vertex nearest to the point
                proceduralCoords3D[i] = new Vector3(xCoord, 
                    GetHeightOfNearestVertex(meshTile.vertices, new Vector2(xCoord, zCoord), size),
                    zCoord);
            }
            // add vector3 list to corrosponding prefab lists 
            prefabPositions.Add(proceduralCoords3D); 
        }
        // return vector3 list 
        return prefabPositions;
    }


    private static float GetHeightOfNearestVertex(Vector3[] meshVertices, Vector2 prefabVertex, int size)
    {
        int halfSize = size / 2;

        // works out the index of the closest in mesh vertex to prefab vertex and returns the height of it to use for prefab 
        int index = (size * ((int) prefabVertex.y + halfSize)) + ((int) prefabVertex.x + halfSize);
        try
                {
            return meshVertices[index].y;
        }
        catch (System.Exception)
        {
            Debug.Log("GETTING HEIGHT EXCEPTION: " + size + " Prefab Coord: " + prefabVertex.x + " , " + prefabVertex.y + " Index: " + index);
            return 10f; 
        }
    }


}


