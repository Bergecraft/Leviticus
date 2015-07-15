using UnityEngine;
using System.Collections;

public class PlayerSpacecraft : MonoBehaviour
{
    public bool godMode = false;
    public bool reverse = false;
    public enum ControlMode { MouseFollow, Keyboard };
    public ControlMode controlMode = ControlMode.MouseFollow;
    Transform target;
    SpacecraftController spacecraft;
    void Start()
    {
        spacecraft = GetComponent<SpacecraftController>();
    }
    public void Update()
    {

        HandleKeysForThrust();
        if (Input.GetKey(KeyCode.Space))
        {
            spacecraft.FirePrimary();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            controlMode = controlMode==ControlMode.MouseFollow?ControlMode.Keyboard:ControlMode.MouseFollow;
        }

        spacecraft.turretTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (godMode)
        {
            foreach (var s in spacecraft.shields)
            {
                s.ShieldPercentage = 1;
            }
            spacecraft.SetHealth(spacecraft.MAX_HEALTH);
        }
    }
    public void FixedUpdate()
    {
        if (reverse)
        {
            var target = GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().velocity;
            spacecraft.TorqueTowards(target);
        }
        else if (controlMode == ControlMode.MouseFollow)
        {
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spacecraft.TorqueTowards(target);
        }
        else if (controlMode == ControlMode.Keyboard)
        {
            Vector3 rotationTarget = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                rotationTarget += Vector3.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                rotationTarget += Vector3.down;
            }
            if (Input.GetKey(KeyCode.D))
            {
                rotationTarget += Vector3.right;
            }
            if (Input.GetKey(KeyCode.A))
            {
                rotationTarget += Vector3.left;
            }
            if (rotationTarget.magnitude > 0)
            {
                spacecraft.TorqueTowards(transform.position + rotationTarget);
                var dot = Vector3.Dot(transform.up, rotationTarget);
                if (dot > 0.8f)
                {
                    spacecraft.ActivateThrusters();
                }
                else
                {
                    spacecraft.DeactivateThrusters();
                }
            }
            else
            {
                spacecraft.DeactivateThrusters();
            }
        }
    }

    private void HandleKeysForThrust()
    {
        if (controlMode == ControlMode.MouseFollow)
        {
            if (Input.GetKey(KeyCode.W))
            {
                spacecraft.ActivateThrusters();
            }
            else
            {
                spacecraft.DeactivateThrusters();
            }
            if (Input.GetKey(KeyCode.A))
            {
                spacecraft.StrafeTowardsRelative(Vector2.left);
            }
            if (Input.GetKey(KeyCode.D))
            {
                spacecraft.StrafeTowardsRelative(Vector2.right);
            }
            reverse = false;
            if (Input.GetKey(KeyCode.S))
            {
                if (GetComponent<Rigidbody2D>().velocity.magnitude > 0)
                {
                    reverse = true;
                }
            }
        }
    }
}
