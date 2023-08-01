using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class ExampleECS_IJobParallelForTransform : MonoBehaviour
{
    [SerializeField] private Transform[] transforms;
    private TransformAccessArray _AccessArray;
    void Awake()
    {
        // Store the transforms inside a TransformAccessArray instance,
        // so that the transforms can be accessed inside a job.
        _AccessArray = new TransformAccessArray(transforms);
    }

    void Update()
    {
        NativeArray<Vector3> velocity = new NativeArray<Vector3>(transforms.Length, Allocator.Persistent);

        // Init data index velocity
        for (int i = 0; i < velocity.Length; i++)
        {
            velocity[i] = new Vector3(0f, 1f, 0f);
        }

        var job = new MovingIJobParallelForTransForm
        {
            velocity = velocity,
            detalTime = Time.deltaTime
        };

        // Schedule a parallel for  transform job.
        JobHandle jobHandle = job.Schedule(_AccessArray);

        // Job has Compelete
        jobHandle.Complete();


        // native arrays must be disposed manually
        velocity.Dispose();
    }
    private void OnDestroy()
    {
        // TransformAccessArrays must be disposed manually
        _AccessArray.Dispose();
    }
}
[BurstCompile]
public struct MovingIJobParallelForTransForm : IJobParallelForTransform
{
    [ReadOnly]
    public NativeArray<Vector3> velocity;
    public float detalTime;
    public void Execute(int index, TransformAccess transform)
    {
        // Move transform based on detal time and velocity
        var pos = transform.position;
        pos += velocity[index] * detalTime;
        transform.position = pos;
    }
}