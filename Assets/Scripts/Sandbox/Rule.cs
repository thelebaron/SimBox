using System;
using Unity.Entities;

namespace Rules
{
    public struct Enabled : IComponentData 
    {
        
    }

    public struct State : IComponentData
    {
        public enum CurrentState
        {
            Waiting, Update, Fail, Success
        }

        public Entity Unit;
        public CurrentState Value;
    }
    
    [Serializable]
    public struct Apply : IComponentData
    {
        public int Value;
    }
    
    [Serializable]
    public struct Rate : IComponentData
    {
        public int Value;
    }
    
    [Serializable]
    public struct SteelRule : IComponentData
    {
        public int Consume;
        public int Produce;
    }
    
    [Serializable]
    public struct Ore_Rule : IComponentData
    {
        public int In;
        public int Out;
    }
    
    
    
}