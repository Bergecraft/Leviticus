using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Entities.Modules.Shields
{
    public class ShieldGeneratorDef : ToggledModuleDef
    {
        public float maxShield { get; set; }
        public float shieldRegen { get; set; }
        public float defaultRadius { get; set; }
    }
}
