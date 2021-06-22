using Unity.Entities;
using UnityEngine;

namespace Sandbox
{
    public struct ChainedRule : IComponentData
    {
    }

    [ConverterVersion("thelebaron", 1)]
    [UpdateAfter(typeof(UnitConversionSystem))]
    public class RuleConversionSystem : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((CreateWoodRuleAuthoring authoring) =>
            {
                var entity = GetPrimaryEntity(authoring);
                
                // Is this just a rule
                if (!DstEntityManager.HasComponent<Unit>(entity))
                {
                    DstEntityManager.AddComponentData(entity, new ChainedRule());
                    DstEntityManager.AddChunkComponentData<Simulation>(entity);
                }
                
                
            });
        }
    }
}