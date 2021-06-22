using Rules;
using Sandbox.Rules;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Sandbox
{
    [UpdateAfter(typeof(GameObjectConversionSystem))]
    public class CreateWoodRuleAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public int peopleIn = 2;
        public int moneyIn = 1;
        public int peopleOut = 2;
        public int woodOut = 1;
        public int apply = 1;
        public int rate = 2;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, Resources.Wood.Default);
            dstManager.AddComponentData(entity, Resources.People.Default);
            dstManager.AddComponentData(entity, Resources.Money.Default);
            
            conversionSystem.DeclareLinkedEntityGroup(gameObject);
            
            var ruleEntity = conversionSystem.CreateAdditionalEntity(gameObject);
            //var linkedEntityGroup = dstManager.GetBuffer<LinkedEntityGroup>(entity);
            //linkedEntityGroup.Add(new LinkedEntityGroup {Value = entity});
            //linkedEntityGroup.Add(new LinkedEntityGroup {Value = ruleEntity});
            
            /*foreach (Transform child in transform)
            {
                linkedEntityGroup.Add(new LinkedEntityGroup {Value = conversionSystem.TryGetPrimaryEntity(child)});
            }*/
            
#if UNITY_EDITOR
            dstManager.SetName(ruleEntity, "CreateWoodRule");            
#endif
            dstManager.AddComponentData(ruleEntity, new Local.ResourceBin {Entity = entity});
            dstManager.AddChunkComponentData<Simulation>(ruleEntity);
            dstManager.AddComponentData(ruleEntity, new RuleEnabled());
            dstManager.AddComponentData(ruleEntity, new Apply {Value = apply});
            dstManager.AddComponentData(ruleEntity, new Rate {Value = rate});
            dstManager.AddComponentData(ruleEntity, new CreateWood
            {
                Input = new CreateWood.Inputs
                {
                    People = peopleIn,
                    Money = moneyIn
                },
                Output = new CreateWood.Outputs
                {
                    People = peopleOut,
                    Wood = woodOut
                }
            });
        }
    }
}