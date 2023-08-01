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
        // Kh?i t?o m?ng và ?i?n giá tr? vào chúng (?ây ch? là ví d?)
        int arraySize = 100000;
        myArray = new NativeArray<int>(arraySize, Allocator.Persistent);
        arr = new NativeArray<int>(arraySize, Allocator.Persistent);

        // ?i?n giá tr? vào m?ng (?ây ch? là ví d?, b?n có th? nh?p giá tr? b?t k?)
        for (int i = 0; i < arraySize; i++)
        {
            myArray[i] = i + 1; // Ví d?: Gán giá tr? t? 1 ??n 100 cho myArray
            arr[i] = i * 2;     // Ví d?: Gán giá tr? b?ng g?p ?ôi ch? s? cho arr
        }
    }

    private void OnDestroy()
    {
        // Hãy nh? gi?i phóng b? nh? khi không s? d?ng n?a
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
