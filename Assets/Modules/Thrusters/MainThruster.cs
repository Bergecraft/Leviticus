using UnityEngine;
using System.Collections;

public class MainThruster : MonoBehaviour {
    public float THRUST = 5000;
    private ParticleSystem[] exhausts;
	// Use this for initialization
	void Start () {
        exhausts = transform.GetComponentsInChildren<ParticleSystem>();
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
            SetEmissionRate(200);
        }
    }
    private void SetEmissionRate(int emissionRate)
    {
        foreach (var exhaust in exhausts)
        {
            exhaust.emissionRate = emissionRate;
        }
    }
    public void Deactivate()
    {
        on = false;
        SetEmissionRate(0);
    }
    public Vector2 ThrustVector
    {
        get{
            if (on)
            {
                return transform.up * THRUST;
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
