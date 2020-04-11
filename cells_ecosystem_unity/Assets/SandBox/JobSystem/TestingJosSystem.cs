//#define USEJOB
//#define USEPARALLEJOB
#define USEPARALLEJOBTRANSFORMS

using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UIElements;


namespace CellsEcosystem.SandBox.JobSystem
{
    public class TestingJosSystem : MonoBehaviour
    {
        [SerializeField]
        bool useJobs;
        [SerializeField]
        Transform pfYukari;
        List<Yukari> yukariList;

        public class Yukari
        {
            public Transform transform;
            public float moveY;
        }

        private void Start()
        {
#if !USEJOB
            yukariList = new List<Yukari>();
            for (int i = 0; i < 1000; i++)
            {
                Transform yukariTransform = Instantiate(pfYukari, new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-5f, 5f), 0f), Quaternion.identity);
                yukariList.Add(new Yukari
                {
                    transform = yukariTransform,
                    moveY = UnityEngine.Random.Range(1f, 2f)
                });
            }
#endif
        }

        private void Update()
        {
            if (useJobs)
            {
#if USEJOB
                NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
                for (int i = 0; i < 10; i++)
                {
                    JobHandle jobHandle = ReallyToughTaskJob();
                    jobHandleList.Add(jobHandle);
                }
                JobHandle.CompleteAll(jobHandleList);
                jobHandleList.Dispose();

#elif USEPARALLEJOB
                NativeArray<float3> positionArray = new NativeArray<float3>(yukariList.Count, Allocator.TempJob);
                NativeArray<float> moveYArray = new NativeArray<float>(yukariList.Count, Allocator.TempJob);

                for (int i = 0; i < yukariList.Count; i++)
                {
                    positionArray[i] = yukariList[i].transform.position;
                    moveYArray[i] = yukariList[i].moveY;
                }

                ReallyToughParalleJob reallyToughParallelJob = new ReallyToughParalleJob
                {
                    deltaTime = Time.deltaTime,
                    positionArray = positionArray,
                    moveYArray = moveYArray,
                };

                JobHandle jobHandle = reallyToughParallelJob.Schedule(yukariList.Count, 100);
                jobHandle.Complete();

                for (int i = 0; i < yukariList.Count; i++)
                {
                    yukariList[i].transform.position = positionArray[i];
                    yukariList[i].moveY = moveYArray[i];
                }

                positionArray.Dispose();
                moveYArray.Dispose();

#elif USEPARALLEJOBTRANSFORMS
                TransformAccessArray transformAccessArray = new TransformAccessArray(yukariList.Count);
                NativeArray<float> moveYArray = new NativeArray<float>(yukariList.Count, Allocator.TempJob);

                for (int i = 0; i < yukariList.Count; i++)
                {
                    moveYArray[i] = yukariList[i].moveY;
                    transformAccessArray.Add(yukariList[i].transform);
                }
                ReallyToughParallelJobTransforms reallyToughParallelJobTransforms = new ReallyToughParallelJobTransforms
                {
                    deltaTime = Time.deltaTime,
                    moveYArray = moveYArray
                };

                var jobHandle = reallyToughParallelJobTransforms.Schedule(transformAccessArray);
                jobHandle.Complete();

                          for (int i = 0; i < yukariList.Count; i++)
                {
                    yukariList[i].moveY = moveYArray[i];
                }

                moveYArray.Dispose();
                transformAccessArray.Dispose();
#endif
            }
            else
            {
#if USEJOB
                for (int i = 0; i < 10; i++)
                {
                    ReallyToughTask();
                }
#else
                foreach (var yukari in yukariList)
                {
                    yukari.transform.position += new Vector3(0, yukari.moveY * Time.deltaTime);
                    if (yukari.transform.position.y > 5f)
                    {
                        yukari.moveY = -math.abs(yukari.moveY);
                    }
                    if (yukari.transform.position.y < -5f)
                    {
                        yukari.moveY = math.abs(yukari.moveY);
                    }
                    var value = 0f;
                    for (int i = 0; i < 50000; i++)
                    {
                        value = math.exp10(math.sqrt(value));
                    }
                }
#endif
            }
        }

        void ReallyToughTask()
        {
            // Represents a tough task like some pathfinding or really complex calculation
            var value = 0f;
            for (int i = 0; i < 50000; i++)
            {
                value = math.exp10(math.sqrt(value));
            }
        }

        JobHandle ReallyToughTaskJob()
        {
            ReallyToughJob job = new ReallyToughJob();
            return job.Schedule();
        }
    }

    [BurstCompile]
    public struct ReallyToughJob : IJob
    {
        public void Execute()
        {
            // Represents a tough task like some pathfinding or really complex calculation
            var value = 0f;
            for (int i = 0; i < 50000; i++)
            {
                value = math.exp10(math.sqrt(value));
            }
        }
    }

    [BurstCompile]
    public struct ReallyToughParalleJob : IJobParallelFor
    {
        public NativeArray<float3> positionArray;
        public NativeArray<float> moveYArray;
        public float deltaTime;
        public void Execute(int index)
        {
            positionArray[index] += new float3(0, moveYArray[index] * deltaTime, 0f);
            if (positionArray[index].y > 5f)
            {
                moveYArray[index] = -math.abs(moveYArray[index]);
            }
            if (positionArray[index].y < -5f)
            {
                moveYArray[index] = math.abs(moveYArray[index]);
            }
            var value = 0f;
            for (int i = 0; i < 50000; i++)
            {
                value = math.exp10(math.sqrt(value));
            }
        }
    }

    [BurstCompile]
    public struct ReallyToughParallelJobTransforms : IJobParallelForTransform
    {
        public NativeArray<float> moveYArray;
        public float deltaTime;
        public void Execute(int index, TransformAccess transform)
        {
            transform.position += new Vector3(0, moveYArray[index] * deltaTime, 0f);
            if (transform.position.y > 5f)
            {
                moveYArray[index] = -math.abs(moveYArray[index]);
            }
            if (transform.position.y < -5f)
            {
                moveYArray[index] = math.abs(moveYArray[index]);
            }
            var value = 0f;
            for (int i = 0; i < 50000; i++)
            {
                value = math.exp10(math.sqrt(value));
            }
        }
    }
}