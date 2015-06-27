using UnityEngine;
using System.Collections;
using System.Linq;

namespace Assets.SpaceCraft
{
    class AiSpacecraft : SpacecraftController
    {
        public Transform target;
        public Vector3 targetPosition;
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (target != null)
            {
                if (targetPosition != null)
                    RotateTowards(targetPosition);
            }
        }
        public override void Update()
        {
            base.Update();

            if (target != null)
            {
                var offset = target.position - transform.position;

                var ammoVelocity = blasters[0].def.force /blasters[0].ammodef.mass; // TODO: Check this
                var bulletTravelTimeToTarget = offset.magnitude / ammoVelocity;
                var projectedOffset = target.GetComponent<Rigidbody2D>().velocity * bulletTravelTimeToTarget;
                targetPosition = target.position + new Vector3(projectedOffset.x, projectedOffset.y, 0);
                projectedOffset = targetPosition - transform.position;

                var dot = Vector2.Dot(projectedOffset.normalized, transform.up);
                if (dot > 0.90f)
                {
                    FirePrimary();
                }
                if (dot > 0.8f)
                {
                    ActiveThrusters();
                }
                else
                {
                    DeactivateThrusters();
                }
            }
            else
            {
                DeactivateThrusters();

                var ships = GameObject.FindObjectsOfType<SpacecraftController>().Where(s => s != this).OrderBy(s => (s.transform.position - transform.position).magnitude).ToArray();
                if (ships.Length > 0)
                {
                    target = ships[0].transform;
                }
            }
        }
        public void OnDrawGizmosSelected()
        {
            if (target != null)
            {
                Gizmos.color = Color.red * 0.5f;
                Gizmos.DrawSphere(target.position, 1);
                Gizmos.color = Color.blue * 0.5f;
                Gizmos.DrawSphere(targetPosition, 1);
            }
        }
    }
}
