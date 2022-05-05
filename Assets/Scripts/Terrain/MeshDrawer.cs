using UnityEngine;
using System.Collections.Generic;

/* MESH GENERATOR
 * Responsible for converting a noise map into a 3d mesh. Does this by accepting a plane with the same amount of vertices as indices 
 * in the 2d noise map. Each vertex in mesh is assigned the corrosponding height value from the noise map. Then triangles are drawn 
 * from each 3 vertices to deal with normals and rendering.
 * Source https://github.com/SebLague/Procedural-Landmass-Generation/blob/master/Proc%20Gen%20E13/Assets/Scripts/MeshGenerator.cs
*/
public static class MeshDrawer
{
    // store triangles and vertices 
    public static Vector3[] vertices;
    public static int[] triangles; 
    // creates 3d mesh from noise map
    public static MeshTile CreateMeshFromNoiseMap(float[,] noiseMap, AnimationCurve meshCurve, int heightMultiplier, int width, int height)
    {
        // create vertices using height values from noise map 
        Vector3[] vertices = MapVertices(noiseMap, meshCurve, heightMultiplier, width, height);
        // create uv map for mesh, used to apply textures where each vertex has a value between 0 - 1 
        Vector2[] uvs = MapUvs(vertices, width, height);
        // specifies triangles between each 3 vertex's 
        int[] triangles = CreateTriangles(vertices, width, height);

        // create mesh tile object and update 
        MeshTile meshTile = new MeshTile();
        meshTile.UpdateMesh(vertices, uvs, triangles);

        return meshTile;
    }

    // maps index's from noise map to vertex's in a 3d mesh 
    private static Vector3[] MapVertices(float[,] noiseMap, AnimationCurve meshCurve, int heightMultiplier, int width, int height)
    {
        // vertices 1d version of noise map 
        Vector3[] vertices = new Vector3[width * height];

        // half width and height to center mesh over 0 coord. Means half the vertices start before the middle coordinate and half the vertices start
        // after, means that the position x, z are actually the centre of the mesh 
        int halfWidth = width / 2;
        int halfHeight = height / 2;
        
        // iterate throught each vertex and assign the evaluated noise map value. 
        for (int i = 0, z = -halfHeight; z < halfHeight; z++)
        {
            for (int x = -halfWidth; x < halfWidth; x++)
            {
                vertices[i++] = new Vector3(x, meshCurve.Evaluate(noiseMap[z + halfHeight, x + halfWidth]) * heightMultiplier, z);
            }
        }
        
        return vertices; 
    }

    // map uv values for mesh, used to apply textures to mesh 
    private static Vector2[] MapUvs(Vector3[] vertices, int width, int height)
    {
        Vector2[] uvs = new Vector2[vertices.Length];

        float minValX = float.MaxValue;
        float maxValX = float.MinValue;
        float minValZ = float.MaxValue;
        float maxValZ = float.MinValue;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].x > maxValX)
                maxValX = vertices[i].x;
            else if (vertices[i].x < minValX)
                minValX = vertices[i].x;

            if (vertices[i].z > maxValZ)
                maxValZ = vertices[i].z;
            else if (vertices[i].z < minValZ)
                minValZ = vertices[i].z;
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2((vertices[i].x - minValX) / (maxValX - minValX), (vertices[i].z - minValZ) / (maxValZ - minValZ));
        }

        return uvs; 
    }

    // creates triangles between each 3 vertex's 
    private static int[] CreateTriangles(Vector3[] vertices, int width, int height)
    {
        int triangleIndex = 0; 
        int[] triangles = new int[width * height * 6];

        for (int x = 0; x < width - 1; x++)
        {
            for (int z = 0; z < height - 1; z++)
            {
                int currentVertex = x * width + z;

                // draw triangles clockwise 
                triangles[triangleIndex] = currentVertex + 0;
                triangles[triangleIndex + 1] = currentVertex + width + 0;
                triangles[triangleIndex + 2] = currentVertex + 1;
                
                triangles[triangleIndex + 3] = currentVertex + 1;
                triangles[triangleIndex + 4] = currentVertex + width + 0;
                triangles[triangleIndex + 5] = currentVertex + width + 1;
                
                triangleIndex += 6; 
            }
        }

        return triangles;
    }
}

// mesh tile store data about each mesh, such as vertices, triangles, uvs and the prefabs that should go ontop using poisson disk sampling
public class MeshTile 
{
    public List<GameObject> prefabsLayers;

    public Vector3[] vertices;
    Vector2[] uvs;
    int[] triangles;

    public MeshTile() { prefabsLayers = new List<GameObject>(); }

    public void UpdateMesh(Vector3[] vertices, Vector2[] uvs, int[] triangles)
    {
        this.vertices = vertices;
        this.uvs = uvs;
        this.triangles = triangles;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh(); 
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        return mesh; 
    }
}

