
using Rules;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Sandbox
{

    [BurstCompile]
    public struct WoodMillUnitSystem : ISystemBase
    {
        public EntityQuery query;
        
        struct WoodMillUnitJob : IJobChunk
        {
            public EntityTypeHandle EntityType;
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.Wood> WoodResource;
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.People> PeopleResource;
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.Money> MoneyResource;
            public ComponentTypeHandle<Simulation> SimulationType;
            [ReadOnly] public ComponentTypeHandle<CreateWoodRule> CreateWoodRuleType;
            public ComponentTypeHandle<Apply> ApplyType;
            public ComponentTypeHandle<Rate> RateType;
            public ComponentTypeHandle<Local.ResourceBin> ResourceBinType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var entities = chunk.GetNativeArray(EntityType);
                var simulation = chunk.GetChunkComponentData(SimulationType);
                var createWoodRules = chunk.GetNativeArray(CreateWoodRuleType);
                var applies = chunk.GetNativeArray(ApplyType);
                var rates = chunk.GetNativeArray(RateType);
                var resourceBins = chunk.GetNativeArray(ResourceBinType);
                
                // if dont update
                if (!simulation.ShouldUpdate)
                    return;
                
                for (int i = 0; i < entities.Length; i++)
                {
                    var entity = entities[i];
                    var createWoodRule = createWoodRules[i];
                    var rate = rates[i];
                    var unit = resourceBins[i].Entity;

                    if (rate.ShouldRun())
                    {
                        var data = new CreateWoodRule.Data
                        {
                            Entity = unit,
                            Money = MoneyResource,
                            People = PeopleResource,
                            Wood =   WoodResource
                        };
                
                        createWoodRule.Evaluate(ref data);
                    }

                    rates[i] = rate;
                }
                
                // updated, so set false
                simulation.ShouldUpdate = false;
                chunk.SetChunkComponentData(SimulationType, simulation);

            }
        }

        public void OnCreate(ref SystemState state)
        {
            query = state.GetEntityQuery(
                ComponentType.ChunkComponent<Simulation>(), 
                typeof(RuleEnabled), typeof(CreateWoodRule), typeof(Rate), typeof(Apply)
                );
        }

        public void OnDestroy(ref SystemState state)
        {
            
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new WoodMillUnitJob
            {
                EntityType = state.GetEntityTypeHandle(),
                WoodResource = state.GetComponentDataFromEntity<Resources.Wood>(),
                MoneyResource = state.GetComponentDataFromEntity<Resources.Money>(),
                PeopleResource = state.GetComponentDataFromEntity<Resources.People>(),
                SimulationType = state.GetComponentTypeHandle<Simulation>(),
                ApplyType = state.GetComponentTypeHandle<Apply>(),
                RateType = state.GetComponentTypeHandle<Rate>(),
                ResourceBinType = state.GetComponentTypeHandle<Local.ResourceBin>(),
                CreateWoodRuleType = state.GetComponentTypeHandle<CreateWoodRule>(true)
            }.Schedule(query, state.Dependency);
        }
    }
}


public interface IRule
    {
        void OnEvaluate(ref RuleState state);
        void OnUpdate(ref RuleState state);
        void OnSuccess(ref RuleState state);
        void OnFail(ref RuleState state);
    }

    public unsafe struct RuleState
    {

    }
    
    

    public interface ISimInputs
    {
    }
    public interface ISimOutputs
    {
    }
    
    public interface ISuccess<TDataFromEntity> where TDataFromEntity : IData
    {
        void OnFail(ref TDataFromEntity resources);
    }
    
    public interface IFail<TDataFromEntity> where TDataFromEntity : IData
    {
        void OnFail(ref TDataFromEntity resources);
    }
    
    public interface IExecute<TBucketData> : ISimulationRule where TBucketData : IData
    {
        void Evaluate(ref TBucketData resources);
    }

    public interface IUpdate<TBucketData> : ISimulationRule where TBucketData : IData
    {
        int Tick { get; set; }
        void Update(ref TBucketData resources);
    }
    
    public interface IData
    {
    }

    public interface ISimulationRule
    {
        
    }
