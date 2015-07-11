using UnityEngine;
using System.Collections;

public class PlayerSpacecraft : SpacecraftController
{
    public bool godMode = false;
    public bool reverse = false;
    public enum ControlMode { MouseFollow, Keyboard };
    public ControlMode controlMode = ControlMode.MouseFollow;
    Transform target;
    public override void Update()
    {
        base.Update();

        HandleKeysForThrust();
        if (Input.GetKey(KeyCode.Space))
        {
            FirePrimary();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            controlMode = controlMode==ControlMode.MouseFollow?ControlMode.Keyboard:ControlMode.MouseFollow;
        }

        turretTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (godMode)
        {
            shield = MAX_SHIELD;
            health = MAX_HEALTH;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (reverse)
        {
            var target = GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().velocity;
            TorqueTowards(target);
        }
        else if (controlMode == ControlMode.MouseFollow)
        {
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TorqueTowards(target);
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
                TorqueTowards(transform.position + rotationTarget);
                var dot = Vector3.Dot(transform.up, rotationTarget);
                if (dot > 0.8f)
                {
                    ActivateThrusters();
                }
                else
                {
                    DeactivateThrusters();
                }
            }
            else
            {
                DeactivateThrusters();
            }
        }
    }

    private void HandleKeysForThrust()
    {
        if (controlMode == ControlMode.MouseFollow)
        {
            if (Input.GetKey(KeyCode.W))
            {
                ActivateThrusters();
            }
            else
            {
                DeactivateThrusters();
            }
            if (Input.GetKey(KeyCode.A))
            {
                GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * MANEUVERING_THRUST);
            }
            if (Input.GetKey(KeyCode.D))
            {
                GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * MANEUVERING_THRUST);
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
