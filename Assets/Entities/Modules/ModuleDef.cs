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
        public string TypeDefName { get; set; }
        public Hardpoint.HardpointSize hardpointSize { get; set; }
        public HardpointDef[] hardpoints { get; set; }
    }
}
