using System;
using Unity.Entities;
using UnityEngine;

namespace Sandbox
{

    [Serializable]
    public struct CreateWoodRule : IComponentData
    {
        public Inputs Input;
        public Outputs Output;
        
        public struct Inputs : ISimInputs
        {
            public int People;
            public int Money;
        }
        
        public struct Outputs : ISimOutputs
        {
            public int People;
            public int Wood;
        }
        
        public struct Data : IData
        {
            public Entity Entity;
            public ComponentDataFromEntity<Resources.Money> Money;
            public ComponentDataFromEntity<Resources.People> People;
            public ComponentDataFromEntity<Resources.Wood> Wood;
        }
        
        public void Evaluate(ref Data data)
        {
            var entity = data.Entity;
            var people = data.People[entity];
            var wood = data.Wood[entity];
            var money = data.Money[entity];
            
            // Fail if criteria not met 
            if (Input.People > people.Amount || 
                Input.People + people.Amount > people.Capacity || 
                Input.Money > money.Amount ||
                Input.Money + money.Amount > money.Capacity ||
                Output.Wood + wood.Amount > wood.Capacity
                )
            {
                OnFail(ref data);
                return;
            }

            people.Amount -= Input.People;
            money.Amount -= Input.Money;
            
            people.Amount  += Output.People;
            wood.Amount += Output.Wood;

            data.People[entity] = people;
            data.Wood[entity] = wood;
            data.Money[entity] = money;

            OnSuccess(ref data);
        }

        public void OnFail(ref Data data)
        {
            // create unhappiness?
        }
        
        public void OnSuccess(ref Data data)
        {
            // maybe make smoke?
        }

        /// <summary>
        /// Conversion
        /// </summary>
        public static void Conversion(Entity unitEntity, EntityManager dstManager,
            GameObjectConversionSystem conversionSystem, GameObject gameObject)
        {
            
        }
    }
    
}