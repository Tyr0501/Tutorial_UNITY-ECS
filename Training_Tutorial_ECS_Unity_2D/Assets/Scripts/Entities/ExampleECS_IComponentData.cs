using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;

public class ExampleECS_IComponentData : MonoBehaviour
{

}


public struct TarrgetPosstionComponent : IComponentData
{
    public float3 postion;
}

public struct IEnemyComponent : IComponentData
{
    public float Health;
    public float Speed;
    public float Damage;
}

//[UpdateInGroup(typeof(SimulationSystemGroup))]
//public class EnemyMoveSystem : ComponentSystemGroup
//{
//    protected override void OnUpdate()
//    {
//        base.OnUpdate();
//    }
//}