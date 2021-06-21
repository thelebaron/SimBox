using Unity.Entities;

namespace Sandbox
{
    public class Resources
    {
        public struct Ore : IComponentData
        {
            public int Value;
            public int Capacity;
            
            public static readonly Ore Default = new Ore
            {
                Capacity = 50
            };
        }
        
        public struct Steel : IComponentData
        {
            public int ResourceValue;
            public int Capacity;

            public static readonly Steel Default = new Steel
            {
                Capacity = 50
            };
        }
    }
}