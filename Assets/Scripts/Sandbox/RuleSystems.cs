using Rules;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Sandbox
{

    [BurstCompile]
    public struct UnitRuleSystem : ISystemBase
    {
        public EntityQuery query;
        
        struct RuleJob : IJobChunk
        {
            public EntityTypeHandle EntityType;
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.Steel> SteelResource;
            public ComponentTypeHandle<Simulation> SimulationType;
            public ComponentTypeHandle<State> StateType;
            public ComponentTypeHandle<SteelRule> SteelRuleType;
            
            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var entities = chunk.GetNativeArray(EntityType);
                var simulation = chunk.GetChunkComponentData(SimulationType);
                var states = chunk.GetNativeArray(StateType);
                var steelRules = chunk.GetNativeArray(SteelRuleType);
                
                // if dont update
                if (!simulation.ShouldUpdate)
                    return;
                
                for (int i = 0; i < entities.Length; i++)
                {
                    var entity = entities[i];
                    var state = states[i];
                    var steelRule = steelRules[i];
                    
                    var bin = SteelResource[state.Unit];
                    if (steelRule.Consume > bin.ResourceValue)
                    {
                        state.Value = State.CurrentState.Fail;
                        continue;
                    }

                    state.Value = State.CurrentState.Success;
                    bin.ResourceValue -= steelRule.Consume;
                    bin.ResourceValue += steelRule.Produce;
                    
                    SteelResource[state.Unit] = bin;
                    states[i] = state;
                }
                
                // updated, so set false
                simulation.ShouldUpdate = false;
                chunk.SetChunkComponentData(SimulationType, simulation);

            }
        }

        public void OnCreate(ref SystemState state)
        {
            query = state.GetEntityQuery(
                typeof(Enabled),
                ComponentType.ChunkComponent<Simulation>(), 
                typeof(SteelRule),
                typeof(State)
                );
        }

        public void OnDestroy(ref SystemState state)
        {
            
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new RuleJob
            {
                EntityType = state.GetEntityTypeHandle(),
                SteelResource = state.GetComponentDataFromEntity<Resources.Steel>(),
                SimulationType = state.GetComponentTypeHandle<Simulation>(),
                StateType = state.GetComponentTypeHandle<State>(),
                SteelRuleType = state.GetComponentTypeHandle<SteelRule>()
            }.Schedule(query, state.Dependency);
        }
    }
}