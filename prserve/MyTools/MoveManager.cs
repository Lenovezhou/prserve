using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveManager : MonoBehaviour
{
    public Transform[] ArrayList;

    public int length;

    public List<MoveRender> MoveRenderList;

    void Start()
    {
        MoveRenderList = new List<MoveRender>();
        length = ArrayList.Length;
        if (length >= 2)
        {
            GameObject go = Resources.Load<GameObject>("StartPoint");
            int num = length % 2;
            if (num == 0)
            {
                for (int i = 0; i < length; i += 2)
                {
                    GameObject Move = (GameObject)Instantiate(go, ArrayList[i].transform.position, Quaternion.identity);
                    MoveRender RenderScript = Move.GetComponent<MoveRender>();
                    RenderScript.Points = new Transform[] { ArrayList[i], ArrayList[i + 1] };
                    if (RenderScript)
                    {
                        MoveRenderList.Add(RenderScript);
                    }
                }
            }
            else
            {

            }
        }


        StartCoroutine(StartToEnd());
    }


    IEnumerator StartToEnd()
    {
        for (int i = 0; i < MoveRenderList.Count; i++)
        {
            MoveRenderList[i].startMove = true;
            while (MoveRenderList[i].StartNext == false)
            {
                yield return null;
            }
        }

    }
}
