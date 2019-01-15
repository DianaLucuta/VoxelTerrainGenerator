using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoissonDisk {

    public static int SAMPLE_LIMIT = 30;

    public static int RADIUS = 5;

    private Vector2[,] backgroundGrid;

    private List<Vector2> activeSamples = new List<Vector2>();

    private float squaredRadius;

    public Vector2[,] GenerateSamples ()
    {
        // step 0
        this.squaredRadius = PoissonDisk.RADIUS * PoissonDisk.RADIUS;

        this.backgroundGrid = new Vector2[World.WIDTH, World.DEPTH];

        // step 1
        Vector2 randomSample = new Vector2(Random.Range(0, World.WIDTH),
                                           Random.Range(0, World.DEPTH));

        this.insertSampleIntoGrid(randomSample);
        
        // step 2
        while (this.activeSamples.Count > 0)
        {
            int randomIndex = Random.Range(0, this.activeSamples.Count);

            Vector2 sample = this.activeSamples[randomIndex];

            bool pointFound = false;

            for (int i = 0; i < PoissonDisk.SAMPLE_LIMIT; i++) 
            {
                Vector2 chosenPt = this.generateRandomPointAround(sample, PoissonDisk.RADIUS);

                if (this.isPointInBounds(chosenPt) && this.isInValidDistance(chosenPt))
                { 
                    pointFound = true;

                    this.insertSampleIntoGrid(chosenPt);

                    break;
                }
            }

            if (!pointFound)
            {
                this.activeSamples[randomIndex] = this.activeSamples[this.activeSamples.Count - 1];

                this.activeSamples.RemoveAt(this.activeSamples.Count - 1);
            }
        }

        return this.backgroundGrid;
    }

    private void insertSampleIntoGrid(Vector2 sample)
    {
        this.activeSamples.Add(sample);

        this.backgroundGrid[(int)sample.x, (int)sample.y] = sample;
    }

    private Vector2 generateRandomPointAround (Vector2 point, float minDist)
    {     
        float rad = minDist * (Random.value + 1);
     
        float angle = 2.0f * Mathf.PI * Random.value;

        float x = point.x + rad * Mathf.Cos(angle);
        float y = point.y + rad * Mathf.Sin(angle);

        return new Vector2(Mathf.Round(x), Mathf.Round(y));
    }

    private bool isPointInBounds (Vector2 point)
    {
        if (point.x >= 0 && point.x < World.WIDTH &&
            point.y >= 0 && point.y < World.DEPTH)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool isInValidDistance(Vector2 point)
    {
        int xMin = Mathf.Max((int)point.x - 2, 0);
        int xMax = Mathf.Min((int)point.x + 2, this.backgroundGrid.GetLength(0) - 1);

        int yMin = Mathf.Max((int)point.y - 2, 0);
        int yMax = Mathf.Min((int)point.y + 2, this.backgroundGrid.GetLength(1) - 1);

        for (int y = yMin; y <= yMax; y++)
        {
            for (int x = xMin; x <= xMax; x++)
            {
                Vector2 neighbor = this.backgroundGrid[x, y];

                if (neighbor != Vector2.zero)
                {
                    Vector2 dist = neighbor - point;

                    if ((dist.x * dist.x + dist.y * dist.y) < this.squaredRadius)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}
