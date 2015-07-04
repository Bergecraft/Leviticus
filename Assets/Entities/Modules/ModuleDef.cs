using Assets.Entities.Items;
using Assets.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities.Modules
{
    public class ModuleDef : ItemDef
    {
        public Hardpoint.HardpointSize hardpointSize { get; set; }
        public int ammoSize { get; set; }
        public Transform[] slots { get; set; }
    }
}
