using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public float minZoom = 3;
    public float maxZoom = 30;
    public float zoomSpeed = 3f;
    public float panSpeed = 20;
	// Use this for initialization
    public Transform target;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var camera = transform.GetComponent<Camera>();
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed,minZoom, maxZoom);


        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, - 10), panSpeed * Time.deltaTime);
	}
}
