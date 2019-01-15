using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

    public const int WIDTH = 512;
    public const int HEIGHT = 32;
    public const int DEPTH = 512;
    public const int CHUNK_SIZE = 32;

    public const float LOAD_DISTANCE = 50.0f;

    private Voxel[,,] worldData;

    private Chunk[,,] chunks;

    private TerrainGenerator terrainGenerator;

    void Start ()
    {
        int chunkAmountX = Mathf.FloorToInt(World.WIDTH / World.CHUNK_SIZE);
        int chunkAmountY = Mathf.FloorToInt(World.HEIGHT / World.CHUNK_SIZE);
        int chunkAmountZ = Mathf.FloorToInt(World.DEPTH / World.CHUNK_SIZE);

        this.chunks = new Chunk[chunkAmountX, chunkAmountY, chunkAmountZ];

        this.generateWorldData();

        this.InvokeRepeating("loadUnloadTerrain", 0.0f, 2.0f);
    }

    private void generateWorldData()
    {
        this.terrainGenerator = GameObject.Find("Terrain").GetComponent<TerrainGenerator>();

        this.terrainGenerator.GenerateTerrain();

        this.worldData = this.terrainGenerator.GetVoxelData();
    }

    private void generateChunk(int x, int z)
    {
        for (int y = 0; y < this.chunks.GetLength(1); y++)
        {
            int newX = x * World.CHUNK_SIZE;
            int newY = y * World.CHUNK_SIZE;
            int newZ = z * World.CHUNK_SIZE;

            GameObject chunkObj = GameObject.Instantiate(Resources.Load("Prefabs/Chunk"), new Vector3(newX - 0.5f, newY + 0.5f, newZ - 0.5f), Quaternion.identity) as GameObject;

            Chunk chunk = chunkObj.GetComponent<Chunk>();

            chunk.SetPosition(newX, newY, newZ);

            this.chunks[x, y, z] = chunk;
        }
    }

    private void deleteChunk(int x, int z)
    {
        for (int y = 0; y < this.chunks.GetLength(1); y++)
        {
            Object.Destroy(this.chunks[x, y, z].gameObject);
        }
    }

    private void loadUnloadTerrain()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        for (int x = 0; x < this.chunks.GetLength(0); x++)
        {
            for (int z = 0; z < this.chunks.GetLength(2); z++)
            {
                float distance = Vector2.Distance(new Vector2(playerPos.x, playerPos.z), 
                                                  new Vector2(x * World.CHUNK_SIZE, z * World.CHUNK_SIZE));
                
                if (distance < World.LOAD_DISTANCE)
                {
                    if (this.chunks[x,0,z] == null)
                    {
                        this.generateChunk(x, z);
                    }
                }
                else
                {
                    if (this.chunks[x,0,z] != null)
                    {
                        this.deleteChunk(x, z);
                    }
                }
            } 
        }
    }

    public Voxel GetVoxel(int x, int y, int z)
    {
        if (this.worldData != null && this.worldData[x, y, z] != null)
        {
            return worldData[x, y, z];
        }
        else
        {
            return null;
        }
    }

    public Voxel [,,] GetWorldData ()
    {
        return this.worldData;
    }

    public Chunk[,,] GetChunks()
    {
        return this.chunks;
    }
}
