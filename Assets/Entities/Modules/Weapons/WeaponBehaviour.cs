using UnityEngine;
using System.Collections;
using Assets.Modules;
using System.Linq;
using Assets.Serialization;
using Newtonsoft.Json;
using Assets.Entities.Items;
using Assets.Entities.Modules.Weapons;
using Assets.Entities.Modules;

public class WeaponBehaviour : SpriteBehaviour<WeaponDef>
{
    //public Ammo prefab;
    private float lastFire;

    public string selectedAmmo;
    //public WeaponDef def;
    public AmmoDef ammodef;
    GameObject ammoPrefab;
    SpacecraftController parent;
	// Use this for initialization
	void Start () {
        base.Start();
        parent = transform.GetComponentInParent<SpacecraftController>();
        if (def != null)
        {
            buildAmmo();
        }
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
                bullet.transform.position = transform.TransformPoint(def.barrelOffset);
                //bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                var zRotation = 180 * (1 - def.accuracy * ammodef.accuracy) * (-0.5f + Random.value);
                bullet.transform.Rotate(0, 0, zRotation);
                //bullet.transform.Rotate(0, 0, 180 * (1 - def.accuracy * ammodef.accuracy));
                bullet.GetComponent<AmmoBehaviour>().def = ammodef;
                bullet.SetActive(true);
                //bullet.GetComponent<Rigidbody2D>().velocity = MUZZLE_VELOCITY * transform.up;
                bullet.GetComponent<Rigidbody2D>().velocity = parent.GetComponent<Rigidbody2D>().velocity;
                bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * def.force, ForceMode2D.Impulse);

                parent.GetComponent<Rigidbody2D>().AddForceAtPosition(-bullet.transform.up * def.force, transform.position, ForceMode2D.Impulse);
            }
        }
        
    }
    private void buildAmmo()
    {
        ammodef = DefinitionManager.GetAllDefinitions<AmmoDef>().Where(a => a.definitionType.StartsWith(def.validAmmoType) && a.size == def.ammoSize).First();
        ammoPrefab = AmmoBehaviour.BuildAmmo(parent, ammodef);
    }
}
