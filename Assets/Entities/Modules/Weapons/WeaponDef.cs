using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Entities.Modules.Weapons
{
    public class WeaponDef : ActivatedModuleDef
    {
        public float force { get; set; }
        public float accuracy { get; set; }
        public int ammoSize { get; set; }
        public string validAmmoType { get; set; }
    }
}
