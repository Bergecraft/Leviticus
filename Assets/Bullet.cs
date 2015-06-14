using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private float createTime;
    private float ARM_TIME = 1.0f;
    private float MAX_AGE = 10;
    //public Vector3 velocity;
	// Use this for initialization
	void Start () {
        createTime = Time.time;
	}
	
	// Update is called once per frame
    void FixedUpdate () {
        if (Time.time - createTime > MAX_AGE)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (Time.time - createTime > ARM_TIME)
        //{
            Destroy(this.gameObject);
        //}
    }
}
