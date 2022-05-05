using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* NOISE 
 * Generates a noise map to be used by map generator
 * SOURCE https://github.com/SebLague/Procedural-Landmass-Generation/blob/master/Proc%20Gen%20E13/Assets/Scripts/Noise.cs
*/
public static class Noise
{
    
    // generates a noise map by calling Mathf.perlin and inputs these values into a 2d which is returned 
    public static float[,] GenerateNoiseMap(int seed, int width, int height, float scale, Vector2 offset, int octaves, float frequency, float lacunarity, float persistance, float amplitude)
    {
        float[,] noiseMap = new float[width, height];

        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        if (seed == 0)
            seed = 1;
        if (scale == 0)
            scale = 0.001f;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        float tempFrequency = frequency;
        float tempAmplitude = amplitude;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xf = (((((float)x) - width / 2) / width * scale) + seed * 100) + offset.x;
                float yf = (((((float)y) - height / 2) / height * scale) - seed * 100) + offset.y;

                float noiseHeight = Mathf.PerlinNoise(xf, yf);
                amplitude = tempAmplitude;
                frequency = tempFrequency;

                /*
                For Extension
                for (int i = 0; i < octaves; i++)
                {
                    noiseHeight += Mathf.PerlinNoise(xf, yf);
                    frequency *= lacunarity;
                    amplitude *= persistance;
                }
                */
                

                noiseMap[x, y] = noiseHeight;

                // for octave perlin 
                if (noiseHeight > maxHeight)
                    maxHeight = noiseHeight;
                else if (noiseHeight < minHeight)
                    minHeight = noiseHeight;
            }
        }


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                noiseMap[x, y] = noiseMap[x, y] - minHeight / maxHeight - minHeight; 
            }
        }
        

        return noiseMap; 
    }

    public static float[,] GenerateBlankMap(int width, int height)
    {
        float[,] noiseMap = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                noiseMap[x, y] = 0; 
            }
        }

        return noiseMap; 
    }


}

