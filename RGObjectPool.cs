using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RGUnityTools
{
    public class RGObjectPool : MonoBehaviour
    {
        public List<GameObject> pool { get; private set; }
        public Queue<GameObject> activeObjects { get; private set; }

        public GameObject prefab;
        public int capacity;
        [Tooltip("Respawns the oldest object if all objects are active.")]
        public bool recycle;

        private void Awake()
        {
            CreatePool();
        }

        void CreatePool()
        {
            pool = new List<GameObject>();
            activeObjects = new Queue<GameObject>();

            for (int i = 0; i < capacity; i++)
            {
                GameObject go = Instantiate(prefab, transform);
                pool.Add(go);
                go.SetActive(false);
            }
        }

        public GameObject SpawnAt(Vector3 spawnpoint)
        {
            if (pool.Count <= 0) return null;

            //Check for inactive object then spawn it:
            foreach (GameObject go in pool)
            {
                if (activeObjects.Contains(go)) continue;

                activeObjects.Enqueue(go);
                go.transform.position = spawnpoint;
                go.SetActive(true);
                return go;
            }

            //Recycle oldest if no inactive objects found:
            if (recycle)
            {
                activeObjects.Dequeue();
                return SpawnAt(spawnpoint);
            }

            return null;
        }

        public GameObject SpawnAt(Vector3 spawnpoint, float chance)
        {
            float rand = Random.Range(0f, 1f);
            if (rand > chance) return null;

            return SpawnAt(spawnpoint);
        }
    }
}
