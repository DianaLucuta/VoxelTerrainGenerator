using UnityEngine;
using System.Collections;

public class EnvironmentControls : MonoBehaviour {

	private float minFieldOfView = 10f;

	private float maxFieldOfView = 500f;

	private float sensitivity = 10f;

	void Update () 
	{
		//this.updateFieldOfView ();
	}

	private void updateFieldOfView ()
	{
		float fieldOfView = Camera.main.fieldOfView;

		fieldOfView -= Input.GetAxis ("Mouse ScrollWheel") * this.sensitivity;

		fieldOfView = Mathf.Clamp (fieldOfView, this.minFieldOfView, this.maxFieldOfView);

		Camera.main.fieldOfView = fieldOfView;
	}
}
