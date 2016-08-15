using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ProductManager : MonoBehaviour
{
    public Material[] mats;
    private Button SelfButton;
    private Text SelfText;
    void Start()
    {
        SelfButton = GetComponent<Button>();
        SelfText = SelfButton.GetComponentInChildren<Text>();
        SelfText.text = name;
       // SelfButton.onClick.AddListener(UImanager._instance.ForthP_button);
        SelfButton.onClick.AddListener(Click);
    }

    void Click()
    {
        StartCoroutine(LoadModel(ModleURL));
    }

    //根据路径加载模型
    public IEnumerator LoadModel(string path)
    {

        WWW bundle = new WWW(AddButton._instand.PathURL[0] + path);

        yield return bundle;

        //克隆出来的Budul 加载到游戏中
        GameObject target = (GameObject)Instantiate(bundle.assetBundle.mainAsset);
        gameObjectEventClick.modelObj = target;
        //if(bundle.assetBundle.mainAsset.name == "FAST微型锁定接骨板")
        //{
        //    target.transform.GetChild(0).gameObject.GetComponents<Material>();
        //}
        
        //
        //www  加载图片  地址是AddButton._instand.PathURL[0] + Description
        //
        if (Description != "")
        {
            StartCoroutine(LoadTexture(AddButton._instand.PathURL[0] + Description));
        }
        AddButton._instand.DeleteGameOBj = target;
        bundle.assetBundle.Unload(false);
        
    }

    //加载图片
    IEnumerator LoadTexture(string path )
    {
        WWW www = new WWW(path);
        yield return www;
        if (www.error == null)
        {
            if (www.texture != null)
            {
                UImanager._instance.productPanel.GetComponent<RawImage>().texture = www.texture;
                UImanager._instance.ForthP_button();
            }
        }
    }
    public void initModule(Product module)
    {
        Name = module.Name;
        ModleURL = module.ModleURL;
        TextureURL = module.TextureURL;
        Description = module.Description;
      
    }


    private string description;
    public string Description
    {
        set { description = value; }
        get { return description; }
    }
    private string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private string modleURL;
    public string ModleURL
    {
        get { return modleURL; }
        set { modleURL = value; }
    }

    private string textureURL;
    public string TextureURL
    {
        get { return textureURL; }
        set { textureURL = value; }
    }
}

public class Product
{

    private string description;
    public string Description
    {
        set { description = value; }
        get { return description; }
    }
    private string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private string modleURL;
    public string ModleURL
    {
        get { return modleURL; }
        set { modleURL = value; }
    }

    private string textureURL;
    public string TextureURL
    {
        get { return textureURL; }
        set { textureURL = value; }
    }
}
