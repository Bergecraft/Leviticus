using UnityEngine;
using System.Collections;
using Assets;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;

public class SpacecraftController : MonoBehaviour {

    public float ROTATE_SPEED = 250;
    public float MAIN_THRUST = 10000;
    public float MANEUVERING_THRUST = 1000;
    public float DRAG_COEFF = 1.0f;

    protected MainThruster[] mainThrusters;
    protected Blaster[] blasters;
    protected float throttle = 1.0f;

    private float health;
    public float MAX_HEALTH = 100;

    [Header("Debug")]
    public float velocity;

    StatusBar status;
	// Use this for initialization
	void Start () {
        mainThrusters = transform.GetComponentsInChildren<MainThruster>();
        blasters = transform.GetComponentsInChildren<Blaster>();

        status = Instantiate(Resources.Load<StatusBar>("Status"));
        health = MAX_HEALTH;
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
            thruster.Activate();
        }
    }
    public void DeactivateThrusters()
    {
        foreach (var thruster in mainThrusters)
        {
            thruster.Deactivate();
        }
    }
    public void Damage(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, MAX_HEALTH);
        if (health == 0)
        {
            var explo = Instantiate(Resources.Load<Transform>("Explosion"));
            explo.position = this.transform.position;
            Destroy(this.gameObject);
            Destroy(explo.gameObject,5);
            Destroy(status.gameObject);
        }
    }
	// Update is called once per frame
    public virtual void Update()
    {
        velocity = GetComponent<Rigidbody2D>().velocity.magnitude;
        status.transform.position = transform.position;
        status.UpdateHealth(health / MAX_HEALTH);
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
        var lookRot = Quaternion.LookRotation(Vector3.forward, target - transform.position);
        var quat = Quaternion.RotateTowards(transform.rotation, lookRot, ROTATE_SPEED * Time.fixedDeltaTime);

        GetComponent<Rigidbody2D>().MoveRotation(quat.eulerAngles.z);
    }

    public void ExportToFile(string fileName)
    {
        var tw = new SpacecraftWrapper<SpacecraftController>(this);
        var json = JsonConvert.SerializeObject(tw, Formatting.Indented);
        //var json = JsonConvert.SerializeObject(transform);
        File.WriteAllText(fileName, json);
    }
}

