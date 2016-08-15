using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class AddButton : MonoBehaviour
{
    public GameObject DeleteGameOBj;
    public Image image1,image2;
    public Text kk;
    public GameObject ModelInfoBtn;
    public bool isClick = false;
    public bool modelInfoBtnClick = false;
    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
    public  List<string> PathURL =new List<string> ();
    public List<GameObject> forthpanelbutton = new List<GameObject>();
    public static AddButton _instand;
    public Transform B_parent;
    
    public GameObject ProductButton,ProductButton1,ProductButton2;
    public RawImage modelInfoTexture;

    public List<GameObject> GameObjectPool;
    public Object[] list;
    public Object[] Icon;
    public Text text;
    List<List<Object>> Module = new List<List<Object>>();
    bool isOne;
    private int activechilde;


    [HideInInspector]
    public DataMsg DataMsg;
    InitServerConfig initIcon;

	void Start () 
    {

       
       // image1.gameObject.SetActive(false);     //  初始向下箭头
      //  image2.gameObject.SetActive(false);
        initIcon = Camera.main.GetComponent<InitServerConfig>();
        DataMsg = Camera.main.GetComponent<DataMsg>();
        GameObjectPool = new List<GameObject>();
        _instand = this;



        string path1 =
#if UNITY_EDITOR
 "file://" + Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
    "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IOS
“far:///”+ Application.Streamingassetspath+ "/Raw/"
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
 "file://" + Application.dataPath + "/StreamingAssets/";
#else
 string.Empty;
#endif
 PathURL.Add(path1);
	}

	
	// Update is called once per frame
    void Update()
    {
        //点击模块按钮时
        if (DataMsg.ModuleClick)
        {

            StartCoroutine(LoadXml(PathURL[0] + "Data.xml", DataMsg.NowModuleName));
            DataMsg.ModuleClick = false;
        }

       
      

    }
    
    //加载XML
    IEnumerator LoadXml(string path,string modNume)
    {
        GameObjectPool.Clear();
        WWW m_SaveWWW = new WWW(path);
        yield return m_SaveWWW;
        DataMsg.moduleList= ReadXML.ReadInfo(m_SaveWWW.text,modNume);
        LoadModuleButton(DataMsg.moduleList);
    }
    //加载模块和子UI的Button （子UI的Button生成时隐藏）
    void LoadModuleButton(List<Module> data)
    {

        for (int i = 0; i < data.Count; i++)//  SaveInfo temp in data)
        {
            GameObject button = Instantiate(ProductButton);
            //button.transform.SetParent(DataMsg.parent[0]);
            button.transform.parent = DataMsg.parent[0];
            button.transform.localScale = Vector3.one;
            ModuleManager module = button.AddComponent<ModuleManager>();    // 为每个按钮添加脚本
            module.initModule(data[i]);     //  初始化
            GameObjectPool.Add(button);         // 保存后用于切换panel时删除实例物体
            for (int j = 0; j < data[i].SubList.Count; j++)
            {
                GameObject subbutton = Instantiate(ProductButton1);//ProductButton：  Button 的预制体
                subbutton.SetActive(false);
                subbutton.transform.SetParent(DataMsg.parent[0]);
                subbutton.transform.localScale = Vector3.one;
                SubManager sub = subbutton.AddComponent<SubManager>();
                sub.initModule(data[i].SubList[j]);
                module.SubObj.Add(subbutton);
                GameObjectPool.Add(subbutton);      // 保存后用于切换panel时删除实例物体
            }
        }

     //   ChackActiveCount(image1);       //  是否显示向下箭头
       
        
        
    }

    //public void ChackActiveCount(Image image)            //      判断当前状态为ture的button个数，决定是否显示向下箭头
    //{
    //    activechilde = 0;
    //    if (GameObjectPool.Count > 8)
    //    {
    //        foreach (GameObject item in GameObjectPool)
    //        {
    //            if (item.activeSelf)
    //            {
    //                activechilde += 1;
    //                if (activechilde>8)
    //                {
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    if (activechilde > 8)
    //    {
    //        image.gameObject.SetActive(true);
    //    }
    //    else 
    //    {
    //        image.gameObject.SetActive(false);
    //    }
    //}
}
