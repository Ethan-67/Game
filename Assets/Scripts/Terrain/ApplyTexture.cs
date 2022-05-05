using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* APPLY TEXTURE 
 * Used to apply textures such as noise map to plane, colour map to plane and the colour map to the 3d mesh 
 * Source https://github.com/SebLague/Procedural-Landmass-Generation/blob/master/Proc%20Gen%20E13/Assets/Scripts/TextureGenerator.cs
*/
public static class ApplyTexture
{
    // applies noise map to plane 
    public static void ApplyNoiseMap(float [,] noiseMap, int width, int height)
    {
        // get plane 
        GameObject noisePlane = GameObject.Find("NoiseTest");

        // set scale 
        noisePlane.transform.localScale = new Vector3(width, 1, height); 

        // get renderer and initalise new texture 
        Renderer renderer = noisePlane.GetComponent<Renderer>(); 
        Texture2D texture = new Texture2D(width, height); 

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // get colour from noise map 
                float noiseColor = noiseMap[x, y];
                // create colour using grey scale value from noise map and set texture pixel of this 
                Color c = new Color(noiseColor, noiseColor, noiseColor);
                texture.SetPixel(x, y, c);
            }
        }
        // apply texture and set plane texture to the noise map 
        texture.Apply();
        renderer.sharedMaterial.mainTexture = texture;
    }

    // applies colour map to plane 
    public static void ApplyColorMap(GameObject obj, float[,] noiseMap, Region[] regions, int width, int height)
    {
        // get colour map using nosie map 
        Color[] colorMap = CreateColorMapFromNoiseMap(noiseMap, regions, width, height);

        // get renderer and set texutre of plane to conversion of noise map to colour map 
        Renderer renderer = obj.GetComponent<Renderer>();
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap); 
        texture.Apply();
        
        renderer.sharedMaterial.mainTexture = texture; 
    }

    // converts colour map into a noise map 
    public static Color[] CreateColorMapFromNoiseMap(float[,] noiseMap, Region[] regions, int width, int height)
    {
        // 2D to 1D 
        Color[] colorMap = new Color[width * height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                foreach (Region region in regions)
                {
                    // if noise value falls in range assign colour of the Region 
                    if (noiseMap[x, y] <= region.range)
                    {
                        colorMap[x * width + y] = region.color;
                        break;
                    }
                }
            }
        }
        return colorMap; 
    }


    // applies the colour map to 3d mesh 
    public static void ApplyColorMapToMesh(GameObject obj, float[,] noiseMap, Region[] regions, int width, int height)
    {
        // create colour map from noise map 
        Color[] colorMap = CreateColorMapFromNoiseMap(noiseMap, regions, width, height);


        // get renderer of mesh and set texture to the colour map
        Renderer renderer = obj.GetComponent<Renderer>();
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        
        renderer.sharedMaterial.mainTexture = texture;
        renderer.sharedMaterial.mainTextureScale = new Vector2(1, 1);
    }
}

