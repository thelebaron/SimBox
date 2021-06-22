using System;
using Unity.Entities;

namespace Rules
{
    public struct RuleEnabled : IComponentData 
    {
        
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
        public int InternalValue;

        public bool ShouldRun()
        {
            InternalValue++;
            if (Value.Equals(InternalValue))
            {
                InternalValue = 0;
                return true;
            }

            return false;
        }
    }
}