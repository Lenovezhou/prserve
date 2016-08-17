using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// ------------------------------------------创建委托实例------------------------------------|
/// 现在如果你回到unity编辑器，它将显示错误消息“在类型` DelegateHandler”之外使用时，事件`DelegateHandler.buttonClickDelegate”只能出现在+ =或-=的左边。
///            在Start方法如下改变来消除错误，别忘了在OnDisable销毁方法，否则，它将内存泄露。
/// ---------------------------------------作者：LenoveZhou-----------------------------------|
/// ----------------------------------------时间：16.08.17------------------------------------|
/// </summary>
public class Delegatehandler : MonoBehaviour {

    
    public delegate void OnButtonClickDelegate();
    public static OnButtonClickDelegate _buttonClickDelegate;         //delegate变量声明
    public static event OnButtonClickDelegate buttonClickDelegate;    //envet   变量申明
    private Button button;
    void Start() 
    {
        button = GameObject.FindGameObjectWithTag("button").GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick() 
    {
        buttonClickDelegate();
    }
    void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
