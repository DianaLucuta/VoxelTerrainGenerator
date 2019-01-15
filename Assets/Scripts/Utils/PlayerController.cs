using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private float speed = 10.0f;

    private TerrainEditor terrainEditor;

    void Start ()
    {
        this.terrainEditor = this.GetComponent<TerrainEditor>();
    }
	
	void Update ()
    {
        float zMovement = Input.GetAxis("Vertical") * this.speed;
        float xMovement = Input.GetAxis("Horizontal") * this.speed;

        zMovement *= Time.deltaTime;
        xMovement *= Time.deltaTime;

        transform.Translate(xMovement, 0, zMovement);

        if (Input.GetMouseButtonDown(0))
        {
            this.terrainEditor.AddVoxel();
        }

        if (Input.GetMouseButtonDown(1))
        {
            this.terrainEditor.RemoveVoxel();
        }
    }
}
