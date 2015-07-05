using Assets.Entities;
using Assets.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Modules
{
    public class DefinitionBehaviour<T> : MonoBehaviour where T: EntityDef
    {
        public void Awake()
        {
            if (selectedDefinition != null && selectedDefinition != "")
            {
                def = DefinitionManager.GetDefinition<T>(selectedDefinition);
            }
            else
            {
                def = null;
            }
        }
        public void setDefinition(string definition)
        {
            selectedDefinition = definition;
            def = DefinitionManager.GetDefinition<T>(selectedDefinition);
        }
        public string selectedDefinition;
        public T def { get; set; }
        //public void LoadDefinition(T def)
        //{
        //    foreach(var prop in typeof(T).GetProperties())
        //    {
        //        prop.SetValue(this, prop.GetValue(def, null), null);
        //    }
        //}
    }
}
