using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Entities.Modules.Thrusters
{
    public class ThrusterDef : ToggledModuleDef
    {
        public float thrust { get; set; }
        public bool omnidirectional { get; set; }
        public float specificImpulse { get; set; }
        public string fuelType { get; set; }
    }
}
