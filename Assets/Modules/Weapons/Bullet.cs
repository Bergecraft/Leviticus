using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float ARM_TIME = 1.0f;
    public float MAX_AGE = 5;
    //public Vector3 velocity;
	// Use this for initialization
    public SpacecraftController source;
	void Start () {
        Destroy(this.gameObject, MAX_AGE);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (Time.time - createTime > ARM_TIME)
        //{
        var otherShip = other.gameObject.GetComponent<SpacecraftController>();
        if (otherShip!=source && otherShip != null)
        {
            otherShip.Damage(1);
            Destroy(this.gameObject);
        }
        //}
    }
}
