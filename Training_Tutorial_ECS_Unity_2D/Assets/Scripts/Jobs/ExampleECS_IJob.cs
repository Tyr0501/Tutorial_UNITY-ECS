using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ExampleECS_IJob : MonoBehaviour
{
    private List<int> myArrays;
    // Update is called once per frame
    void Update()
    {
        // ... somewhere in main thread code
        Job();
        // or
        OtherJob();
    }

    private void Job()
    {
        NativeArray<int> myArrays = new NativeArray<int>(10, Allocator.Persistent);

        // instantiate the job
        var job = new SquaresJob { Nums = myArrays };
        // Find more work it can pull this Job Off the quere 
        JobHandle jobHandle = job.Schedule();
        // affter ensure complete job before end Update(), avoid error 
        jobHandle.Complete();
    }

    private void OtherJob()
    {
        NativeArray<int> myArrays = new NativeArray<int>(10, Allocator.Persistent);

        // Create Two instantiate the job
        var job = new SquaresJob { Nums = myArrays };
        var otherJob = new SquaresJob { Nums = myArrays };

        // Ok: this second job depnds upon the first
        JobHandle jobHandle = job.Schedule();
        JobHandle otherJobHandle = otherJob.Schedule(jobHandle);

        jobHandle.Complete();
        otherJobHandle.Complete();

    }
}

[BurstCompile]
public struct SquaresJob : IJob
{
    public NativeArray<int> Nums;
    public void Execute()
    {
        for (int i = 0; i < Nums.Length; i++)
        {
            Nums[i] *= Nums[i];
        }
    }
}