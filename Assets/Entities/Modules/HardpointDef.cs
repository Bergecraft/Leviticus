using Assets.Modules;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities.Modules
{
    public class HardpointDef
    {
        public string name { get; set; }

        public Hardpoint.HardpointSize size { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(Hardpoint.HardpointType.none)]
        public Hardpoint.HardpointType type { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Vector3 position { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Vector3 rotation { get; set; }

        [JsonProperty(DefaultValueHandling=DefaultValueHandling.Ignore)]
        [DefaultValue(typeof(Vector3),"1,1,1")]
        public Vector3 scale { get; set; }
    }
}
