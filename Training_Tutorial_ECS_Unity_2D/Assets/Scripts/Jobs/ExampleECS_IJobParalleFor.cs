using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ExampleECS_IJobParalleFor : MonoBehaviour
{
    private NativeArray<int> myArray;
    private NativeArray<int> arr;

    private void Start()
    {
        // Kh?i t?o m?ng v� ?i?n gi� tr? v�o ch�ng (?�y ch? l� v� d?)
        int arraySize = 100000;
        myArray = new NativeArray<int>(arraySize, Allocator.Persistent);
        arr = new NativeArray<int>(arraySize, Allocator.Persistent);

        // ?i?n gi� tr? v�o m?ng (?�y ch? l� v� d?, b?n c� th? nh?p gi� tr? b?t k?)
        for (int i = 0; i < arraySize; i++)
        {
            myArray[i] = i + 1; // V� d?: G�n gi� tr? t? 1 ??n 100 cho myArray
            arr[i] = i * 2;     // V� d?: G�n gi� tr? b?ng g?p ?�i ch? s? cho arr
        }
    }

    private void OnDestroy()
    {
        // H�y nh? gi?i ph�ng b? nh? khi kh�ng s? d?ng n?a
        myArray.Dispose();
        arr.Dispose();
    }


    private void Update()
    {
        // Instantiate the job
        var job = new SquaresJobParallelFor { NumsIJobParleFor = myArray };

        // schedule and complete the job
        int batchCount = 100;
        JobHandle handle = job.Schedule(arr.Length, batchCount);

        handle.Complete();

    }
}

[BurstCompile]
public struct SquaresJobParallelFor : IJobParallelFor
{
    public NativeArray<int> NumsIJobParleFor;
    public void Execute(int index)
    {

        NumsIJobParleFor[index] *= NumsIJobParleFor[index];
    }
}
