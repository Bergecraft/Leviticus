using Assets.Entities.Modules;
using Assets.Entities.Modules.Reactors;
using Assets.Entities.Modules.Shields;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Serialization
{
    class ModuleDefConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ModuleDef).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new List<ModuleDef>();
            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read();

                while (reader.TokenType != JsonToken.EndArray)
                {

                    JObject item = JObject.Load(reader);
                    String TypeDefName = item["TypeDefName"].ToString();
                    ModuleDef value;
                    switch (TypeDefName)
                    {
                        case "ModuleDef":
                            value = item.ToObject<ModuleDef>();
                            break;
                        case "ShieldGeneratorDef":
                            value = item.ToObject<ShieldGeneratorDef>();
                            break;
                        case "ReactorDef":
                            value = item.ToObject<ReactorDef>();
                            break;
                        default:
                            value = item.ToObject<ModuleDef>();
                            break;
                    }
                    //result.Add((T)reader.Value);
                    result.Add(value);
                    reader.Read();
                }

                //return result;
            }
            return result.ToArray();
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
