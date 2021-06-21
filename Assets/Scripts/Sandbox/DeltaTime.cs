using Unity.Burst;
using Unity.Entities;

namespace Sandbox
{
    public struct Simulation : IComponentData
    {
        public float DeltaTime;
        public int Tick;
        public bool ShouldUpdate;
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class UnitTickSystem : SystemBase
    {
        public EntityQuery query;

        public void OnCreate(ref SystemState state)
        {
            query = state.GetEntityQuery(typeof(Simulation));
        }

        [BurstCompile]
        private struct FinishUpdateJob : IJobChunk
        {
            public ComponentTypeHandle<Simulation> SimulationType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var simulation = chunk.GetChunkComponentData(SimulationType);
                simulation.ShouldUpdate = false;
                chunk.SetChunkComponentData(SimulationType, simulation);
            }
        }

        protected override void OnUpdate()
        {
            Dependency = new FinishUpdateJob
            {
                SimulationType = GetComponentTypeHandle<Simulation>()
            }.Schedule(query, Dependency);
        }
    }

    public struct Tick : IComponentData
    {
        public int Value;
    }

    public struct DeltaTime : IComponentData
    {
        public float Value;
    }

    [BurstCompile]
    public struct DeltaTimeSystem : ISystemBase
    {
        public EntityQuery query;

        public void OnCreate(ref SystemState state)
        {
            query = state.GetEntityQuery(ComponentType.ChunkComponent(typeof(Simulation)));
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        private struct UpdateTimeJob : IJobChunk
        {
            public float DeltaTime;
            public ComponentTypeHandle<Simulation> SimulationType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var simulation = chunk.GetChunkComponentData(SimulationType);

                simulation.DeltaTime += DeltaTime;
                if (simulation.DeltaTime >= 1)
                {
                    simulation.Tick++;
                    simulation.DeltaTime = 0;
                    simulation.ShouldUpdate = true;
                }

                chunk.SetChunkComponentData(SimulationType, simulation);
            }
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new UpdateTimeJob
            {
                DeltaTime = state.Time.DeltaTime,
                SimulationType = state.GetComponentTypeHandle<Simulation>()
            }.ScheduleSingle(query, state.Dependency);
        }
    }
    
}