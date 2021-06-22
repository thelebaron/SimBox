using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class PrefabSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject prefab;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var prefabSpawner = new PrefabSpawner {Prefab = conversionSystem.TryGetPrimaryEntity(prefab)};
        dstManager.AddComponentData(entity, prefabSpawner);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefab);
    }
}

public struct PrefabSpawner : IComponentData
{
    public Entity Prefab;
}


public class PrefabSpawnerSystem : SystemBase
{
    
    protected override void OnUpdate()
    {
        var pos = new float3 {x = Random.Range(-55, 55), y = 0, z = Random.Range(-55, 55)};
        
        Entities.ForEach((Entity entity, ref PrefabSpawner spawner) =>
        {
            var e = EntityManager.Instantiate(spawner.Prefab);
            EntityManager.SetComponentData(e, new Translation{Value = pos});
            
        }).WithStructuralChanges().Run();
    }
}