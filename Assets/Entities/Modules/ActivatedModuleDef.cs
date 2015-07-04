using Assets.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Entities.Modules
{
    public class ActivatedModuleDef : ModuleDef
    {
        public float energyCost { get; set; }
        public float cooldown { get; set; }
    }
}
