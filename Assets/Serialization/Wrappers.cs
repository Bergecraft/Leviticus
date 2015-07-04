using Assets.Entities.Modules.Thrusters;
using Assets.Entities.Modules.Weapons;
using Assets.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class ValueWrapper<T>
    {
        [JsonIgnore]
        protected T model;
        public ValueWrapper(T prop)
        {
            model = prop;
        }
    }
    public class RefWrapper<T>
    {
        [JsonIgnore]
        protected T model;
        public RefWrapper(ref T prop)
        {
            model = prop;
        }
    }
    public class PropertyWrapper<T>
    {
        [JsonIgnore]
        private Func<T> getModel;
        private Action<T> setModel;
        protected T model
        {
            get
            {
                return getModel();
            }
            set
            {
                setModel(value);
            }
        }
        public PropertyWrapper(Func<T> getter, Action<T> setter)
        {
            getModel = getter;
            setModel = setter;
        }
    }
    [Serializable]
    public class TransformWrapper : ValueWrapper<Transform>
    {
        public Vector3Wrapper localPosition;
        public Vector3Wrapper localEulerAngles;
        public Vector3Wrapper localScale;
        
        public TransformWrapper(Transform t)
            : base(t)
        {
            localPosition = new Vector3Wrapper(() => model.localPosition, value => model.localPosition = value);
            localEulerAngles = new Vector3Wrapper(() => model.localEulerAngles, value => model.localEulerAngles = value);
            localScale = new Vector3Wrapper(() => model.localScale, value => model.localScale = value);
        }
        public string name
        {
            get { return model.name; } 
            set { model.name = value; }
        }
    }
    public class MonoBehaviourWrapper<T> : ValueWrapper<T> where T: MonoBehaviour
    {
        public TransformWrapper transform;
        
        public MonoBehaviourWrapper(T t)
            : base(t)
        {
            transform = new TransformWrapper(model.transform);
        }
    }
    public class SpacecraftWrapper<T> : MonoBehaviourWrapper<T> where T : SpacecraftController
    {
        public float DRAG_COEFF
        {
            get { return model.DRAG_COEFF; }
            set { model.DRAG_COEFF = value; }
        }
        public WeaponDef[] weapons;
        public ThrusterDef[] mainThrusters;
        public SpacecraftWrapper(T t)
            : base(t)
        {
            //children = Enumerable.Range(0, model.childCount).Select(i => new TransformWrapper(model.GetChild(i))).ToArray();
            weapons = model.transform.GetComponentsInChildren<WeaponBehaviour>().Select(b => b.def).ToArray();
            mainThrusters = model.transform.GetComponentsInChildren<MainThrusterBehaviour>().Select(b => b.def).ToArray();
        }
    }

    [Serializable]
    public class Vector3Wrapper : PropertyWrapper<Vector3>
    {
        public Vector3Wrapper(Func<Vector3> getter, Action<Vector3> setter) : base(getter, setter) { }
        public float x
        {
            get { return model.x; }
            set { model = new Vector3(value, model.y, model.z); }
        }
        public float y
        {
            get { return model.y; }
            set { model = new Vector3(model.x, value, model.z); }
        }
        public float z
        {
            get { return model.z; }
            set { model = new Vector3(model.x, model.y, value); }
        }
    }
}
