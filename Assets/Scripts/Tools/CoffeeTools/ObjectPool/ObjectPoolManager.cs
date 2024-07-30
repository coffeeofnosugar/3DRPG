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
        /// ��������
        /// </summary>
        /// <param name="objectToSpawn"></param>
        /// <param name="spawnPosition"></param>
        /// <param name="spawnRotation"></param>
        /// <param name="poolType"></param>
        /// <returns></returns>
        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolTypeEnum poolType = PoolTypeEnum.None)
        {
            // �ȼ���Ƿ��������(���涼����pool)
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
            
            // ���û���ҵ�pool������һ��
            if (pool == null)
            {
                pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
                ObjectPools.Add(pool);
            }
            
            // ���pool���Ƿ�������
            GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObj == null)
            {
                // ͨ��ö�ٻ�ȡ����
                Transform parentObject = SetParentObject(poolType);
                
                // ���pool��û�д��������壬����һ��
                spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

                // ���ͨ��ö�ٻ�ȡ���ĸ�����Ϊ�գ������ø���
                if (parentObject != null)
                    spawnableObj.transform.SetParent(parentObject);
            }
            else
            {
                // ���pool���д������壬������
                spawnableObj.transform.position = spawnPosition;
                spawnableObj.transform.rotation = spawnRotation;
                pool.InactiveObjects.Remove(spawnableObj);
                spawnableObj.SetActive(true);
            }

            return spawnableObj;
        }
        
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="objectToSpawn"></param>
        /// <param name="parentTransform"></param>>
        /// <param name="index"></param>
        /// <returns></returns>
        public static GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform, int? index = null)
        {
            // �ȼ���Ƿ��������(���涼����pool)
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
            
            // ���û���ҵ�pool������һ��
            if (pool == null)
            {
                pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
                ObjectPools.Add(pool);
            }
            
            // ���pool���Ƿ�������
            GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObj == null)
            {
                // ���pool��û�д��������壬����һ��
                spawnableObj = Instantiate(objectToSpawn, parentTransform);
            }
            else
            {
                // ���pool���д������壬������
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
        /// �������壬��������Ż�pool
        /// </summary>
        /// <param name="obj"></param>
        public static void ReturnObjectToPool(GameObject obj)
        {
            string goName = obj.name[..^7];  // �Ƴ��������е�(Clone)

            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

            if (pool == null)
            {
                Debug.LogWarning("�����ͷ�һ��û��pool�Ķ���: " + obj.name);
            }
            else
            {
                // ����������
                obj.SetActive(false);
                pool.InactiveObjects.Add(obj);
            }
        }

        /// <summary>
        /// ͨ��ö�����ø���
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