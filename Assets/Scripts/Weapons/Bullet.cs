using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float ARM_TIME = 1.0f;
    public float MAX_AGE = 5;
    //public Vector3 velocity;
	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, MAX_AGE);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (Time.time - createTime > ARM_TIME)
        //{
        if (other.gameObject.GetComponent<SpacecraftController>() != null)
        {
            other.gameObject.GetComponent<SpacecraftController>().Damage(1);
            Destroy(this.gameObject);
        }
        //}
    }
}
