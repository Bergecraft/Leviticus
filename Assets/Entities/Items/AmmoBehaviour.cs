using UnityEngine;
using System.Collections;
using Assets.Modules;
using System.Linq;
using Assets.Serialization;
using Assets.Entities.Items;
using Assets.Entities;
using Assets.Entities.Modules;

public class AmmoBehaviour : SpriteBehaviour<AmmoDef>
{
    void Awake()
    {

    }
    void Start()
    {
        //base.Start();
    }
    //public Vector3 velocity;
	// Use this for initialization
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
    public static GameObject BuildAmmo(SpacecraftController source, AmmoDef ammodef)
    {
        var ammoPrefab = new GameObject(ammodef.definitionType + "_Prefab");
        ammoPrefab.hideFlags = HideFlags.HideInHierarchy;
        ammoPrefab.SetActive(false);
        ammoPrefab.layer = 8;
        //Ammo Script
        var ammo = ammoPrefab.AddComponent<AmmoBehaviour>();
        ammo.def = ammodef;

        ammo.BuildSprite();

        ammo.source = source;

        var collider = ammoPrefab.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;
        //Physics2D.IgnoreCollision(collider, ammo.source.GetComponent<Collider2D>());
        Physics2D.IgnoreLayerCollision(8, 8);
        var rigidbody = ammoPrefab.AddComponent<Rigidbody2D>();
        rigidbody.mass = ammodef.mass;

        return ammoPrefab;
    }
}