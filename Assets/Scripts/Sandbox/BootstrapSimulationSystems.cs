using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
/*
public class BootstrapSimulationSystems : ICustomBootstrap
{
    public bool Initialize(string defaultWorldName)
    {
        Debug.Log("init");
        var world                             = World.DefaultGameObjectInjectionWorld;

        var initializationSystemGroup = world.simulationSystemGroup;
        var simulationSystemGroup     = world.simulationSystemGroup;
        var presentationSystemGroup   = world.presentationSystemGroup;
        var systems                   = new List<Type>(DefaultWorldInitialization.GetAllSystems(WorldSystemFilterFlags.Default));

        systems.RemoveSwapBack(typeof(LatiosInitializationSystemGroup));
        systems.RemoveSwapBack(typeof(LatiosSimulationSystemGroup));
        systems.RemoveSwapBack(typeof(LatiosPresentationSystemGroup));
        systems.RemoveSwapBack(typeof(InitializationSystemGroup));
        systems.RemoveSwapBack(typeof(SimulationSystemGroup));
        systems.RemoveSwapBack(typeof(PresentationSystemGroup));

        DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(world, systems);

        initializationSystemGroup.SortSystems();
        simulationSystemGroup.SortSystems();
        presentationSystemGroup.SortSystems();

        ScriptBehaviourUpdateOrder.AddWorldToCurrentPlayerLoop(world);
        return true;
    }
}*/