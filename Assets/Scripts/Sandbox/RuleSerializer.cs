using System;
using System.IO;
using Rules;
using Unity.Collections;
using Unity.Entities;
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
      
      
           Entity as Rule
      
      Ore Rule { In = 10 }
      Steel Rule { Out = 2 }
      Currency Rule { Out = 2, Global = true }
      Rate  { Value = 2 } // every 2 sim ticks
      Apply  { Value = 1 }
      
      
     */

    struct Rule
    {
        public FixedList32<SteelRule> inList;
    }

    


    
    // this is a very brittle setup, need flexibility
    [Serializable]
    public struct UnitTestRule
    {
        public int SteelBin;
        public SteelRule steelRule;
        public float Rate;
        public int Apply;
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

            var rule = new UnitTestRule
            {
                SteelBin = 31,
                steelRule = new SteelRule
                {
                    Consume    = 1,
                    Produce   = 0
                },
                Rate = 2,
                Apply = 1
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