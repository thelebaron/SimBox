using Rules;
using Sandbox.Rules;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Sandbox
{
    public class WoodMillUnitSystem : SystemBase
    {
        private EntityQuery woodMillQuery;
        private EntityQuery houseQuery;
        private EndSimulationEntityCommandBufferSystem CommandBufferSystem;
        
        [BurstCompile]
        struct WoodMillUnitJob : IJobChunk
        {
            [ReadOnly] public EntityTypeHandle EntityType;
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.Wood> WoodResource;
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.People> PeopleResource;
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.Money> MoneyResource;
            public ComponentTypeHandle<Simulation> SimulationType;
            [ReadOnly] public ComponentTypeHandle<CreateWood> CreateWoodRuleType;
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
                        var data = new CreateWood.Data
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

        [BurstCompile]
        struct HouseUnitJob : IJobChunk
        {
            public EntityCommandBuffer.ParallelWriter EntityCommandBuffer;
            [ReadOnly] public EntityTypeHandle EntityType;
            [ReadOnly] public ComponentDataFromEntity<Translation> TranslationData;
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.People> PeopleResource;
            public ComponentTypeHandle<Simulation> SimulationType;
            [ReadOnly] public ComponentTypeHandle<CreateWorker> CreateWorkerType;
            public ComponentTypeHandle<Apply> ApplyType;
            public ComponentTypeHandle<Rate> RateType;
            public ComponentTypeHandle<Local.ResourceBin> ResourceBinType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var entities = chunk.GetNativeArray(EntityType);
                var simulation = chunk.GetChunkComponentData(SimulationType);
                var createWorkers = chunk.GetNativeArray(CreateWorkerType);
                var applies = chunk.GetNativeArray(ApplyType);
                var rates = chunk.GetNativeArray(RateType);
                var resourceBins = chunk.GetNativeArray(ResourceBinType);
                
                // if dont update
                if (!simulation.ShouldUpdate)
                    return;
                
                for (int i = 0; i < entities.Length; i++)
                {
                    var entity = entities[i];
                    var createWorker = createWorkers[i];
                    var rate = rates[i];
                    var unit = resourceBins[i].Entity;
                    var translation = TranslationData[unit];

                    
                    if (rate.ShouldRun())
                    {
                        var data = new CreateWorker.Data
                        {
                            Translation = translation.Value,
                            Entity = unit,
                            People = PeopleResource,
                            JobIndex = chunkIndex,
                            EntityCommandBuffer = EntityCommandBuffer
                        };
                        
                        createWorker.Evaluate(ref data);
                        
                        var e = EntityCommandBuffer.Instantiate(chunkIndex, createWorker.WorkerPrefab);
                        EntityCommandBuffer.SetComponent(chunkIndex, e, new Translation{Value = translation.Value});

                        //
                        
                        
                    }

                    rates[i] = rate;
                }
                
                // updated, so set false
                simulation.ShouldUpdate = false;
                chunk.SetChunkComponentData(SimulationType, simulation);
            }
        }
        
        protected override void OnCreate()
        {
            woodMillQuery = GetEntityQuery(ComponentType.ChunkComponent<Simulation>(), 
                typeof(RuleEnabled), typeof(CreateWood), typeof(Rate), typeof(Apply)
                );
            
            houseQuery = GetEntityQuery(ComponentType.ChunkComponent<Simulation>(), 
                typeof(RuleEnabled), typeof(CreateWorker), typeof(Rate), typeof(Apply)
            );
            CommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }


        protected override void OnUpdate()
        {
            Dependency = new HouseUnitJob
            {
                EntityCommandBuffer = CommandBufferSystem.CreateCommandBuffer().AsParallelWriter(),
                EntityType = GetEntityTypeHandle(),
                TranslationData = GetComponentDataFromEntity<Translation>(true),
                PeopleResource = GetComponentDataFromEntity<Resources.People>(),
                SimulationType = GetComponentTypeHandle<Simulation>(),
                CreateWorkerType = GetComponentTypeHandle<CreateWorker>(true),
                ApplyType = GetComponentTypeHandle<Apply>(),
                RateType = GetComponentTypeHandle<Rate>(),
                ResourceBinType = GetComponentTypeHandle<Local.ResourceBin>(),
            }.Schedule(houseQuery, Dependency);
            CommandBufferSystem.AddJobHandleForProducer(Dependency);
            
            Dependency = new WoodMillUnitJob
            {
                EntityType = GetEntityTypeHandle(),
                WoodResource = GetComponentDataFromEntity<Resources.Wood>(),
                MoneyResource = GetComponentDataFromEntity<Resources.Money>(),
                PeopleResource = GetComponentDataFromEntity<Resources.People>(),
                SimulationType = GetComponentTypeHandle<Simulation>(),
                ApplyType = GetComponentTypeHandle<Apply>(),
                RateType = GetComponentTypeHandle<Rate>(),
                ResourceBinType = GetComponentTypeHandle<Local.ResourceBin>(),
                CreateWoodRuleType = GetComponentTypeHandle<CreateWood>(true)
            }.Schedule(woodMillQuery, Dependency);
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
