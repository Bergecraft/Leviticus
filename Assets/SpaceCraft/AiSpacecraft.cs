using UnityEngine;
using System.Collections;
using System.Linq;

namespace Assets.SpaceCraft
{
    public class AiSpacecraft : SpacecraftController
    {
        private string[] neutralFactions = new string[]{/*"Rebel"*/};
        public Transform target;
        public Vector3 targetPosition;
        private float ttt;
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (target != null)
            {
                if (targetPosition != null && (target.position - transform.position).magnitude>4)
                    TorqueTowards(targetPosition);
            }
        }
        float lastSearchTime = 0;
        const float TIME_BETWEEN_SEARCHES = 0.5f;
        public override void Update()
        {
            base.Update();

            if (target != null && target.gameObject.activeInHierarchy)
            {
                //var distance = target.position - transform.position;


                var vamag = blasters[0].def.force / blasters[0].ammodef.mass; // TODO: Check this
                var vt = target.GetComponent<Rigidbody2D>().velocity;
                var v = GetComponent<Rigidbody2D>().velocity;
                var pt = new Vector2(target.position.x, target.position.y);
                var p = new Vector2(transform.position.x, transform.position.y);
                //-------METHOD ONE------------
                //var vt_va = vtmag / vamag;
                //var pp = (pt - p * vt_va) / (1 - vt_va);
                //var o = pp - p;
                //var o_norm = o.normalized;
                //var va = o_norm * vamag;
                //var varmag = (v + va).magnitude;
                //var t = o.magnitude / varmag;
                //---------METHOD TWO--------------

                var t_seed = (p - pt).magnitude / vamag;
                Vector2 va;
                var t = CalculateTimeToTarget(p, pt, v, vt, vamag, t_seed, out va);
                ttt = t;
                //--------------------------------
                //var bulletTravelTimeToTarget = distance.magnitude / ammoVelocity;
                //var projectedOffset = (target.GetComponent<Rigidbody2D>().velocity - GetComponent<Rigidbody2D>().velocity) * bulletTravelTimeToTarget;
                targetPosition = pt + vt * t; //target.position + new Vector3(pp.x, pp.y, 0);
                turretTarget = pt + vt * t - v * t; //targetPosition;
                //pp = targetPosition - transform.position;

                var o = turretTarget - new Vector3(p.x,p.y,0);
                var o_norm = o.normalized;

                foreach (var blaster in blasters)
                {
                    var blastdot = Vector2.Dot(o_norm, blaster.transform.up);
                    if (blastdot > 0.90f)
                    {
                        blaster.Fire();
                    }
                }
                var dot = Vector2.Dot(o_norm, transform.up);

                //if (dot > 0.7f && distance.magnitude < 3)
                //{
                //    //ROTATE away
                //    targetPosition = transform.right * 10;
                //}

                if (dot > 0.9f || o.magnitude < 4)
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

                //GameObject.FindObjectOfType<MessageController>().AddMessage("AI has no target", Color.red);
                if (Time.time > lastSearchTime + TIME_BETWEEN_SEARCHES)
                {
                    lastSearchTime = Time.time;
                    var ships = GameObject.FindObjectsOfType<SpacecraftController>()
                        .Where(s => s != this && s.faction != this.faction && !neutralFactions.Contains(s.faction) && s.isActiveAndEnabled)
                        .OrderBy(s => (s.transform.position - transform.position).magnitude).ToArray();
                    if (ships.Length > 0)
                    {
                        target = ships[0].transform;
                        //GameObject.FindObjectOfType<MessageController>().AddMessage("AI found target", Color.green);
                    }
                }
            }
        }

        public static float CalculateTimeToTarget(Vector2 p, Vector2 pt, Vector2 v, Vector2 vt, float vamag, float t, out Vector2 ammoVelocity, float iterations = 4)
        {
            var pp = p + v * t;
            var ppt = pt + vt * t;
            var o = ppt - pp;
            var newT = o.magnitude / vamag;
            ammoVelocity = o.normalized * vamag;
            if (iterations > 0)
            {
                newT = CalculateTimeToTarget(p, pt, v, vt, vamag, newT, out ammoVelocity, iterations-1);
            }
            return newT;
        }

        public void OnDrawGizmosSelected()
        {
            if (target != null)
            {
                var tvel2d = target.GetComponent<Rigidbody2D>().velocity;
                var tvel = new Vector3(tvel2d.x, tvel2d.y);
                var vel2d = transform.GetComponent<Rigidbody2D>().velocity;
                var vel = new Vector3(vel2d.x, vel2d.y);

                var targetProjected = target.position + tvel * ttt;
                var thisProjected = transform.position + vel * ttt;
                Gizmos.color = Color.red;
                //Gizmos.DrawSphere(target.position, 0.5f);
                Gizmos.DrawLine(target.position, targetProjected);
                Gizmos.color = Color.blue;
                //Gizmos.DrawSphere(transform.position, 0.5f);
                //Gizmos.DrawLine(transform.position, thisProjected);
                Gizmos.DrawLine(targetProjected, targetProjected - vel*ttt);
                Gizmos.color = Color.yellow;
                //Gizmos.DrawLine(targetProjected, thisProjected);
                Gizmos.DrawLine(transform.position, targetProjected - vel * ttt);
            }
        }
    }
}
