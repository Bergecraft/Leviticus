using Assets.Entities.Items;
using Assets.Entities.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Entities.Spacecraft
{
    class SpacecraftDef : ItemDef
    {
        public string Faction { get; set; }
        public int MaxHealth { get; set; }
        public int MaxShield { get; set; }
        public int ShieldRegen { get; set; }
        public float SpaceDragCoeff { get; set; }
        public float LinearDrag { get; set; }
        public float AngularDrag { get; set; }
        public HardpointDef[] Hardpoints { get; set; }
    }
}
