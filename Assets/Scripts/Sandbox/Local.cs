using Unity.Entities;

namespace Sandbox
{
    public static class Local
    {
        public struct ResourceBin : IComponentData
        {
            public Entity Entity;
        }
        
        public struct MoneyIn : IComponentData
        {
            public int Value;
            
            public static implicit operator int(MoneyIn e) { return e.Value; }
            public static implicit operator MoneyIn(int e) { return new MoneyIn { Value = e }; }
        }
        
        public struct MoneyOut : IComponentData
        {
            public int Value;
            
            public static implicit operator int(MoneyOut e) { return e.Value; }
            public static implicit operator MoneyOut(int e) { return new MoneyOut { Value = e }; }
        }
        
        public struct OreIn : IComponentData
        {
            public int Value;
            
            public static implicit operator int(OreIn e) { return e.Value; }
            public static implicit operator OreIn(int e) { return new OreIn { Value = e }; }
        }
        
        public struct OreOut : IComponentData
        {
            public int Value;
            
            public static implicit operator int(OreOut e) { return e.Value; }
            public static implicit operator OreOut(int e) { return new OreOut { Value = e }; }
        }
        
        public struct SteelIn : IComponentData
        {
            public int Value;
            
            public static implicit operator int(SteelIn e) { return e.Value; }
            public static implicit operator SteelIn(int e) { return new SteelIn { Value = e }; }
        }
        
        public struct SteelOut : IComponentData
        {
            public int Value;
            
            public static implicit operator int(SteelOut e) { return e.Value; }
            public static implicit operator SteelOut(int e) { return new SteelOut { Value = e }; }
        }
        
        public struct PeopleIn : IComponentData
        {
            public int Value;
            
            public static implicit operator int(PeopleIn e) { return e.Value; }
            public static implicit operator PeopleIn(int e) { return new PeopleIn { Value = e }; }
        }
        
        public struct PeopleOut : IComponentData
        {
            public int Value;
            
            public static implicit operator int(PeopleOut e) { return e.Value; }
            public static implicit operator PeopleOut(int e) { return new PeopleOut { Value = e }; }
        }
        
        public struct WoodIn : IComponentData
        {
            public int Value;
            
            public static implicit operator int(WoodIn e) { return e.Value; }
            public static implicit operator WoodIn(int e) { return new WoodIn { Value = e }; }
        }
        
        public struct WoodOut : IComponentData
        {
            public int Value;
            
            public static implicit operator int(WoodOut e) { return e.Value; }
            public static implicit operator WoodOut(int e) { return new WoodOut { Value = e }; }
        }
    }
}