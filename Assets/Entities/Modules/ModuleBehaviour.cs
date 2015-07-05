using Assets.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities.Modules
{
    public class ModuleBehaviour : SpriteBehaviour<ModuleDef>
    {
        public void OnValidate()
        {
            DefinitionManager.LoadWeaponsJson();
            this.Awake();
            this.Start();
            var hpsTrans = transform.FindChild("hardpoints");
            if (def!=null && def.hardpoints.Length > 0 && hpsTrans == null)
            {
                var hpsgo = new GameObject("hardpoints");
                hpsgo.transform.parent = this.transform;
                hpsgo.transform.localPosition = Vector3.zero;
                hpsTrans = hpsgo.transform;

                foreach (var hpdef in def.hardpoints)
                {
                    if (hpsTrans.FindChild(hpdef.name) == null)
                    {
                        var hpgo = new GameObject(hpdef.name);
                        hpgo.transform.parent = hpsTrans;
                        hpgo.transform.localPosition = Vector3.zero;
                        var hp = hpgo.AddComponent<Hardpoint>();
                        hp.size = hpdef.size;
                        hp.type = hpdef.type;
                        hp.transform.localPosition = hpdef.position;
                        hp.transform.localEulerAngles = hpdef.rotation;
                        hp.transform.localScale = hpdef.scale;
                        hp.OnValidate();
                    }
                }
            }
            else if ((def == null || def.hardpoints.Length == 0) && hpsTrans != null)
            {
                hpsTrans.name = "delete me";
                //GameObject.DestroyImmediate(hpsTrans);
            }
        }
    }
}
