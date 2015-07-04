using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using UnityEngine;
using Assets;
using Assets.Serialization;
using System.IO;

namespace UnityVS.Leviticus.CSharp.Test
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Vector3WrapperJsonTest()
        {
            var vector3 = new Vector3Wrapper(() => new Vector3(1, 2, 3), v => { });
            var vector3json = JsonConvert.SerializeObject(vector3);
            Assert.AreEqual(vector3json, "{\"x\":1.0,\"y\":2.0,\"z\":3.0}");
            Assert.AreEqual(JsonConvert.DeserializeObject<Vector3Wrapper>(vector3json), vector3);
        }
        class boolObject
        {
            public bool boolean;
        }
        [TestMethod]
        public void boolJsonTest()
        {
            var bo = new boolObject()
            {
                boolean = true
            };
            var jsonText = JsonConvert.SerializeObject(bo);
            Assert.AreEqual(jsonText, "{\"boolean\":true}");
        }
        [TestMethod]
        public void Vector3SerializeTest()
        {
            var vector3 = new Vector3(1, 2, 3);
            var vector3json = JsonConvert.SerializeObject(vector3, new JsonSerializerSettings() { ContractResolver = IgnoreableContractResolver.Default});
            Assert.AreEqual(vector3json, "{\"x\":1.0,\"y\":2.0,\"z\":3.0}");
        }
        [TestMethod]
        public void Vector3DeserializeTest()
        {
            var vector3 = JsonConvert.DeserializeObject<Vector3>("{\"x\":1.0,\"y\":2.0,\"z\":3.0}");
            Assert.AreEqual(vector3, new Vector3(1, 2, 3));
        }
        [TestMethod]
        public void JsonConfigTest()
        {
            #if UNITY
            var weaponsJsonText = Resources.Load<TextAsset>("Modules/weapons").text;
            #else
            var weaponsJsonText = File.ReadAllText("C:/Users/Jesse/Documents/Unity/Projects/Leviticus/Assets/Resources/Modules/weapons.json");
            #endif
            //var weaponsJsonText = File.ReadAllLines("Assets/Resources/Modules/weapons.json").Aggregate((sum,s) => sum + "\n" + s);
            var WeaponsJson = JsonConvert.DeserializeObject<WeaponsJson>(weaponsJsonText);

        }
    }
}
