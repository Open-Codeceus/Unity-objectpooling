using System.Collections.Generic;
using UnityEngine;
public class SBulletPool : MonoBehaviour
{
    public class Pool
    {
        public string name;
        public int count;
        GameObject item;

        int idx = 0;
        Transform parent;
        List<GameObject> list;

        public Pool(string name, GameObject item, int count, Transform parent)
        {
            this.name = name;
            this.item = item;
            this.count = count;
            this.parent = parent;
            list = new List<GameObject>();
        }

        public GameObject getItem()
        {
            idx %= list.Count;

            if (list[idx].activeInHierarchy)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (!list[idx++].activeInHierarchy) return list[idx++];
                    idx %= list.Count;
                }
                idx = list.Count;
                generateItem();
            }

            list[idx].SetActive(true);
            return list[idx++];
        }

        public void generateItem()
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(item, parent);
                list.Add(go);
                go.SetActive(false);
            }
        }
        public void generateItem(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(item, parent);
                list.Add(go);
                go.SetActive(false);
            }
        }
    }

    [System.Serializable]
    public class PoolInfo
    {
        public string name;
        public int count;
        public GameObject item;
    }

    public static SBulletPool inst;
    public List<PoolInfo> poolInfo;
    public Dictionary<string, Pool> pools;

    void Awake()
    {
        if (inst == null) inst = this;
        pools = new Dictionary<string, Pool>();

        foreach (PoolInfo pi in poolInfo)
        {
            GameObject go = new GameObject();
            go.transform.parent = gameObject.transform;
            go.gameObject.name = pi.name;
            pools.Add(pi.name, new Pool(pi.name, pi.item, pi.count, go.transform));
            pools[pi.name].generateItem();
        }
    }
}
