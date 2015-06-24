using UnityEngine;
using System.Collections;

namespace Assets.SpaceCraft
{
    class AiSpacecraft : SpacecraftController
    {
        public Transform target;
        public Vector3 targetPosition;
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(targetPosition!=null)
                RotateTowards(targetPosition);
        }
        public override void Update()
        {
            base.Update();

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
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawSphere(target.position, 1);
            Gizmos.color = Color.blue * 0.5f;
            Gizmos.DrawSphere(targetPosition, 1);
        }
    }
}
