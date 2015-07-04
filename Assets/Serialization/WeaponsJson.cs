using Assets.Entities.Items;
using Assets.Entities.Modules.Thrusters;
using Assets.Entities.Modules.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Serialization
{
    public class WeaponsJson
    {
        public WeaponDef[] weaponDefs;
        public AmmoDef[] ammoDefs;
        public ThrusterDef[] thrusterDefs;
    }
}
