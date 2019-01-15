using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {

    private Voxel[,,] voxels;

    private World world;

    private int chunkX, chunkY, chunkZ;
    private int faceCount;

    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Vector2> uvs;

    private Mesh mesh;

    private Vector2[] textures = { new Vector2(0, 2), new Vector2(0, 1), new Vector2(1, 1),
                                   new Vector2(3, 1), new Vector2(0, 0), new Vector2(1, 0),
                                   new Vector2(3, 0)};

    private Vector2 grassSide = new Vector2(2, 1);
    private Vector2 snowSide = new Vector2(2, 0);

    private float tileOffset = 0.25f;

    void Start ()
    {
        this.world = GameObject.Find("Terrain").GetComponent<World>();

        this.faceCount = 0;

        this.vertices = new List<Vector3>();
        this.triangles = new List<int>();
        this.uvs = new List<Vector2>();

        this.mesh = GetComponent<MeshFilter>().mesh;

        this.GenerateChunk();
    } 

    private Voxel getVoxel(int x, int y, int z)
    {
        return this.world.GetVoxel(x + this.chunkX, y + this.chunkY, z + this.chunkZ);
    }

    private void generateVoxel(Voxel voxel, int x, int y, int z)
    {
        Vector2 texture = this.textures[voxel.GetTerrainType()];

        //Top       
        if (voxel.GetTopNeighbor() == null || (voxel.GetTopNeighbor() != null && !voxel.GetTopNeighbor().IsActive()))
        {
            this.generateFace(new Vector3(x, y + 1, z),
                          new Vector3(x + 1, y + 1, z),
                          new Vector3(x, y + 1, z + 1),
                          new Vector3(x + 1, y + 1, z + 1),
                          texture);
        }

        if (voxel.GetTerrainType() == TerrainType.GRASS && voxel.GetBottomNeighbor().GetTerrainType() == TerrainType.DIRT)
        {
            texture = this.grassSide;
        }


        if (voxel.GetTerrainType() == TerrainType.SNOW && voxel.GetBottomNeighbor().GetTerrainType() == TerrainType.MOUNTAIN)
        {
            texture = this.snowSide;
        }

        //Front
        if (voxel.GetFrontNeighbor() == null || (voxel.GetFrontNeighbor() != null && !voxel.GetFrontNeighbor().IsActive()))
        {            
            this.generateFace(new Vector3(x, y, z),
                          new Vector3(x + 1, y, z),
                          new Vector3(x, y + 1, z),
                          new Vector3(x + 1, y + 1, z),
                          texture);
        }

        //Back
        if (voxel.GetBackNeighbor() == null || (voxel.GetBackNeighbor() != null && !voxel.GetBackNeighbor().IsActive()))
        {
            this.generateFace(new Vector3(x + 1, y, z + 1),
                          new Vector3(x, y, z + 1),
                          new Vector3(x + 1, y + 1, z + 1),
                          new Vector3(x, y + 1, z + 1),
                          texture);
        }

        //Left
        if (voxel.GetLeftNeighbor() == null || (voxel.GetLeftNeighbor() != null && !voxel.GetLeftNeighbor().IsActive()))
        {
            this.generateFace(new Vector3(x, y, z + 1),
                          new Vector3(x, y, z),
                          new Vector3(x, y + 1, z + 1),
                          new Vector3(x, y + 1, z),
                          texture);
        }

        //Right
        if (voxel.GetRightNeighbor() == null || (voxel.GetRightNeighbor() != null && !voxel.GetRightNeighbor().IsActive()))
        {
            this.generateFace(new Vector3(x + 1, y, z),
                          new Vector3(x + 1, y, z + 1),
                          new Vector3(x + 1, y + 1, z),
                          new Vector3(x + 1, y + 1, z + 1),
                          texture);
        }

        //Bottom
        if (voxel.GetBottomNeighbor() == null || (voxel.GetBottomNeighbor() != null && !voxel.GetBottomNeighbor().IsActive()))
        {
            this.generateFace(new Vector3(x, y, z + 1),
                          new Vector3(x + 1, y, z + 1),
                          new Vector3(x, y, z),
                          new Vector3(x + 1, y, z),
                          texture);
        }
    }

    private void generateFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector2 texture)
    {
        this.vertices.Add(v1);
        this.vertices.Add(v2);
        this.vertices.Add(v3);
        this.vertices.Add(v4);

        this.triangles.Add(this.faceCount * 4);
        this.triangles.Add(this.faceCount * 4 + 2);
        this.triangles.Add(this.faceCount * 4 + 1);
        this.triangles.Add(this.faceCount * 4 + 2);
        this.triangles.Add(this.faceCount * 4 + 3);
        this.triangles.Add(this.faceCount * 4 + 1);

        this.uvs.Add(new Vector2(texture.x * this.tileOffset, texture.y * this.tileOffset));
        this.uvs.Add(new Vector2(texture.x * this.tileOffset + this.tileOffset, texture.y * this.tileOffset));
        this.uvs.Add(new Vector2(texture.x * this.tileOffset, texture.y * this.tileOffset + this.tileOffset));
        this.uvs.Add(new Vector2(texture.x * this.tileOffset + this.tileOffset, texture.y * this.tileOffset + this.tileOffset));

        this.faceCount++;
    }

    private void updateMesh()
    {
        this.mesh.Clear();

        mesh.vertices = this.vertices.ToArray();
        mesh.triangles = this.triangles.ToArray();
        mesh.uv = this.uvs.ToArray();

        ;
        mesh.RecalculateNormals();

        this.vertices.Clear();
        this.triangles.Clear();
        this.uvs.Clear();
    }

    public void GenerateChunk()
    {
        this.voxels = new Voxel[World.CHUNK_SIZE, World.CHUNK_SIZE, World.CHUNK_SIZE];

        for (int i = 0; i < voxels.GetLength(0); i++)
        {
            for (int j = 0; j < voxels.GetLength(1); j++)
            {
                for (int k = 0; k < voxels.GetLength(2); k++)
                {
                    Voxel voxel = this.getVoxel(i, j, k);

                    if (voxel != null && voxel.IsActive())
                    {
                        this.generateVoxel(voxel, i, j, k);

                        if (voxel.HasTree())
                        {
                            GameObject tree = GameObject.Instantiate(Resources.Load("Prefabs/Tree"), voxel.GetPosition() + new Vector3(0, 2, 0), Quaternion.identity) as GameObject;

                            tree.transform.SetParent(this.gameObject.transform);
                        }
                    }
                }
            }
        }

        this.updateMesh();
    }

    public void SetPosition(int x, int y, int z)
    {
        this.chunkX = x;
        this.chunkY = y;
        this.chunkZ = z;
    }
}
