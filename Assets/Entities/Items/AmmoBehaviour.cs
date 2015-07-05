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
        var sourceColor = source.GetComponent<SpriteRenderer>().color;
        var primaryColor = Primary(sourceColor);
        ammo.def.spriteColor = primaryColor;
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
    static Color Saturate(Color color, float saturation)
    {
        return Color.white/2 + color / saturation;


        //var L = 0.3f * color.r + 0.6f * color.g + 0.1f * color.b;
        //var new_r = color.r + saturation * (L - color.r);
        //var new_g = color.g + saturation * (L - color.g);
        //var new_b = color.b + saturation * (L - color.b);
        //return new Color(new_r, new_g, new_b);
    }
    static Color Primary(Color color)
    {
        if (color.r == color.g && color.r == color.b)
        {
            return Color.white;
        }
        else if (color.r == color.maxColorComponent)
        {
            return new Color(1, color.g, color.b);
        }
        else if (color.g == color.maxColorComponent)
        {
            return new Color(color.r, 1, color.b);
        }
        else if (color.b == color.maxColorComponent)
        {
            return new Color(color.r, color.g, 1);
        }
        return color;
    }
}