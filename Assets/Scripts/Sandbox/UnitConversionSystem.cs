using Unity.Entities;

namespace Sandbox
{

    [ConverterVersion("thelebaron", 1)]
    public class UnitConversionSystem : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((UnitAuthoring authoring) =>
            {
                var entity = GetPrimaryEntity(authoring);
                DstEntityManager.AddComponentData(entity, new Unit());
            });
        }
    }

    public struct Unit : IComponentData
    {
        
    }
}