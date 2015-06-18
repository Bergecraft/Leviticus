using UnityEngine;
using System.Collections;

public class PlayerSpacecraft : SpacecraftController
{
    public bool autoRotate = true;
    public bool reverse = false;
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
            autoRotate = !autoRotate;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (reverse)
        {
            var target = GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().velocity;
            RotateTowards(target);
        }
        else if (autoRotate)
        {
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RotateTowards(target);
        }
    }

    private void HandleKeysForThrust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            ActiveThrusters();
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
