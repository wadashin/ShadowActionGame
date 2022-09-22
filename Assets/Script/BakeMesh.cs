using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeMesh : MonoBehaviour
{
    [SerializeField]
    MeshRenderer BaseMeshObj;    // ベイクする元のオブジェクト

    [SerializeField]
    GameObject BakeMeshObj;            // ベイクしたメッシュを格納するGameObject

    // BakeMeshObjをインスタンスした際のSkinnedMeshRendererリスト
    List<GameObject> BakeCloneMeshList = new List<GameObject>();

    int MaxCloneCount = 5;       // 残像数
    int CloneCount = 0;
    float FlameCountMax = 0.02f;    // 残像を更新する頻度
    int FlameCount = 0;

    void Start()
    {
        StartCoroutine("ZanzouCreate");
    }

    void FixedUpdate()
    {
        
    }

    IEnumerator ZanzouCreate()
    {
        if (CloneCount < MaxCloneCount)
        {
            CloneCount++;
            GameObject copy = Instantiate(BakeMeshObj, transform.position = BaseMeshObj.transform.position, transform.rotation = BaseMeshObj.transform.rotation);
            BakeCloneMeshList.Add(copy);
            
        }
        else
        {
            BakeCloneMeshList[0].transform.position = BaseMeshObj.transform.position;
            BakeCloneMeshList[0].transform.rotation = BaseMeshObj.transform.rotation;
            BakeCloneMeshList.Add(BakeCloneMeshList[0]);
            BakeCloneMeshList.RemoveAt(0);
        }
        yield return new WaitForSecondsRealtime(FlameCountMax);
        StartCoroutine("ZanzouCreate");
    }
}