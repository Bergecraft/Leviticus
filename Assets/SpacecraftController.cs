using UnityEngine;
using System.Collections;

public class SpacecraftController : MonoBehaviour {

    public float ROTATE_SPEED = 250;
    public float MAIN_THRUST = 10000;
    public float MANEUVERING_THRUST = 1000;
    public float DRAG_COEFF = 1.0f;

    private MainThruster[] mainThrusters;
    private Blaster[] blasters;

    [Header("Debug")]
    public float velocity;

	// Use this for initialization
	void Start () {
        mainThrusters = transform.GetComponentsInChildren<MainThruster>();
        blasters = transform.GetComponentsInChildren<Blaster>();
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
	// Update is called once per frame
    void Update()
    {
        velocity = GetComponent<Rigidbody2D>().velocity.magnitude;
	}
    public void FixedUpdate()
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
            GetComponent<Rigidbody2D>().AddForceAtPosition(thruster.ThrustVector, thruster.position2d);
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
}
