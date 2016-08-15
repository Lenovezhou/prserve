using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class myrotation : MonoBehaviour {
    public GameObject a;
   
    public GameObject target,road,parent;
    public float rotationspped;
    private float addx, addy;
    private float distance;
    private float h,v,rotateSpeed=3;
    private Touch oldTouch1,oldTouch2;
    private Vector3 roadaspeakte;
    private List<GameObject> roads;
    private float timer;
	void Start () {
        roads = new List<GameObject>();
        addx =target.transform.localRotation.eulerAngles.x;
        addy =target.transform.localRotation.eulerAngles.y;
      
	}

    void Update() {
       
        timer += Time.deltaTime;
        a.GetComponent<Renderer>().sharedMaterial.mainTextureOffset = new Vector2(-timer,0);



        //没有触摸
        if (Input.touchCount <= 0)
        {
            return;
        }

        //单点触摸， 水平上下旋转
        if (1 == Input.touchCount)
        {
            
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition;
            target.transform.Rotate(Vector3.back  * deltaPos.x , Space.Self); 
         //   target.transform.Rotate(Vector3.right * deltaPos.y, Space.World);
        }

        //多点触摸, 放大缩小
        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);

        //第2点刚开始接触屏幕, 只记录，不做处理
        if (Input.GetTouch(1).phase==TouchPhase.Began)
        {
          
            oldTouch2 = newTouch2;
            oldTouch1 = newTouch1;
            return;
        }
        if (2==Input.touchCount)
        {
            //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型
            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

            //两个距离之差，为正表示放大手势， 为负表示缩小手势
            float offset = newDistance - oldDistance;

            //放大因子， 一个像素按 0.01倍来算(100可调整)
            float scaleFactor = offset / 100f;

            //控制对象的scale

            Vector3 localScale = target.transform.localScale;
            Vector3 scale = new Vector3(localScale.x + scaleFactor,
                                        localScale.y + scaleFactor,
                                        localScale.z + scaleFactor);

            //最小缩放到 0.3 倍
            if (scale.x > 0.3f && scale.y > 0.3f && scale.z > 0.3f)
            {
                target.transform.localScale = scale;
            }

            //记住最新的触摸点，下次使用
            oldTouch1 = newTouch1;
            oldTouch2 = newTouch2;
        }
       
    }





	void LateUpdate () {




        if (Input.GetMouseButton(0))
        {
                float a = Input.GetAxis("Mouse X");
                float b = Input.GetAxis("Mouse Y");
                addx += a;
                addy += b;
                //target.transform.rotation = Quaternion.Euler(0, a * Time.deltaTime * 2, 0);
                target.transform.Rotate(0, 0, -a * 5, Space.Self);

        }
        if (Input.GetAxis("Mouse ScrollWheel")!=0)
        {
            float scroolwheel = Input.GetAxis("Mouse ScrollWheel");
            float targetscale = target.transform.localScale.x;
            targetscale += scroolwheel*10;
         target.transform.localScale=new Vector3 (targetscale,targetscale,targetscale);   
        }
        if (Input.GetMouseButton(1))
        {
            float a = Input.GetAxis("Mouse X");
            float b = Input.GetAxis("Mouse Y");
            addx += a;
            addy += b;
            transform.localPosition += new Vector3(-addx, -addy, 0);





          //  Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            // Move object across XY plane
         //   transform.Translate(-touchDeltaPosition.x*5 , -touchDeltaPosition.y*5 , 0);
        }
      //当没有触控时就返回
     

	
	

		
	}

}
