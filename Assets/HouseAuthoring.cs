
using System.Collections.Generic;
using Rules;
using Sandbox.Rules;
using Unity.Entities;
using UnityEngine;

namespace Sandbox
{
    public class HouseAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public int peopleIn = 1;
        public int rate = 10;
        public int apply = 1;
        public GameObject WorkerPrefab;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            conversionSystem.DeclareLinkedEntityGroup(gameObject);
        
            dstManager.AddComponentData(entity, new Resources.People{ Amount = 2, Capacity = 4});
            
            
            var ruleEntity = conversionSystem.CreateAdditionalEntity(gameObject);
            
                   
#if UNITY_EDITOR
            dstManager.SetName(ruleEntity, "CreateWorker");            
#endif
            dstManager.AddComponentData(ruleEntity, new Local.ResourceBin {Entity = entity});
            dstManager.AddChunkComponentData<Simulation>(ruleEntity);
            dstManager.AddComponentData(ruleEntity, new RuleEnabled());
            dstManager.AddComponentData(ruleEntity, new Apply {Value = apply});
            dstManager.AddComponentData(ruleEntity, new Rate {Value = rate});
            dstManager.AddComponentData(ruleEntity, new CreateWorker
            {
                Input = new CreateWorker.Inputs
                {
                    People = peopleIn
                },
                WorkerPrefab = conversionSystem.GetPrimaryEntity(WorkerPrefab)
            });
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(WorkerPrefab);
        }
    }
}


