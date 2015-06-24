using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Modules
{
    public class Hardpoint : MonoBehaviour
    {
        public enum HardpointSize { h6=6,h8=8,h12=12,h16=16,h24=24,h32=32,h48=48,h64=64};
        public HardpointSize size;
        public enum HardpointType { none, fuel, hydrogen, };
        public HardpointType type;
        const string HARDPOINT_TEMPLATE = "modules/Hardpoints_xcf-{0}px_Hardpoint";
        void OnValidate()
        {
            if (GetComponent<SpriteRenderer>() == null)
            {
                gameObject.AddComponent<SpriteRenderer>();
            }

            LoadHardpointSprite();
            var spriteName = "";
            if (GetComponent<Weapon>() != null)
            {
                switch (size)
                {
                    case HardpointSize.h6:
                        spriteName = "modules/Hardpoints_xcf-Blaster__6px_";
                        break;
                    case HardpointSize.h8:
                        spriteName = "modules/Hardpoints_xcf-Dual_Blaster__8px_";
                        break;
                    case HardpointSize.h16:
                        spriteName = "modules/Hardpoints_xcf-Turret_Mount__16px__";
                        break;
                }
            }

            var sprite = Resources.Load<Sprite>(spriteName);
            if (sprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = sprite;
            }
                
        }

        private void LoadHardpointSprite()
        {
            var filename = string.Format(HARDPOINT_TEMPLATE, (int)size);
            var sprite = Resources.Load<Sprite>(filename);
            GetComponent<SpriteRenderer>().sprite = sprite;
            switch (type)
            {
                case HardpointType.fuel:
                    GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case HardpointType.hydrogen:
                    GetComponent<SpriteRenderer>().color = Color.cyan;
                    break;
                default:
                    GetComponent<SpriteRenderer>().color = Color.white;
                    break;
            }
        }
        void Awake()
        {
            if (GetComponent<MainThruster>() != null)
            {
                var exhaustPrefab = Resources.Load<GameObject>("modules/Main Thruster");
                var exhaust = Instantiate(exhaustPrefab);
                exhaust.transform.parent = transform;
                exhaust.transform.localPosition = Vector3.zero;
            }
        }
    }
}
