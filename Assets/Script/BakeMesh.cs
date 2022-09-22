using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeMesh : MonoBehaviour
{
    [SerializeField]
    MeshRenderer BaseMeshObj;    // �x�C�N���錳�̃I�u�W�F�N�g

    [SerializeField]
    GameObject BakeMeshObj;            // �x�C�N�������b�V�����i�[����GameObject

    // BakeMeshObj���C���X�^���X�����ۂ�SkinnedMeshRenderer���X�g
    List<GameObject> BakeCloneMeshList = new List<GameObject>();

    int MaxCloneCount = 5;       // �c����
    int CloneCount = 0;
    float FlameCountMax = 0.02f;    // �c�����X�V����p�x
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