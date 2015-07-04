using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Expressions;
using UnityEngine;

namespace Assets.Serialization
{
    public class IgnoreableContractResolver : DefaultContractResolver
    {
        public static IgnoreableContractResolver Default{
            get
            {
                return new IgnoreableContractResolver()
                    //.Ignore(typeof(MonoBehaviour))
                    .Ignore<MonoBehaviour>(m => m.gameObject)
                    .Ignore<Transform>(t => t.childCount)
                    .Ignore<Transform>(t => t.eulerAngles)
                    .Ignore<Transform>(t => t.forward)
                    .Ignore<Transform>(t => t.gameObject)
                    .Ignore<Transform>(t => t.hasChanged)
                    .Ignore<Transform>(t => t.hideFlags)
                    .Ignore<Transform>(t => t.localRotation)
                    .Ignore<Transform>(t => t.localToWorldMatrix)
                    .Ignore<Transform>(t => t.lossyScale)
                    .Ignore<Transform>(t => t.parent)
                    .Ignore<Transform>(t => t.position)
                    .Ignore<Transform>(t => t.right)
                    .Ignore<Transform>(t => t.root)
                    .Ignore<Transform>(t => t.rotation)
                    .Ignore<Transform>(t => t.transform)
                    .Ignore<Transform>(t => t.up)
                    .Ignore<Transform>(t => t.worldToLocalMatrix)
                    .Ignore<Vector3>(v => v.magnitude)
                    .Ignore<Vector3>(v => v.normalized)
                    .Ignore<Vector3>(v => v.sqrMagnitude)
                    .Ignore<Color>(c => c.gamma)
                    .Ignore<Color>(c => c.grayscale)
                    .Ignore<Color>(c => c.linear)
                    .Ignore<Color>(c => c.maxColorComponent);
            }
        }
        public bool ignoreDerived; 
        //protected readonly List<Type> ignoredTypes;
        protected readonly Dictionary<Type, HashSet<string>> ignores;
        public IgnoreableContractResolver(bool ignoreDerived = true)
        {
            //this.ignoredTypes = new List<Type>();
            this.ignores = new Dictionary<Type, HashSet<string>>();
            this.ignoreDerived = ignoreDerived;
        }
        public IgnoreableContractResolver Ignore(Type type, params string[] propertyName)
        {
            if (!this.ignores.ContainsKey(type)) this.ignores[type] = new HashSet<string>();
            foreach (var prop in propertyName)
            {
                this.ignores[type].Add(prop);
            }
            if (ignoreDerived && type.BaseType!=null)
            {
                Ignore(type.BaseType, propertyName);
            }

            return this;
        }
        public IgnoreableContractResolver Ignore<TModel>(Expression<System.Func<TModel, object>> selector)
        {
            MemberExpression body = selector.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)selector.Body;
                body = ubody.Operand as MemberExpression;

                if (body == null)
                {
                    throw new ArgumentException("Could not get property name", "selector");
                }
            }

            string propertyName = body.Member.Name;
            this.Ignore(typeof(TModel), propertyName);
            return this;
        }
        public bool IsIgnored(Type type, string propertyName)
        {
            if (!this.ignores.ContainsKey(type)) return false;

            // if no properties provided, ignore the type entirely
            if (this.ignores[type].Count == 0) return true;

            return this.ignores[type].Contains(propertyName);
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (   this.IsIgnored(property.DeclaringType, property.PropertyName)
                || this.IsIgnored(property.DeclaringType.BaseType, property.PropertyName)
                //|| ignoredTypes.Contains(property.DeclaringType))
                )
            {
                property.ShouldSerialize = instance => { return false; };
            }
            return property;
        }
    }
}
