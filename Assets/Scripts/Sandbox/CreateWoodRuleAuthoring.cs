using Rules;
using Unity.Entities;
using UnityEngine;

namespace Sandbox
{
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
            
            
            var ruleEntity = conversionSystem.CreateAdditionalEntity(gameObject);
            if (dstManager.HasComponent<LinkedEntityGroup>(entity))
            {
                var linkedEntityGroup = dstManager.GetBuffer<LinkedEntityGroup>(entity);
                linkedEntityGroup.Add(new LinkedEntityGroup {Value = ruleEntity});
            }
            else
            {
                dstManager.AddBuffer<LinkedEntityGroup>(entity);
                var linkedEntityGroup = dstManager.GetBuffer<LinkedEntityGroup>(entity);
                linkedEntityGroup.Add(new LinkedEntityGroup {Value = ruleEntity});
            }
            
#if UNITY_EDITOR
            dstManager.SetName(ruleEntity, "CreateWoodRule");            
#endif
            dstManager.AddComponentData(ruleEntity, new Local.ResourceBin {Entity = entity});
            dstManager.AddChunkComponentData<Simulation>(ruleEntity);
            dstManager.AddComponentData(ruleEntity, new RuleEnabled());
            dstManager.AddComponentData(ruleEntity, new Apply {Value = apply});
            dstManager.AddComponentData(ruleEntity, new Rate {Value = rate});
            dstManager.AddComponentData(ruleEntity, new CreateWoodRule
            {
                Input = new CreateWoodRule.Inputs
                {
                    People = peopleIn,
                    Money = moneyIn
                },
                Output = new CreateWoodRule.Outputs
                {
                    People = peopleOut,
                    Wood = woodOut
                }
            });
        }
    }
}