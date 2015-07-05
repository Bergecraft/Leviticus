using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using UnityEngine;
using Assets;
using Assets.Serialization;
using System.IO;
using System.Linq;

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
        [TestMethod]
        public void TestRCS()
        {
            AssertDots(1, Vector3.up, 0, 0, 0, 1);
            AssertDots(-1, Vector3.up, 0, 1, 0, 0);
            AssertDots(1, new Vector3(1, 1, 0), Mathf.Sqrt(2)/2, 0, 0, Mathf.Sqrt(2)/2);
        }
        private void AssertDots(float torque, Vector3 position, params float[] expected)
        {
            var perp = CalculatePerp(position, torque);
            var exhaust = new Vector3[] { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
            var dots = exhaust.Select(v => Mathf.Max(Vector3.Dot(perp, v), 0)).ToArray();

            foreach (var dot in dots)
            {
                Console.Write(dot + " ");
            }
            Console.WriteLine();
            CollectionAssert.AreEqual(dots, expected);
        }
        private Vector3 CalculatePerp(Vector3 position, float direction)
        {
            if (direction > 0)
            {
                return Vector3.Cross(Vector3.forward, position).normalized;
            }
            else
            {
                return Vector3.Cross(position, Vector3.forward).normalized;
            }
        }
    }
}
