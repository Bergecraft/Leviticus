﻿using UnityEngine;
using System.Collections;
using Assets;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;
using DG.Tweening;
using Assets.Entities.Modules.Thrusters;
using System.Linq;
using Assets.SpaceCraft;
using Assets.Entities.Modules.Shields;
using Assets.Entities.Modules.Reactors;

public class SpacecraftController : MonoBehaviour {

    protected float ROTATE_SPEED = 250;
    protected float MANEUVERING_THRUST = 1000;
    public float DRAG_COEFF = 1.0f;

    [HideInInspector]
    protected MainThrusterBehaviour[] mainThrusters;
    [HideInInspector]
    public WeaponBehaviour[] blasters;
    [HideInInspector]
    protected RCSBehaviour[] rcs;
    [HideInInspector]
    public ShieldBehaviour[] shields;
    [HideInInspector]
    public ReactorBehaviour reactor;
    protected float throttle = 1.0f;

    private float health;
    //private float shield;
    //public float MAX_SHIELD = 20;
    //public float SHIELD_REGEN = 3;
    public float MAX_HEALTH = 100;
    public string faction;
    [HideInInspector]
    public Vector3 turretTarget{get;set;}
    private float velocity;

    StatusBar status;

    SpacecraftManager manager;
	// Use this for initialization
	void Start () {
        mainThrusters = transform.GetComponentsInChildren<MainThrusterBehaviour>();
        blasters = transform.GetComponentsInChildren<WeaponBehaviour>();
        rcs = transform.GetComponentsInChildren<RCSBehaviour>();
        shields = transform.GetComponentsInChildren<ShieldBehaviour>();
        reactor = transform.GetComponentInChildren<ReactorBehaviour>();
        ROTATE_SPEED = rcs.Select(r => r.transform.position.magnitude * r.def.thrust).Aggregate((sum, r) => sum + r);
        MANEUVERING_THRUST = rcs.Select(r => r.def.thrust).Aggregate((sum, r) => sum + r);
        status = Instantiate(Resources.Load<StatusBar>("Status"));
        health = MAX_HEALTH;
        //shield = MAX_SHIELD;

        CreateIcon();

        manager = GameObject.FindObjectOfType<SpacecraftManager>();
        manager.Add(this);

        GetComponent<SpriteRenderer>().sortingOrder = -1;
	}

    private void CreateIcon()
    {
        var icon = new GameObject("icon");
        icon.transform.parent = transform;
        icon.transform.localPosition = Vector3.zero;
        icon.transform.localScale = Vector3.one *3;
        icon.layer = 10;
        var sr = icon.AddComponent<SpriteRenderer>();
        sr.color = GetComponent<SpriteRenderer>().color;
        sr.sprite = Resources.Load<Sprite>("Spacecraft/Icons_xcf-TriangleIcon");
    }
    public void FirePrimary()
    {
        foreach (var blaster in blasters)
        {
            blaster.Fire();
        }
    }
    public void ActivateThrusters()
    {
        foreach (var thruster in mainThrusters)
        {
            thruster.On();
        }
    }
    public void DeactivateThrusters()
    {
        foreach (var thruster in mainThrusters)
        {
            thruster.Off();
        }
    }
    public string ColoredName
    {
        get
        {
            var color = GetComponent<SpriteRenderer>().color;
            return MessageController.GetColoredString(gameObject.name, color);
        }
    }
    public void Damage(float damage,Vector3 position, SpacecraftController source = null)
    {
        var hitShields = shields.Where(s => s.Overlaps(position))
            .OrderBy(s => s.ShieldPercentage).ToArray();
        float overflow = damage;
        if (hitShields.Length>0)
        {
            overflow = hitShields[0].Damage(damage, position);
        }
        health = Mathf.Clamp(health - overflow, 0, MAX_HEALTH);
        //GameObject.FindObjectOfType<MessageController>().AddMessage(damage.ToString("n2") + " damage");

     
        if (health == 0)
        {
            var deathMessage = ColoredName + " destroyed";
            if (source != null)
            {
                deathMessage += " by " + source.ColoredName;
            }
            GameObject.FindObjectOfType<MessageController>().AddMessage(deathMessage);
                
            var explo = Instantiate(Resources.Load<Transform>("Explosion"));
            explo.position = this.transform.position;
            Destroy(this.gameObject);
            Destroy(explo.gameObject,5);
            Destroy(status.gameObject);
            if(GameObject.FindObjectOfType<PlayerSpacecraft>()==null ||
               GameObject.FindObjectsOfType<AiSpacecraft>().Where(ai => ai.isActiveAndEnabled).Count() == 0)
            {
                GameOver();
            }
        }
    }

    private static void GameOver()
    {
        DOTween.To(() => Time.timeScale, (t) => Time.timeScale = t, 0.1f, 0.5f);
        DOTween.To(() => Time.fixedDeltaTime, (t) => Time.fixedDeltaTime = t, 0.1f * 0.02f, 0.5f);
        DOTween.To(() => Camera.main.orthographicSize, (s) => Camera.main.orthographicSize = s, 2, 1);
        var theEnd = GameObject.Find("The End").GetComponent<CanvasGroup>();
        DOTween.To(() => theEnd.alpha, (a) => theEnd.alpha = a, 1.0f, 1f);
    }
	// Update is called once per frame
    public virtual void Update()
    {
        velocity = GetComponent<Rigidbody2D>().velocity.magnitude;
        status.transform.position = transform.position;
        status.UpdateHealth(health / MAX_HEALTH);
        status.UpdateShield(shields.Select(s => s.ShieldPercentage).Min());
        status.UpdateEnergy(reactor.EnergyPercentage);
	}
    public virtual void FixedUpdate()
    {
        ApplyDrag();
        ApplyThrust();
    }

    private void ApplyDrag()
    {
        GetComponent<Rigidbody2D>().AddForce(-DRAG_COEFF * GetComponent<Rigidbody2D>().velocity.normalized * GetComponent<Rigidbody2D>().velocity.sqrMagnitude);

    }
    private void ApplyThrust()
    {
        foreach (var thruster in mainThrusters)
        {
            GetComponent<Rigidbody2D>().AddForceAtPosition(thruster.ThrustVector * throttle, thruster.position2d);
        }
    }
    public void TorqueTowards(Vector2 target)
    {
        TorqueTowards(new Vector3(target.x, target.y, 0));
    }
    protected void TorqueTowards(Vector3 target)
    {
        target.z = 0;
        var targetRotation = (target - transform.position).normalized;
        var angle = Vector3.Angle(targetRotation, transform.up);
        var sign = Vector3.Cross(targetRotation, transform.up);
        angle = -Mathf.Sign(sign.z) * angle;
        var vel = GetComponent<Rigidbody2D>().angularVelocity;
        var torque = ROTATE_SPEED * (angle/180 - Mathf.Clamp(vel,-400, 400)/400);
        GetComponent<Rigidbody2D>().AddTorque(torque);
        //var lookRot = Quaternion.LookRotation(Vector3.forward, target - transform.position);
        //var quat = Quaternion.RotateTowards(transform.rotation, lookRot, ROTATE_SPEED * Time.fixedDeltaTime);

        //GetComponent<Rigidbody2D>().MoveRotation(quat.eulerAngles.z);
        foreach (var r in rcs)
        {
            r.updateExhausts(torque);
        }
    }
    public void StrafeTowardsRelative(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().AddRelativeForce(direction * MANEUVERING_THRUST);
    }

    public string ToJson()
    {
        var tw = new SpacecraftWrapper<SpacecraftController>(this);
        var json = JsonConvert.SerializeObject(tw, Formatting.Indented);
        //var json = JsonConvert.SerializeObject(transform);
        return json;
        //File.WriteAllText(fileName, json);
    }
    public void OnDestroy()
    {
        manager.Remove(this);
    }
    public void SetHealth(float value)
    {
        health = Mathf.Clamp(value, 0, MAX_HEALTH);
    }
}

