using UnityEngine;
using System.Collections;

public class Blaster : MonoBehaviour {
    public float FIRE_RATE;
    public float MUZZLE_VELOCITY;

    public Bullet prefab;

    private float lastFire;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Fire()
    {
        if (Time.time - lastFire > FIRE_RATE)
        {
            lastFire = Time.time;
            var bullet = Instantiate(prefab);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody2D>().velocity = MUZZLE_VELOCITY * transform.up;
        }
        
    }
}
