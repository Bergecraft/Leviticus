using UnityEngine;
using System.Collections;
using Assets.Modules;
using System.Linq;
using Assets.Modules.Weapons;

public class Weapon : MonoBehaviour {
    //public Ammo prefab;
    private float lastFire;

    public string selectedWeapon;
    public string selectedAmmo;
    public WeaponDef def;
    public AmmoDef ammodef;
    public GameObject ammoPrefab;
	// Use this for initialization
	void Start () {
        def = ModuleManager.weaponDefs[selectedWeapon];
        buildAmmo();
	}
	
	// Update is called once per frame
	void Update () {
	}
    public void Fire()
    {
        if (def != null && ammoPrefab !=null)
        {
            if (Time.time - lastFire > def.cooldown)
            {
                lastFire = Time.time;
                var bullet = Instantiate(ammoPrefab);
                //bullet.source = transform.GetComponentInParent<SpacecraftController>();
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                //bullet.transform.Rotate(0, 0, 180 * (1 - def.accuracy * ammodef.accuracy));
                bullet.GetComponent<Ammo>().def = ammodef;
                bullet.SetActive(true);
                //bullet.GetComponent<Rigidbody2D>().velocity = MUZZLE_VELOCITY * transform.up;
                bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * def.force, ForceMode2D.Impulse);
            }
        }
        
    }
    private void buildAmmo()
    {
        ammodef = ModuleManager.ammoDefs.Values.Where(a => a.type == def.validAmmoType && a.size == def.ammoSize).First();
        if(ammodef!=null){
            ammoPrefab = new GameObject();
            ammoPrefab.name = ammodef.fullName+"_Prefab";
            ammoPrefab.hideFlags = HideFlags.HideInHierarchy;
            ammoPrefab.SetActive(false);
            ammoPrefab.layer = 8;
            //Ammo Script
            var ammo = ammoPrefab.AddComponent<Ammo>();
            ammo.def = ammodef;
            ammo.source = transform.GetComponentInParent<SpacecraftController>();

            var spriteRenderer = ammoPrefab.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>(ammodef.spritePath);
            spriteRenderer.color = new Color(ammodef.spriteColor.r, ammodef.spriteColor.g, ammodef.spriteColor.b, ammodef.spriteColor.a);

            var collider = ammoPrefab.AddComponent<PolygonCollider2D>();
            collider.isTrigger = true;
            //Physics2D.IgnoreCollision(collider, ammo.source.GetComponent<Collider2D>());
            Physics2D.IgnoreLayerCollision(8, 8);
            var rigidbody = ammoPrefab.AddComponent<Rigidbody2D>();
            rigidbody.mass = ammodef.mass;
        }
    }
}
