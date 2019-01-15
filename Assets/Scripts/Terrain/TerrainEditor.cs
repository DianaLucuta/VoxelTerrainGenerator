using UnityEngine;
using System.Collections;

public class TerrainEditor : MonoBehaviour {

    private World world;

    private GameObject cameraObj;

	void Start ()
    {
        this.world = GameObject.Find("Terrain").GetComponent<World>();

        this.cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void setVoxelActive(int x, int y, int z, bool isActive)
    {
        this.world.GetWorldData()[x, y, z].SetActive(isActive);

        this.updateChunk(x, y, z);
    }

    private void setVoxelActiveAtPoint(RaycastHit hit, bool isActive)
    {
        Vector3 position = hit.point;

        position += hit.normal * 0.5f;

        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        int z = Mathf.RoundToInt(position.z);

        this.setVoxelActive(x, y, z, isActive);
    }

    private void updateChunk(int x, int y, int z)
    {
        x = Mathf.FloorToInt(x / World.CHUNK_SIZE);
        y = Mathf.FloorToInt(y / World.CHUNK_SIZE);
        z = Mathf.FloorToInt(z / World.CHUNK_SIZE);

        this.world.GetChunks()[x, y, z].GenerateChunk();
    }

    public void AddVoxel ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            this.setVoxelActiveAtPoint(hit, true);
        }
    }

    public void RemoveVoxel ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            this.setVoxelActiveAtPoint(hit, false);
        }
    }
}
