using UnityEngine;
using System.Collections;
using Assets.Modules;
using System.Linq;
using Assets.Modules.Weapons;

public class Ammo : MonoBehaviour {

    //public Vector3 velocity;
	// Use this for initialization
    public AmmoDef def;
    public SpacecraftController source;
	void OnEnable () {
        Destroy(this.gameObject, def.lifetime);
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        DetectCollision(other.collider);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        DetectCollision(other);
    }
    void DetectCollision(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            Physics2D.IgnoreCollision(other, this.GetComponent<Collider2D>());
        }

        var otherShip = other.gameObject.GetComponent<SpacecraftController>();
        if (otherShip != null)
        {
            if (otherShip == source)
            {
                Physics2D.IgnoreCollision(other, this.GetComponent<Collider2D>());
            }
            else
            {
                otherShip.Damage(def.bonusDamage + getRelativeVelocity(other.GetComponent<Rigidbody2D>()).magnitude * def.damageVelocityScalar);
                Destroy(this.gameObject);
            }
        }
    }
    Vector2 getRelativeVelocity(Rigidbody2D other)
    {
        return other.velocity - this.GetComponent<Rigidbody2D>().velocity;
    }
}