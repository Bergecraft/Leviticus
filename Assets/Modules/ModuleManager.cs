using Assets.Modules.Weapons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Modules
{
    class ModuleManager : MonoBehaviour
    {
        public static Dictionary<string, WeaponDef> weaponDefs = new Dictionary<string, WeaponDef>();
        public static Dictionary<string, AmmoDef> ammoDefs = new Dictionary<string, AmmoDef>();

        public void Awake()
        {
            LoadWeaponsJson();
        }
        public static void LoadWeaponsJson()
        {
            var weaponsJsonText = File.ReadAllText("Assets/Resources/Modules/weapons.json");
            var WeaponsJson = JsonConvert.DeserializeObject<WeaponsJson>(weaponsJsonText);

            foreach (var def in WeaponsJson.weaponDefs)
            {
                weaponDefs[def.fullName] = def;
            }
            foreach (var def in WeaponsJson.ammoDefs)
            {
                ammoDefs[def.fullName] = def;
            }
        }
    }
}
