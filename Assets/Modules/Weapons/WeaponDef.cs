using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Modules.Weapons
{
    public class ColorDef
    {
        public float r;
        public float g;
        public float b;
        public float a;
    }
    public class WeaponsJson
    {
        public WeaponDef[] weaponDefs;
        public AmmoDef[] ammoDefs;
    }
    public class SpriteDef : NamedAndTyped
    {
        public string spritePath;
        public ColorDef spriteColor;
    }
    public class ModuleDef : SpriteDef
    {
        public float energyCost;
        public float cooldown;
        public Hardpoint.HardpointSize hardpointSize;
        public int ammoSize;
        public TransformWrapper[] slots;
    }
    public class WeaponDef : ModuleDef
    {
        public float force;
        public float accuracy;
        public string validAmmoType;
    }
    public class NamedAndTyped //: IEqualityComparer<NamedAndTyped>
    {
        public string name;
        public string type;

        public string fullName
        {
            get{
                return type + "/" + name;
            }
            
        }
        public override string ToString()
        {
            return fullName;
        }
    }

    public class AmmoDef : SpriteDef
    {
        public int size;
        public float damageVelocityScalar;
        public float bonusDamage;
        public float explosiveForce;
        public float mass;
        public float lifetime;
        public float accuracy;
    }
}
