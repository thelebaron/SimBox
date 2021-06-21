using Rules;
using Unity.Entities;
using UnityEngine;
using static Sandbox.RuleConversionUtility;
namespace Sandbox
{
    public class UnitAuthoring : UnitBaseAuthoring, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var linkedEntityGroup = dstManager.AddBuffer<LinkedEntityGroup>(entity);
            linkedEntityGroup.Add(new LinkedEntityGroup {Value = entity});
            
            dstManager.AddComponentData(entity, Resources.Ore.Default);
            dstManager.AddComponentData(entity, Resources.Steel.Default);
            
            CoreRuleComponents(gameObject, out var produceSteelRule, entity, dstManager, conversionSystem, 
                "ProduceSteelRule");
            
            dstManager.AddComponentData(produceSteelRule, new Ore_Rule {In = 2});
            dstManager.AddComponentData(produceSteelRule, new SteelRule {Produce = 1});
            
            
        }
    }

    public static class RuleConversionUtility
    {
        public static void CoreRuleComponents(GameObject gameObject, out Entity rule, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, string ruleName = "Rule")
        {
            rule = conversionSystem.CreateAdditionalEntity(gameObject);
            dstManager.AddChunkComponentData<Simulation>(rule);

            if (dstManager.HasComponent<LinkedEntityGroup>(entity))
            {
                var linkedEntityGroup = dstManager.GetBuffer<LinkedEntityGroup>(entity);
                linkedEntityGroup.Add(new LinkedEntityGroup {Value = rule});
            }

            if (!dstManager.HasComponent<LinkedEntityGroup>(entity))
            {
                dstManager.AddBuffer<LinkedEntityGroup>(entity);
                var linkedEntityGroup = dstManager.GetBuffer<LinkedEntityGroup>(entity);
                linkedEntityGroup.Add(new LinkedEntityGroup {Value = rule});
            }
            
            #if UNITY_EDITOR
            dstManager.SetName(rule, ruleName);            
            #endif
            
            dstManager.AddComponentData(rule, new Enabled());
            dstManager.AddComponentData(rule, new State{ Unit = entity });
            dstManager.AddComponentData(rule, new Apply {Value = 1});
            dstManager.AddComponentData(rule, new Rate {Value = 2});
        }
    }
    
    public abstract class UnitBaseAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

        }
    }
}