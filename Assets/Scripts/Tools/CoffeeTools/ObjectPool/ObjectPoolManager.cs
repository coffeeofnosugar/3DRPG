using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tools.CoffeeTools
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

        private GameObject _objectPoolEmptyHolder;

        private static GameObject _particleSystemEmpty;

        private static GameObject _gameObjectEmpty;
        
        public enum PoolTypeEnum { ParticleSystem, GameObject, None }

        public static PoolTypeEnum PoolType;

        private void Awake()
        {
            SetupEmpties();
        }

        private void SetupEmpties()
        {
            _objectPoolEmptyHolder = new GameObject("Pool Objects");

            _particleSystemEmpty = new GameObject("Particle Effects");
            _particleSystemEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

            _gameObjectEmpty = new GameObject("GameObjects");
            _gameObjectEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
        }

        /// <summary>
        /// 创建物体
        /// </summary>
        /// <param name="objectToSpawn"></param>
        /// <param name="spawnPosition"></param>
        /// <param name="spawnRotation"></param>
        /// <param name="poolType"></param>
        /// <returns></returns>
        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolTypeEnum poolType = PoolTypeEnum.None)
        {
            // 先检测是否有这个池(后面都称作pool)
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
            
            // 如果没有找到pool，创建一个
            if (pool == null)
            {
                pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
                ObjectPools.Add(pool);
            }
            
            // 检测pool中是否有物体
            GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObj == null)
            {
                // 通过枚举获取父级
                Transform parentObject = SetParentObject(poolType);
                
                // 如果pool中没有待机的物体，创建一个
                spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

                // 如果通过枚举获取到的父级不为空，就设置父级
                if (parentObject != null)
                    spawnableObj.transform.SetParent(parentObject);
            }
            else
            {
                // 如果pool中有待机物体，激活他
                spawnableObj.transform.position = spawnPosition;
                spawnableObj.transform.rotation = spawnRotation;
                pool.InactiveObjects.Remove(spawnableObj);
                spawnableObj.SetActive(true);
            }

            return spawnableObj;
        }
        
        /// <summary>
        /// 创建物体
        /// </summary>
        /// <param name="objectToSpawn"></param>
        /// <param name="parentTransform"></param>>
        /// <param name="index"></param>
        /// <returns></returns>
        public static GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform, int? index = null)
        {
            // 先检测是否有这个池(后面都称作pool)
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
            
            // 如果没有找到pool，创建一个
            if (pool == null)
            {
                pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
                ObjectPools.Add(pool);
            }
            
            // 检测pool中是否有物体
            GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObj == null)
            {
                // 如果pool中没有待机的物体，创建一个
                spawnableObj = Instantiate(objectToSpawn, parentTransform);
            }
            else
            {
                // 如果pool中有待机物体，激活他
                spawnableObj.transform.SetParent(parentTransform);
                spawnableObj.transform.position = parentTransform.position;
                spawnableObj.transform.rotation = parentTransform.rotation;
                pool.InactiveObjects.Remove(spawnableObj);
                spawnableObj.SetActive(true);
            }

            if (index == -1)
                spawnableObj.transform.SetAsLastSibling();
            else if (index == 0)
                spawnableObj.transform.SetAsFirstSibling();
            else if (index != null)
                spawnableObj.transform.SetSiblingIndex((int)index);
            

            return spawnableObj;
        }

        /// <summary>
        /// 待机物体，将该物体放回pool
        /// </summary>
        /// <param name="obj"></param>
        public static void ReturnObjectToPool(GameObject obj)
        {
            string goName = obj.name[..^7];  // 移除掉名字中的(Clone)

            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

            if (pool == null)
            {
                Debug.LogWarning("尝试释放一个没有pool的对象: " + obj.name);
            }
            else
            {
                // 待机该物体
                obj.SetActive(false);
                pool.InactiveObjects.Add(obj);
            }
        }

        /// <summary>
        /// 通过枚举设置父级
        /// </summary>
        /// <param name="poolType"></param>
        /// <returns></returns>
        private static Transform SetParentObject(PoolTypeEnum poolType)
        {
            return poolType switch
            {
                PoolTypeEnum.ParticleSystem => _particleSystemEmpty.transform,
                PoolTypeEnum.GameObject => _gameObjectEmpty.transform,
                PoolTypeEnum.None => null,
                _ => null
            };
        }
    }


    public class PooledObjectInfo
    {
        public string LookupString;
        public List<GameObject> InactiveObjects = new List<GameObject>();
    }
}