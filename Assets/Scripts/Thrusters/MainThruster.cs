using UnityEngine;
using System.Collections;

public class MainThruster : MonoBehaviour {
    public float THRUST = 5000;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private bool on;
    public void Activate()
    {
        if (gameObject.activeInHierarchy)
        {
            on = true;
            transform.GetComponent<ParticleSystem>().emissionRate = 200;
        }
    }
    public void Deactivate()
    {
        on = false;
        transform.GetComponent<ParticleSystem>().emissionRate = 0;
    }
    public Vector2 ThrustVector
    {
        get{
            if (on)
            {
                return -transform.forward * THRUST;
            }
            return Vector2.zero;
        }
    }
    public Vector2 position2d
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.y);
        }
    }
}
