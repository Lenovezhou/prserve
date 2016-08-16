#if UNITY_ANDROID && !UNITY_EDITOR
#define ANDROID
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
#define IPHONE
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cameraRotation : MonoBehaviour 
{
    /// <summary>
    /// 这个脚本主要是控制摄像机旋转
    /// 让摄像机跟随一个空的物体（坐标为（0，0，0））旋转，
    /// 先旋转跟随的物体，然后再让摄像机与跟随物体之间插值，实现缓冲效果；
    /// 单点触控为旋转，多点触控为放大或缩小
    /// </summary>
    /// 
    public static cameraRotation _instance;

    private Transform followTarget;//摄像机跟随的目标
    private float rotateSpeed =3f;//旋转的速度
    private float transSpeed = 3f;//移动的速度
    private float h;//x轴方向的移动量
    private float v;//y轴方向的移动量
    private float distance = 5f;//摄像机与物体之间的距离
    private Touch oldTouch1;//上次触摸点1
    private Touch oldTouch2;//上次触摸点2
    private Touch newTouch1;//新的触摸点1
    private Touch newTouch2;//新的触摸点2

    private Quaternion cameraBeginRotation;//摄像机原始旋转
    private  Vector3 cameraPosition;//摄像机原始位置
    private bool istrans = false;
    private Vector3 vc;

	void Start () 
    {
        GameObject obj = new GameObject();
        obj.transform.position = Vector3.zero;
        _instance = this;
        cameraBeginRotation = transform.rotation;
        cameraPosition = transform.position;

        followTarget = obj.transform;
        if(followTarget)
        {
            //让摄像机平实目标，y轴和跟随目标旋转同样的角度
            transform.rotation = Quaternion.Euler(0, followTarget.rotation.eulerAngles.y, 0);
            //让摄像机与跟随目标之间有一定的距离
            transform.position = transform.rotation * new Vector3(0, 0, -distance) + followTarget.position;
        }
	}
	
	void Update ()
    {
        //当没有触控时就返回
        if (Input.touchCount <= 0)
        {
            return;
        }
#if IPHONE || ANDROID
        if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
        if (EventSystem.current.IsPointerOverGameObject())
#endif
            Debug.Log("aaaaaaaaa");
        else
        {
            //单点触控+
            .0.
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    istrans = true;
                    //旋转跟随目标
                    h = Input.GetAxis("Mouse X") * rotateSpeed;
                    v = Input.GetAxis("Mouse Y") * rotateSpeed;
                    followTarget.Rotate(-v, h, 0, Space.Self);
                }
            }
            else
            {
                istrans = false;
            }
            //多点触控实现控制摄像机与物体之间的距离
            if (Input.touchCount >= 2)
            {
                newTouch1 = Input.GetTouch(0);
                newTouch2 = Input.GetTouch(1);

                //当第2点刚开始触摸屏幕时，只记录不处理
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch1 = newTouch1;
                    oldTouch2 = newTouch2;
                    return;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                    float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
                    if (Mathf.Abs(oldDistance - newDistance) < 1.5f)
                    {

                        vc = Input.GetTouch(0).deltaPosition * Time.deltaTime * 0.5f ;
                        followTarget.Translate(-vc.x, -vc.y, 0);
                    }
                    else
                    {

                        float offset = newDistance - oldDistance;

                        //放大、缩小
                        float scaleSpeed = offset / 300f;
                        distance -= scaleSpeed;
                        distance = Mathf.Clamp(distance, 1, 10);
                    }

                } else if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                    {
                        //
                        float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                        float offset = newDistance - oldDistance;

                        //放大、缩小
                        float scaleSpeed = offset / 300f;
                        distance -= scaleSpeed;
                        distance = Mathf.Clamp(distance, 1, 10);
                    }


                //记录最新的触摸点，下次使用
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
        }


	}



    void LateUpdate()
    {
        //根据跟随目标旋转的角度，调整摄像机的旋转角度和位置

        ////备用
        //transform.rotation = Quaternion.Slerp(transform.rotation, followTarget.rotation, 0.08f);
        //Vector3  ps = transform.rotation * new Vector3(0, 0, -distance) + followTarget.transform.position;
        //transform.position = Vector3.Lerp(transform.position, ps,200*Time.deltaTime);

        if(istrans)
        {
           transform.rotation = Quaternion.Slerp(transform.rotation, followTarget.rotation, 0.08f);
           transform.position = transform.rotation * new Vector3(0, 0, -distance) + followTarget.transform.position;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, transform.rotation * new Vector3(0, 0, -distance) + followTarget.transform.position, 0.2f);
        }

    }


    public void cameraRecover()
    {
        Debug.Log("调用归位显示");
        distance = 5f;
        followTarget.position = Vector3.zero;
        followTarget.rotation = Quaternion.identity;
        transform.position = cameraPosition;
        transform.rotation = Quaternion.Euler(cameraBeginRotation.x, cameraBeginRotation.y, cameraBeginRotation.z);
    }


}
