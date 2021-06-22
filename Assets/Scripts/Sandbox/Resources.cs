using Unity.Entities;

namespace Sandbox
{
    public static class Resources
    {
        public struct Money : IComponentData
        {
            public int Amount;
            public int Capacity;
            
            public static readonly Money Default = new Money
            {
                Amount = 50,
                Capacity = 500
            };
        }
        
        public struct People : IComponentData
        {
            public int Amount;
            public int Capacity;
            
            public static readonly People Default = new People
            {
                Amount = 2,
                Capacity = 50
            };
        }
        
        public struct Wood : IComponentData
        {
            public int Amount;
            public int Capacity;
            
            public static readonly Wood Default = new Wood
            {
                Capacity = 12
            };
        }
        
        public struct Ore : IComponentData
        {
            public int Amount;
            public int Capacity;
            
            public static readonly Ore Default = new Ore
            {
                Capacity = 50
            };
        }
        
        public struct Steel : IComponentData
        {
            public int Amount;
            public int Capacity;

            public static readonly Steel Default = new Steel
            {
                Capacity = 50
            };
        }
    }
}