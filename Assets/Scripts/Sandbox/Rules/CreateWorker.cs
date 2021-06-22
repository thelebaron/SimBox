using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Sandbox.Rules
{
    public struct CreateWorker : IComponentData
    {
        public Inputs Input;
        public Entity WorkerPrefab;
        
        public struct Inputs : ISimOutputs
        {
            public int People;
        }

        public struct Data : IData
        {
            public float3 Translation;
            public Entity Entity;
            public ComponentDataFromEntity<Resources.People> People;
            public int JobIndex;
            public EntityCommandBuffer.ParallelWriter EntityCommandBuffer;
        }
        
        public void Evaluate(ref Data data)
        {
            var entity = data.Entity;
            var people = data.People[entity];
            
            // Fail if criteria not met 
            if (Input.People > people.Amount)
                return;
            
            people.Amount -= Input.People;
            
            //var e = data.EntityCommandBuffer.Instantiate(data.JobIndex, WorkerPrefab);

            //var tr = data.Translation;
            //data.EntityCommandBuffer.SetComponent(data.JobIndex, e, new Translation{Value = new float3(3,0,3)});
            
            data.People[entity] = people;
        }


    }
}