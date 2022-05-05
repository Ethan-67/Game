using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* POISSON DISK SAMPLING 
Step 0. Initialize an n-dimensional background grid for storing
samples and accelerating spatial searches. We pick the cell size to
be bounded by r/sqrt(n), so that each grid cell will contain at most
one sample, and thus the grid can be implemented as a simple n-
dimensional array of integers: the default ?1 indicates no sample, a
non-negative integer gives the index of the sample located in a cell.

Step 1. Select the initial sample, x0, randomly chosen uniformly
from the domain. Insert it into the background grid, and initialize
the “active list” (an array of sample indices) with this index (zero).

Step 2. While the active list is not empty, choose a random index
from it (say i). Generate up to k points chosen uniformly from the
spherical annulus between radius r and 2r around xi. For each
point in turn, check if it is within distance r of existing samples
(using the background grid to only test nearby samples). If a point
is adequately far from existing samples, emit it as the next sample
and add it to the active list. If after k attempts no such point is
found, instead remove i from the active list

* Source https://doi.org/https://doi.org/10.1145/1278780.1278807
*/


public static class PoissonDiskSampling
{
    public static Vector2[] SamplePoints(int size, int sampleSize, float radius)
    {
        // size of each cell size bounded by r/sqrt(n) (r radius, n dimensions) 
        float cellSize = radius / Mathf.Sqrt(2);

        // init n-dimensional background grid for storing samples and accerlating spatial searches
        Vector2[,] backgroundGrid = new Vector2[Mathf.CeilToInt(size / cellSize), Mathf.CeilToInt(size / cellSize)];
        
        // active list for storing potential samples 
        List<Vector2> activeList = new List<Vector2>();

        // the default ?1 indicates no sample, a non - negative integer gives the index of the sample located in a cell.
        InitialiseGrid(backgroundGrid);

        // Select the initial sample, x0, randomly chosen uniformly from the domain
        Vector2 firstPoint = ChooseRandomPointOnGrid(size);

        // Insert it into the background grid, and initialize the “active list” (an array of sample indices) with this index(zero).
        InputVectorToGridSquare(backgroundGrid, firstPoint, cellSize);
        activeList.Add(firstPoint);

        bool sampleFound = false;
        // While the active list is not empty, choose a random index from it (say i).
        int count = 0; 
        while (activeList.Count != 0)
        {
            Vector2 randomPoint = activeList[GetRandomIndex(activeList)];
            if (sampleFound)
                randomPoint = activeList[activeList.Count - 1];

            //Generate up to k points chosen uniformly from the spherical annulus between radius r and 2r around xi.
            Vector2[] kPoints = GenerateKPointsAroundPoint(randomPoint, size, sampleSize, radius);

            sampleFound = false;
            // For each point in turn, check if it is within distance r of existing samples
            foreach (Vector2 kPoint in kPoints)
            {
                // If a point is adequately far from existing samples, emit it as the next sample and add it to the active list.
                if (IsKpointTooCloseToExisting(backgroundGrid, kPoint, radius, cellSize, size))
                {
                    continue;
                }
                sampleFound = true;
                activeList.Add(kPoint);
                InputVectorToGridSquare(backgroundGrid, kPoint, cellSize);
                break;
            }

            // If after k attempts no such point is found, instead remove i from the active list
            if (!sampleFound) 
                activeList.Remove(randomPoint);

            if (count > 4000)
            {
                Debug.Log("ERROR poisson disk sample overflow");
                break;
            }
            count++; 
        }

        return Convert2DVectorArrayInto1D(backgroundGrid, size);
    }

    private static bool IsKpointTooCloseToExisting(Vector2[,] backgroundGrid, Vector2 kPoint, float radius, float cellSize, int size)
    {
        // due to how the cell size was established we only need to check 1 square around of the point square
        int xOrigin = GetIndexOfGridSquareRelativeGlobalVector(kPoint.x, cellSize);
        int yOrigin = GetIndexOfGridSquareRelativeGlobalVector(kPoint.y, cellSize);

        // check point is viable (in bounds of grid) 
        if (kPoint.x < 0 || kPoint.x > size || kPoint.y < 0 || kPoint.y > size)
            return true; 

        for (int xSquare = -1; xSquare <= 1; xSquare++)
        {
            for (int ySquare = -1; ySquare <= 1; ySquare++)
            {
                try
                {
                    if (Vector2.Distance(backgroundGrid[xOrigin + xSquare, yOrigin + ySquare], kPoint) < radius)
                    {
                        return true; 
                    }
                }
                catch (System.Exception)
                {
                    //Debug.Log("OutOfBoundsException, xCoord: " + xOrigin + xSquare + " yCoord: " + yOrigin + ySquare);
                    continue;
                }
            }
        }
        return false;
    }

    // randomly choose distance of a Kpoint, uniformly angled 
    private static Vector2[] GenerateKPointsAroundPoint(Vector2 sourcePoint, int size, int sampleSize, float radius)
    {
        // Generate up to k points chosen uniformly from the spherical annulus between radius r and 2r around xi.
        Vector2[] kPoints = new Vector2[sampleSize];

        // angle of a circle / sample size, to recieve a uniform angle to try each point around source
        float uniformAngleIncrement = 360f / (float) sampleSize;

        for (int i = 0; i < sampleSize; i++)
        {
            float distance = ChooseRandomDistanceInAnnulus(radius);

            //Vector2 inputCoord = ConvertLocalToGlobalVector(sourcePoint, new Vector2(xCoord, yCoord));

            
            //float distance = Vector2.Distance(sourcePoint, inputCoord);

            // create point using the distances collected and the uniform angle increment to map a new coordinate
            Vector2 kPoint = new Vector2(sourcePoint.x + Mathf.Cos(uniformAngleIncrement * i) * distance,
                sourcePoint.y + Mathf.Sin(uniformAngleIncrement * i) * distance);

            kPoints[i] = kPoint; 
        }
        return kPoints; 
    }

    private static void InputVectorToGridSquare(Vector2[,] grid, Vector2 vector, float cellSize)
    {
        int xIndex = GetIndexOfGridSquareRelativeGlobalVector(vector.x, cellSize); 
        int yIndex = GetIndexOfGridSquareRelativeGlobalVector(vector.y, cellSize);

        grid[xIndex, yIndex] = vector;
    }

    // chooses a random distance between r - 2r, returns either positive or negaive floating point in range
    private static float ChooseRandomDistanceInAnnulus(float radius)
    {
        float value = Random.Range(radius, 2 * radius);
        int positiveOrNegative = Random.Range(0, 2);

        return positiveOrNegative == 1 ? value : -value; 
    }

    private static int GetRandomIndex(List<Vector2> list)
    {
        return Random.Range(0, list.Count); 
    }

    private static Vector2 ChooseRandomPointOnGrid(int size)
    {
        float randomX = Random.Range(0f, size);
        float randomY = Random.Range(0f, size);

        return new Vector2(randomX, randomY); 
    }

    private static void InitialiseGrid(Vector2[,] grid)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                try
                {
                    grid[x, y] = new Vector2(-1, -1);
                }
                catch (System.Exception)
                {
                    //Debug.Log("IndexOutOfBounds" + "| x: " + x + "|   y: " + y + " |");
                    continue; 
                }
            }
        }
    }

    private static int GetIndexOfGridSquareRelativeGlobalVector(float value, float cellSize)
    {
        int index = 0;
        float vectorValue = 0; 
        while(vectorValue + cellSize < value)
        {
            index++;
            vectorValue += cellSize; 
        }
        return index;
    }

    private static Vector2[] Convert2DVectorArrayInto1D(Vector2[,] grid, int size)
    {
        int halfSize = size / 2;

        int actualSize = 0; 
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] != new Vector2(-1, -1))
                    actualSize++;
            }
        }

        Vector2[] output = new Vector2[actualSize];
        for (int i = 0, x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] != new Vector2(-1, -1))
                {
                    output[i++] = new Vector2(grid[x, y].x, grid[x, y].y);
                }
            }
        }

        return output;
    }
}
