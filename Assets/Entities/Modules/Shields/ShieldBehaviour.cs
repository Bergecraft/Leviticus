using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities.Modules.Shields
{
    public class ShieldBehaviour : SpriteBehaviour<ShieldGeneratorDef>
    {
        private float shield { get; set; }

        void Start()
        {
            base.Start();
            shield = def.maxShield;
        }
        public float Damage(float damage)
        {
            var overflow = -Mathf.Min(shield - damage, 0);
            shield = Mathf.Clamp(shield - damage, 0, def.maxShield);
            return overflow;
        }

        public void Update()
        {
            shield = Mathf.Clamp(shield + def.shieldRegen * Time.deltaTime, 0, def.maxShield);
        }
        public float ShieldPercentage
        {
            get
            {
                return shield / def.maxShield;
            }
        }
        public void SetShield(float value)
        {
            shield = Mathf.Clamp(value, 0, def.maxShield);
        }
        public void SetShieldPercentage(float value)
        {
            shield = Mathf.Clamp(def.maxShield * value, 0, def.maxShield);
        }
    }
}
