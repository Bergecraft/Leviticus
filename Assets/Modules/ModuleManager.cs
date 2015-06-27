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
    public class ModuleManager : MonoBehaviour
    {
        public static Dictionary<string, WeaponDef> weaponDefs = new Dictionary<string, WeaponDef>();
        public static Dictionary<string, AmmoDef> ammoDefs = new Dictionary<string, AmmoDef>();

        public void Awake()
        {
            LoadWeaponsJson();
        }
        public static void LoadWeaponsJson()
        {
            var weaponsJsonText = Resources.Load<TextAsset>("Modules/weapons").text;
            //var weaponsJsonText = File.ReadAllLines("Assets/Resources/Modules/weapons.json").Aggregate((sum,s) => sum + "\n" + s);
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
