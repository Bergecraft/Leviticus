using Assets.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities.Modules
{
    public class TurretBehaviour : ModuleBehaviour
    {
        private SpacecraftController controller;
        private float ROTATE_SPEED = 360;
        void Start()
        {
            base.Start();
            controller = transform.GetComponentInParent<SpacecraftController>();

            var childsort = GetComponent<SpriteRenderer>().sortingOrder +2;
            foreach (var hp in GetComponentsInChildren<Hardpoint>())
            {
                if (hp.gameObject != gameObject)
                {
                    hp.GetComponent<SpriteRenderer>().sortingOrder = childsort;
                }
            }
        }
        void Update()
        {
            var lookRot = Quaternion.LookRotation(Vector3.forward, controller.turretTarget - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, ROTATE_SPEED * Time.deltaTime);
        }
    }
}
