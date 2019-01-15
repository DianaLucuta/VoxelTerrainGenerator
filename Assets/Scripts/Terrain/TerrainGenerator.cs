using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {

	private PerlinNoise perlinNoise;

    private byte[,,] terrainData;

    private Voxel[,,] voxelData;

    private void generateTerrainData()
    {
        this.perlinNoise = new PerlinNoise();

        this.terrainData = new byte[World.WIDTH, World.HEIGHT, World.DEPTH];

        float[][] perlinNoiseArray = new float[World.WIDTH][];

        perlinNoiseArray = this.perlinNoise.GeneratePerlinNoise(World.WIDTH, World.DEPTH);

        for (int i = 0; i < this.terrainData.GetLength(0); i++)
        {
            for (int k = 0; k < this.terrainData.GetLength(2); k++)
            {
                int columnHeight = (int)(World.HEIGHT * Mathf.Clamp01(perlinNoiseArray[i][k]));

                for (int j = 0; j < this.terrainData.GetLength(1); j++)
                {
                    float altitude = (float)j / (float)this.terrainData.GetLength(1);

                    if (altitude <= BiomeConstants.WATER || j <= columnHeight)
                    {
                        this.terrainData[i, j, k] = 1;
                    }
                    else
                    {
                        this.terrainData[i, j, k] = 0;
                    }
                }
            }
        }
    }

    private void generateVoxelData()
    {
        this.voxelData = new Voxel[World.WIDTH, World.HEIGHT, World.DEPTH];

        for (int i = 0; i < this.terrainData.GetLength(0); i++)
        {
            for (int j = 0; j < this.terrainData.GetLength(1); j++)
            {
                for (int k = 0; k < this.terrainData.GetLength(2); k++)
                {
                    Voxel voxel = new Voxel();

                    if (this.terrainData[i,j,k] == 0)
                    {
                        voxel.SetActive(false);
                    }
                    else
                    {
                        float altitude = (float)j / (float)this.terrainData.GetLength(1);

                        this.assignTerrainType(voxel, altitude);
                    }

                    voxel.SetPosition(i, j, k);

                    this.voxelData[i, j, k] = voxel;
                }
            }
        }

        for (int i = 0; i < this.voxelData.GetLength(0); i++)
        {
            for (int j = 0; j < this.voxelData.GetLength(1); j++)
            {
                for (int k = 0; k < this.voxelData.GetLength(2); k++)
                {
                    this.assignNeighbors(this.voxelData[i, j, k]);
                }
            }
        }

        this.assignTrees();
    }

    private void assignNeighbors(Voxel voxel)
    {
        int x = voxel.GetX();
        int y = voxel.GetY();
        int z = voxel.GetZ();

        if (y + 1 < World.HEIGHT)
        {
            Voxel top = this.voxelData[x, y + 1, z];

            voxel.SetTopNeighbor(top);
        }

        if (y - 1 >= 0)
        {
            Voxel bottom = this.voxelData[x, y - 1, z];

            voxel.SetBottomNeighbor(bottom);
        }

        if (x - 1 >= 0)
        {
            Voxel left = this.voxelData[x - 1, y, z];

            voxel.SetLeftNeighbor(left);
        }

        if (x + 1 < World.WIDTH)
        {
            Voxel right = this.voxelData[x + 1, y, z];

            voxel.SetRightNeighbor(right);
        }

        if (z - 1 >= 0)
        {
            Voxel front = this.voxelData[x, y, z - 1];

            voxel.SetFrontNeighbor(front);
        }

        if (z + 1 < World.DEPTH)
        {
            Voxel back = this.voxelData[x, y, z + 1];

            voxel.SetBackNeighbor(back);
        }
    }

    private void assignTerrainType(Voxel voxel, float altitude)
    {
        if (altitude <= BiomeConstants.WATER)
        {
            voxel.SetTerrainType(TerrainType.WATER);
        }
        else if (altitude <= BiomeConstants.SAND)
        {
            voxel.SetTerrainType(TerrainType.SAND);
        }
        else if (altitude <= BiomeConstants.DIRT)
        {
            voxel.SetTerrainType(TerrainType.DIRT);
        }
        else if (altitude <= BiomeConstants.GRASS)
        {
            voxel.SetTerrainType(TerrainType.GRASS);
        }
        else if (altitude <= BiomeConstants.FOREST)
        {
            voxel.SetTerrainType(TerrainType.FOREST);
        }
        else if (altitude <= BiomeConstants.MOUNTAIN)
        {
            voxel.SetTerrainType(TerrainType.MOUNTAIN);
        }
        else if (altitude <= BiomeConstants.SNOW)
        {
            voxel.SetTerrainType(TerrainType.SNOW);
        }
    }

    private void assignTrees ()
    {
        PoissonDisk poisson = new PoissonDisk();

        Vector2[,] treeSamples = poisson.GenerateSamples();

        for (int i = 0; i < treeSamples.GetLength(0); i++)
        {
            for (int j = 0; j < treeSamples.GetLength(1); j++)
            {
                if (treeSamples[i, j] != Vector2.zero) 
                {
                    int x = (int)treeSamples[i, j].x;
                    int z = (int)treeSamples[i, j].y;

                    int height = this.getHeightForCoords(x, z);

                    Voxel voxel = this.voxelData[x, height, z];

                    voxel.SetTree();
                }
            }
        }
    }

    private int getHeightForCoords (int x, int z)
    {
        for (int y = 0; y < World.HEIGHT; y++) 
        {
            if (!this.voxelData[x,y,z].IsActive())
            {
                return y - 1;
            }
        }

        return 0;
    }

    public void GenerateTerrain()
    {
        this.generateTerrainData();

        this.generateVoxelData();
    }

    public Voxel[,,] GetVoxelData ()
    {
        return this.voxelData;
    }
}
