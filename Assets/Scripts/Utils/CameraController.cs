using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private GameObject player;

    private float sensitivity = 5.0f;
    private float smoothing = 2.0f;

    private Vector2 mouseLook;
    private Vector2 smoothV;

	void Start ()
    {
        this.player = this.transform.parent.gameObject;
	}

	void Update ()
    {
        Vector2 delta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        delta = Vector2.Scale(delta, new Vector2(this.sensitivity * this.smoothing, this.sensitivity * this.smoothing));

        this.smoothV.x = Mathf.Lerp(this.smoothV.x, delta.x, 1.0f / this.smoothing);
        this.smoothV.y = Mathf.Lerp(this.smoothV.y, delta.y, 1.0f / this.smoothing);

        this.mouseLook += this.smoothV;

        this.transform.localRotation = Quaternion.AngleAxis(-this.mouseLook.y, Vector3.right);

        this.player.transform.localRotation = Quaternion.AngleAxis(this.mouseLook.x, this.player.transform.up);
    }
}
