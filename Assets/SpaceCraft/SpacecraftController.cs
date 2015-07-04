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

public class SpacecraftController : MonoBehaviour {

    protected float ROTATE_SPEED = 250;
    protected float MANEUVERING_THRUST = 1000;
    public float DRAG_COEFF = 1.0f;

    protected MainThrusterBehaviour[] mainThrusters;
    protected WeaponBehaviour[] blasters;
    protected RCSBehaviour[] rcs;
    protected float throttle = 1.0f;

    private float health;
    private float shield;
    public float MAX_SHIELD = 20;
    public float MAX_HEALTH = 100;

    public string faction;

    [Header("Debug")]
    public float velocity;

    StatusBar status;
	// Use this for initialization
	void Start () {
        mainThrusters = transform.GetComponentsInChildren<MainThrusterBehaviour>();
        blasters = transform.GetComponentsInChildren<WeaponBehaviour>();
        rcs = transform.GetComponentsInChildren<RCSBehaviour>();
        ROTATE_SPEED = rcs.Select(r => r.transform.position.magnitude * r.def.thrust).Aggregate((sum, r) => sum + r);
        MANEUVERING_THRUST = rcs.Select(r => r.def.thrust).Aggregate((sum, r) => sum + r);
        status = Instantiate(Resources.Load<StatusBar>("Status"));
        health = MAX_HEALTH;
        shield = MAX_SHIELD;
	}
    public void FirePrimary()
    {
        foreach (var blaster in blasters)
        {
            blaster.Fire();
        }
    }
    public void ActiveThrusters()
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
    public void Damage(float damage)
    {
        var overflow = -Mathf.Min(shield - damage, 0);
        shield = Mathf.Clamp(shield - damage, 0, MAX_SHIELD);
        health = Mathf.Clamp(health - overflow, 0, MAX_HEALTH);
        GameObject.FindObjectOfType<MessageController>().AddMessage(damage.ToString("n2") + " damage");
                
        if (health == 0)
        {
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
        shield = Mathf.Clamp(shield + 1 * Time.deltaTime, 0, MAX_SHIELD);
        status.UpdateShield(shield / MAX_SHIELD);
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
    protected void RotateTowards(Vector2 target)
    {
        RotateTowards(new Vector3(target.x, target.y, 0));
    }
    protected void RotateTowards(Vector3 target)
    {
        target.z = 0;
        var targetRotation = (target - transform.position).normalized;
        var angle = Vector3.Angle(targetRotation, transform.up);
        var sign = Vector3.Cross(targetRotation, transform.up);
        angle = -Mathf.Sign(sign.z) * angle;
        var vel = GetComponent<Rigidbody2D>().angularVelocity;
        GetComponent<Rigidbody2D>().AddTorque(ROTATE_SPEED * (angle/180 - vel/400));
        //var lookRot = Quaternion.LookRotation(Vector3.forward, target - transform.position);
        //var quat = Quaternion.RotateTowards(transform.rotation, lookRot, ROTATE_SPEED * Time.fixedDeltaTime);

        //GetComponent<Rigidbody2D>().MoveRotation(quat.eulerAngles.z);
    }

    public string ToJson()
    {
        var tw = new SpacecraftWrapper<SpacecraftController>(this);
        var json = JsonConvert.SerializeObject(tw, Formatting.Indented);
        //var json = JsonConvert.SerializeObject(transform);
        return json;
        //File.WriteAllText(fileName, json);
    }
}

