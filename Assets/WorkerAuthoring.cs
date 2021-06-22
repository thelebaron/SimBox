using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Agent : IComponentData
{
    public float3 Direction;
}
public class WorkerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new Agent());
    }
}

public class AgentSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;
        Entities.ForEach((Entity entity, ref Agent agent, ref Translation tranlation) =>
        {
            if (agent.Direction.Equals(float3.zero))
            {
                agent.Direction = Random.insideUnitSphere * tranlation.Value;
                agent.Direction.y = 0;
            }

            tranlation.Value += agent.Direction * dt;

        }).Run();
    }
}