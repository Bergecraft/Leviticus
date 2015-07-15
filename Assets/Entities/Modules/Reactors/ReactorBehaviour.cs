using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities.Modules.Reactors
{
    public class ReactorBehaviour : SpriteBehaviour<ReactorDef>
    {
        float _energy = 0;
        public float Energy
        {
            get
            {
                return _energy;
            }
            set
            {
                _energy = Mathf.Clamp(value, 0, def.maxEnergy);
            }
        }
        public float EnergyPercentage
        {
            get
            {
                return _energy / def.maxEnergy;
            }
            set
            {
                Energy = def.maxEnergy * value;
            }
        }
        void Start()
        {
            base.Start();
            _energy = def.maxEnergy;
        }
        public bool Use(float energy)
        {
            if (Energy > energy)
            {
                Energy -= energy;
                return true;
            }
            return false;
        }
        public void Update()
        {
            Energy += def.energyRegen * Time.deltaTime;
        }
    }
}
