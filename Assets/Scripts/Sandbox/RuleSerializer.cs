using System;
using System.IO;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Sandbox
{
    
    
    /*
     * unitRule CreateAlloy
     * rate 2
     * applyCount 1
     * local Ore in 10
     * local Alloy out 15
     * end
     */
    
    // 
    /*
           Entity as Resource
      Entity 21
      Ore Component { Value = 100 }
      
      Entity 1
      Ore Component { Value = 1 }
      Steel Component { Value = 50 }
      
           Entity as Unit
      Ore Component { Value = 101 }
      Steel Component { Value = 50 }
      Ore Rule { In = 10 }
      Steel Rule { Out = 2 }
      Currency Rule { Out = 2, Global = true }
      Rate  { Value = 2 } // every 2 sim ticks
      Apply  { Value = 1 }
      
      
     */


    public struct Enabled : IComponentData 
    {
        
    }
    
    [Serializable]
    public class Rule
    {
        public FixedString32 StringValue;
        public Entity EntityValue;
        public int NumberValue;
        public float3 Float3Value;
    }
    
    [ExecuteAlways]
    public class RuleSerializer : MonoBehaviour
    {
        public bool Save;

        private void Update()
        {
            if (!Save)
            {
                return;
            }

            var rule = new Rule
            {
                StringValue = "Entity",
                EntityValue = Entity.Null,
                NumberValue = 31,
                Float3Value = new float3(1,0,1)
            };
                
            
            
            var saveLocation = Application.persistentDataPath + "\\" + "Saves";

            if (!Directory.Exists(saveLocation))
                Directory.CreateDirectory(saveLocation);
            
            var path     = saveLocation + "\\" + "Rule.json";
            var jsondata = JsonUtility.ToJson(rule, true);
            File.WriteAllText(path, jsondata);
            
            Save = false;
        }
    }
}