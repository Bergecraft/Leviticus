using Assets.Entities.Items;
using Assets.Entities.Modules.Thrusters;
using Assets.Entities.Modules.Weapons;
using Assets.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Entities.Modules;
using Assets.Entities;

namespace Assets.Modules
{
    public class DefinitionManager : MonoBehaviour
    {
        public static Dictionary<Type, Dictionary<string, EntityDef>> defs = new Dictionary<Type, Dictionary<string, EntityDef>>();
        //public static Dictionary<string, WeaponDef> weaponDefs = new Dictionary<string, WeaponDef>();
        //public static Dictionary<string, AmmoDef> ammoDefs = new Dictionary<string, AmmoDef>();
        //public static Dictionary<string, ThrusterDef> thrusterDefs = new Dictionary<string, ThrusterDef>();

        public void Awake()
        {
            LoadWeaponsJson();
        }
        private static void AddDefinition(EntityDef def)
        {
            if (!defs.ContainsKey(def.GetType()))
            {
                defs[def.GetType()] = new Dictionary<string, EntityDef>();
            }
            defs[def.GetType()][def.definitionType] = def;
        }
        public static T GetDefinition<T>(string definitionName) where T : EntityDef
        {
            if (defs.ContainsKey(typeof(T)))
            {
                var definitions = defs[typeof(T)];
                if (definitions.ContainsKey(definitionName))
                {
                    return (T)defs[typeof(T)][definitionName];
                }
            }
            return null;
        }
        public static List<T> GetAllDefinitions<T>() where T : EntityDef
        {
            if (defs.ContainsKey(typeof(T)))
            {
                return defs[typeof(T)].Values.Select(e => (T)e).ToList();
            }
            return new List<T>();
        }
        public static void LoadWeaponsJson()
        {
            var weaponsJsonText = Resources.Load<TextAsset>("Modules/weapons").text;
            //var weaponsJsonText = File.ReadAllLines("Assets/Resources/Modules/weapons.json").Aggregate((sum,s) => sum + "\n" + s);
            var WeaponsJson = JsonConvert.DeserializeObject<WeaponsJson>(weaponsJsonText);

            foreach (var def in WeaponsJson.weaponDefs)
            {
                AddDefinition(def);
                //weaponDefs[def.definitionType] = def;
            }
            foreach (var def in WeaponsJson.ammoDefs)
            {
                AddDefinition(def);
                //ammoDefs[def.definitionType] = def;
            }
            foreach (var def in WeaponsJson.thrusterDefs)
            {
                AddDefinition(def);
            }
            foreach (var def in WeaponsJson.moduleDefs)
            {
                AddDefinition(def);
            }
        }
    }
}
