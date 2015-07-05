using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities.Modules.Thrusters
{
    public class RCSBehaviour : SpriteBehaviour<ThrusterDef>
    {
        SpacecraftController parent;
        GameObject[] exhausts = new GameObject[4];
        void Awake()
        {
            base.Awake();

            foreach(var i in Enumerable.Range(0,4))
            {
                var rotation = Quaternion.Euler(0,0,i*90);
                var offset = rotation * Vector3.up;
                
                var go = new GameObject("exhaust");
                go.transform.parent = transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = rotation;
                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = Resources.Load<Sprite>("Modules/Hardpoints_xcf-RCS__6px__exhaust");
                sr.sortingOrder = 2;
                exhausts[i] = go;
            }
            parent = GetComponentInParent<SpacecraftController>();
        }
        public void updateExhausts(float torque){

            var perp = CalculatePerp(transform.position - parent.transform.position, torque);
            foreach (var exhaust in exhausts)
            {
                var color = Color.white;
                var dot = Vector3.Dot(perp, -exhaust.transform.right);
                color.a = Mathf.Max(dot,0);// *Mathf.Abs(torque) * 1000;
                exhaust.GetComponent<SpriteRenderer>().color = color;
            }
        }
        private Vector3 CalculatePerp(Vector3 position, float direction)
        {
            if (direction > 0)
            {
                return Vector3.Cross(Vector3.forward, position).normalized;
            }
            else
            {
                return Vector3.Cross(position, Vector3.forward).normalized;
            }
        }
    }
}
