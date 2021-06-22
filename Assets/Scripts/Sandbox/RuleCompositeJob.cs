using Rules;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Sandbox
{
    struct CompositeRuleJob : IJobChunk
    {
        public EntityTypeHandle EntityType;
        [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.Wood> WoodResource;
        [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.People> PeopleResource;
        [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.Money> MoneyResource;
        [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.Steel> SteelResource;
        [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Resources.Ore> OreResource;
        
        public ComponentTypeHandle<Simulation> SimulationType;
        public ComponentTypeHandle<Rate> RateType;
        [ReadOnly] public ComponentTypeHandle<Apply> ApplyType;
        
        public ComponentTypeHandle<Local.ResourceBin> ResourceBinType;
        public ComponentTypeHandle<Local.WoodIn> WoodInType;
        public ComponentTypeHandle<Local.WoodOut> WoodOutType;
        public ComponentTypeHandle<Local.PeopleIn> PeopleInType;
        public ComponentTypeHandle<Local.PeopleOut> PeopleOutType;
        public ComponentTypeHandle<Local.MoneyIn> MoneyInType;
        public ComponentTypeHandle<Local.MoneyOut> MoneyOutType;
        public ComponentTypeHandle<Local.SteelIn> SteelInType;
        public ComponentTypeHandle<Local.SteelOut> SteelOutType;
        public ComponentTypeHandle<Local.OreIn> OreInType;
        public ComponentTypeHandle<Local.OreOut> OreOutType;
        
        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var entities = chunk.GetNativeArray(EntityType);
            var simulation = chunk.GetChunkComponentData(SimulationType);
            var rates = chunk.GetNativeArray(RateType);
            var applies = chunk.GetNativeArray(ApplyType);
            var resourceBins = chunk.GetNativeArray(ResourceBinType);

            var hasWoodIn = chunk.Has(WoodInType);
            var woodIns = chunk.GetNativeArray(WoodInType);
            var hasWoodOut = chunk.Has(WoodOutType);
            var woodOuts = chunk.GetNativeArray(WoodOutType);
            var hasPeopleIn = chunk.Has(PeopleInType);
            var peopleIns = chunk.GetNativeArray(PeopleInType);
            var hasPeopleOut = chunk.Has(PeopleOutType);
            var peopleOuts = chunk.GetNativeArray(PeopleOutType);
            var hasMoneyIn = chunk.Has(MoneyInType);
            var moneyIns = chunk.GetNativeArray(MoneyInType);
            var hasMoneyOut = chunk.Has(MoneyOutType);
            var moneyOuts = chunk.GetNativeArray(MoneyOutType);
            var hasSteelIn = chunk.Has(SteelInType);
            var steelIns = chunk.GetNativeArray(SteelInType);
            var hasSteelOut = chunk.Has(SteelOutType);
            var steelOuts = chunk.GetNativeArray(SteelOutType);
            var hasOreIn = chunk.Has(OreInType);
            var oreIns = chunk.GetNativeArray(OreInType);
            var hasOreOut = chunk.Has(OreOutType);
            var oreOuts = chunk.GetNativeArray(OreOutType);

            // if dont update
            if (!simulation.ShouldUpdate)
                return;
                
            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var rate = rates[i];
                var apply = applies[i];
                var resourceBin = resourceBins[i];

                if (!rate.ShouldRun())
                {
                    rates[i] = rate;
                    continue;
                }
                
                // All criteria must be met
                if (hasWoodIn)
                {
                    var woodIn = woodIns[i];
                    var woodOut = woodOuts[i];
                    var currentAmount = WoodResource[resourceBin.Entity].Amount;
                    var currentCapacity = WoodResource[resourceBin.Entity].Capacity;
                    if (woodIn > currentAmount || woodOut + currentAmount> currentCapacity)
                        continue;
                }
                
                if (hasPeopleIn)
                {
                    var peopleIn = peopleIns[i];
                    var peopleOut = peopleOuts[i];
                    var currentAmount = PeopleResource[resourceBin.Entity].Amount;
                    var currentCapacity = PeopleResource[resourceBin.Entity].Capacity;
                    if (peopleIn > currentAmount || peopleOut + currentAmount > currentCapacity)
                        continue;
                }
                
                if (hasMoneyIn)
                {
                    var moneyIn = moneyIns[i];
                    var moneyOut = moneyOuts[i];
                    var currentAmount = MoneyResource[resourceBin.Entity].Amount;
                    var currentCapacity = MoneyResource[resourceBin.Entity].Capacity;
                    if (moneyIn > currentAmount || moneyOut + currentAmount > currentCapacity)
                        continue;
                }
                
                if (hasSteelIn)
                {
                    var steelIn = steelIns[i];
                    var steelOut = steelOuts[i];
                    var currentAmount = SteelResource[resourceBin.Entity].Amount;
                    var currentCapacity = SteelResource[resourceBin.Entity].Capacity;
                    if (steelIn > currentAmount || steelOut + currentAmount > currentCapacity)
                        continue;
                }
                
                if (hasOreIn)
                {
                    var oreIn = oreIns[i];
                    var oreOut = oreOuts[i];
                    var currentAmount = OreResource[resourceBin.Entity].Amount;
                    var currentCapacity = OreResource[resourceBin.Entity].Capacity;
                    if (oreIn > currentAmount || oreOut + currentAmount > currentCapacity)
                        continue;
                }
	
                
                // if all criteria met, modify resource bin data
                if (hasWoodOut)
                {
                    var woodOut = woodOuts[i];
                    var wood = WoodResource[resourceBin.Entity];
                    wood.Amount += woodOut;
                    wood.Amount = math.clamp(wood.Amount, wood.Capacity, wood.Amount);
                }
                
                if (hasPeopleOut)
                {
                    var peopleOut = peopleOuts[i];
                    var people = PeopleResource[resourceBin.Entity];
                    people.Amount += peopleOut;
                    people.Amount = math.clamp(people.Amount, people.Capacity, people.Amount);
                }
                
                if (hasMoneyOut)
                {
                    var moneyOut = moneyOuts[i];
                    var money = MoneyResource[resourceBin.Entity];
                    money.Amount += moneyOut;
                    money.Amount = math.clamp(money.Amount, money.Capacity, money.Amount);
                }                
                
                if (hasSteelOut)
                {
                    var steelOut = steelOuts[i];
                    var steel = SteelResource[resourceBin.Entity];
                    steel.Amount += steelOut;
                    steel.Amount = math.clamp(steel.Amount, steel.Capacity, steel.Amount);
                }

                if (hasOreOut)
                {
                    var oreOut = oreOuts[i];
                    var ore = OreResource[resourceBin.Entity];
                    ore.Amount += oreOut;
                    ore.Amount = math.clamp(ore.Amount, ore.Capacity, ore.Amount);
                }

                rates[i] = rate;
            }
                
            // updated, so set false
            simulation.ShouldUpdate = false;
            chunk.SetChunkComponentData(SimulationType, simulation);

        }
    }
}