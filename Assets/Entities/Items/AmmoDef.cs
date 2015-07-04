using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Entities.Items
{
    public class AmmoDef : ItemDef
    {
        public int size { get; set; }
        public float damageVelocityScalar { get; set; }
        public float bonusDamage { get; set; }
        public float explosiveForce { get; set; }
        public float mass { get; set; }
        public float lifetime { get; set; }
        public float accuracy { get; set; }
    }
}
