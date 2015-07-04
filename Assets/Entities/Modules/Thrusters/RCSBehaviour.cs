using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities.Modules.Thrusters
{
    public class RCSBehaviour : SpriteBehaviour<ThrusterDef>
    {
        void Awake()
        {
            base.Awake();
            var go = new GameObject("exhaust");
        }
    }
}
