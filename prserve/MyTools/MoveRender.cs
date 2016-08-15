using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveRender : MonoBehaviour
{
    public Transform[] Points;

    public Transform StartPoint;
    public Transform EndPoint;

    public bool startMove;

    private int ArrayLength;
    private int index;

    private Transform MoveCube;
    private TextMesh ShowText;
    private TrailRenderer Renderline;

    void Start()
    {
        StartPoint = null;
        EndPoint = null;
        ArrayLength = Points.Length;

        MoveCube = transform.GetChild(0);
        ShowText = GetComponentInChildren<TextMesh>();
        Renderline = GetComponentInChildren<TrailRenderer>();
        Renderline.time = Mathf.Infinity;
    }

    public void StartLine(Transform[] PositionArray, int num, out Transform startPos, out Transform endPos)
    {
        startPos = null;
        endPos = null;
        if (startMove == false) return;
        int length = PositionArray.Length;
        if (length < 2) return;
        //数组包含数量大于等于2
        else
        {
            Debug.Log("赋值");
            if (num >= 0 && num < length - 1)
            {
                startPos = PositionArray[num];
                endPos = PositionArray[num + 1];
                return;
            }
        }
    }

    void Update()
    {
        if (startMove == false) return;

        if (StartPoint == null && EndPoint == null)
        {
            StartLine(Points, index, out StartPoint, out EndPoint);
            if (StartPoint != null && EndPoint != null)
            {
                transform.position = StartPoint.position;
                MoveCube.transform.localPosition = Vector3.zero;
            }

        }
        else
        {
            //RenderLines(StartPoint, EndPoint);
            if (Vector3.Distance(MoveCube.position, EndPoint.position) > 0.1f)
            {
                MoveCube.position = Vector3.MoveTowards(MoveCube.position, EndPoint.position, 0.02f);
                float distance = Vector3.Distance(transform.position, MoveCube.position);
                distance = Round(distance, 2);
                ShowText.text = distance + "";
            }
            else
            {
                StartPoint = null;
                EndPoint = null;
                index++;
            }

        }
    }

    public static float Round(float value, int digit)
    {
        float vt = Mathf.Pow(10, digit);
        //1.乘以倍数 + 0.5
        float vx = value * vt + 0.5f;
        //2.向下取整
        float temp = Mathf.Floor(vx);
        //3.再除以倍数
        return (temp / vt);
    }


    bool RenderLines(Transform StartPosition, Transform EndPosition)
    {

        return false;
    }
}
