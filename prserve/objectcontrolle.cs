using UnityEngine;
using System.Collections;


/// <summary>
/// ----如果多个类监听相同的引用，这种情况是很难去跟踪的，为了避免这种情况，我们使用事件------|
/// 它将显示错误消息“在类型` DelegateHandler”之外使用时，事件`DelegateHandler.buttonClickDelegate”
/// 只能出现在+ =或-=的左边，要将所有的合并运算符都改成+=或-=，且在左边；
/// ---------------------------------------作者：LenoveZhou-----------------------------------|
/// ----------------------------------------时间：16.08.17------------------------------------|
/// </summary>



public class objectcontrolle : MonoBehaviour {

    MeshRenderer objRenderer;
    void Start() 
    {
        objRenderer = GetComponent<MeshRenderer>();
        Delegatehandler.buttonClickDelegate += ChangePosition;
        Delegatehandler.buttonClickDelegate += ChangeColor;
        Delegatehandler.buttonClickDelegate += ChangeRotation;   //只有changerotation会执行
    }
    void ChangePosition() 
    {
        transform.position = new Vector2(transform.position.x+2f,transform.position.y);
    }
    void ChangeColor() 
    {
       
        objRenderer.material.color = Color.yellow;
    }
    void ChangeRotation() 
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y+10f,transform.eulerAngles.z);
        Debug.Log("changerotation");
    }
    void OnDisable() 
    {
        Delegatehandler.buttonClickDelegate -= ChangeColor;
        Delegatehandler.buttonClickDelegate -= ChangePosition;
        Delegatehandler.buttonClickDelegate -= ChangeRotation;

    }
}
