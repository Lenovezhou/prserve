using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Text;
using UnityEngine.UI;

public enum Info_Type 
{
    SendCompany_ID = 0,
   
}

/// <summary>
/// ------------------------------------所有注释内容均为有用的--------------------------------|
/// ---------------------------------------作者：LenoveZhou-----------------------------------|
/// ----------------------------------------时间：16.08.17------------------------------------|
/// </summary>
public class ALLdownANDxmltool : MonoBehaviour {
    
    private string localpath;

	void Start () {
        string str11 = "sdfljlsd//sdfljle";
        string[] s = str11.Split('/');      //参数为1个char字符时只有之前保留
        localpath = Application.dataPath + "/XML/writexml/testfiledelete.cs";
        FileinfoDirectory();
        CreatXML();
       // ReadXml(Application.dataPath + "/XML/config.xml");
	}


    /// <summary>
    /// 文件夹及文件的操作
    /// </summary>

    void FileinfoDirectory() 
    {
        if (File.Exists(localpath))
        {
            Debug.Log("this folder is alive");
            File.Delete(localpath);
            Directory.CreateDirectory(Application.dataPath+"/XML/WriteXml_new");
            File.Create(Application.dataPath + "/XML/WriteXml_new/newscript1.xml");
            File.Create(Application.dataPath + "/XML/WriteXml_new/newscript2.cs");
            File.Create(Application.dataPath + "/XML/WriteXml_new/newscript3.cs");
            File.Create(Application.dataPath + "/XML/WriteXml_new/newscript4.cs");
            
        }
        string[] path = Directory.GetFiles(Application.dataPath+"/XML/writexml/", "*.cs", SearchOption.TopDirectoryOnly);
      //  Debug.LogError(path[0]);
        //获取文件夹下所以文件路径
        if (Directory.Exists(Application.dataPath + "/XML/WriteXml_new"))
        {
            string[] diArr = System.IO.Directory.GetDirectories(Application.dataPath , "*", System.IO.SearchOption.AllDirectories);
            string[] rootfiArr = System.IO.Directory.GetFiles(Application.dataPath + "/XML/WriteXml_new");
            if (rootfiArr != null)
            {
                foreach (string fi in rootfiArr)
                {
                    // Debug.Log(fi.ToString());
                    if (!fi.Contains("meta"))
                    {
                     //   Debug.Log(fi);
                    }
                }
            }
            if (rootfiArr != null)
            {
                foreach (string fi in diArr)
                {
                  //  Debug.Log(fi);
                }
            }
        }

    }
    /// <summary>
    /// 创建xml的方法
    /// </summary>
    public static void CreatXML()
    {
     
        XmlDocument doc = new XmlDocument();
        XmlNode declare = doc.CreateXmlDeclaration("1.0", "utf-8", "");
        doc.AppendChild(declare);
        XmlElement root = doc.CreateElement("Program");
        doc.AppendChild(root);
        //for (int i = 0; i < roomid.Count; i++)
        //{
    
        //    XmlElement id = doc.CreateElement("id");

        //    id.SetAttribute("layoutID", roomid[i]);

        //    id.SetAttribute("type", sceneindex[i].ToString());
        //    root.AppendChild(id);

        //}
        doc.Save(Application.dataPath + "/XML/config.xml");
      
    }


    /// <summary>
    /// CreatXML的重载方法：从服务器加载
    /// </summary>
    /// <param name="sendBuff">获取下来的字符串</param>
    /// <param name="infoTpye">枚举类型确定写入的地址</param>
    /// <param name="name">添加字符到xml的名字</param>
    public void start(string sendBuff, Info_Type info_Tpye, string name = "1")
    {
        byte[] sendBuffs = Encoding.UTF8.GetBytes(sendBuff);
        StartCoroutine(LoadXML(Application.dataPath+"/XML", sendBuffs, Info_Type.SendCompany_ID, name));
    }
    public IEnumerator LoadXML(string path, byte[] sendBuff, Info_Type infoTpye, string name)
    {
        string xml = "";
        WWW www = new WWW(path, sendBuff);
        yield return www;
        xml = www.text;
        if (www.error == null)
        {
            if (www.text == "")
            {
                Debug.Log("下载失败");
            }
            else
            {
                Debug.Log("下载成功");
                StartDownLoadXML(www.text, name + ".xml", infoTpye);
                //List<string> strlist = new List<string>();
                //strlist = ReadXML.ReadCompanyInfo(www.text, infoTpye);
                //LoadNext(strlist, infoTpye);
            }
        }
        else
        {
            Debug.Log("下载失败.................");
        }
    }

    public void StartDownLoadXML(string file, string name, Info_Type infoTpye)
    {

        byte[] model = Encoding.UTF8.GetBytes(file);
        int length = model.Length;

        string DirectoryPath = "";
        switch (infoTpye)
        {
            case Info_Type.SendCompany_ID:
                DirectoryPath = Application.dataPath + "/XML/PromotionHouse/";
                break;
            
        }
        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }

        string localpath = DirectoryPath + name;         //写入xml到本地

        if (!File.Exists(localpath))
        {
            //Debug.Log("文件夹不存在，创建中  " + localpath);

            CreateModelFile(DirectoryPath, name, model, length);
        }
        else
        {
            //StreamWriter sw = new StreamWriter(localpath, false, Encoding.UTF8);

            //sw.Write(model);
            //Stream sw;
            //sw = new FileInfo(localpath).Create();
            //sw.Write(model, 0, length);

            Debug.Log("文件已存在  " + localpath);
        }


    }

    /// <summary>
    /// 将文件写入指定路径内
    /// </summary>
    /// <param name="path">路径前缀</param>
    /// <param name="name">路径后缀</param>
    /// <param name="info">写入的文本内容</param>
    /// <param name="length">写入的文本长度</param>
    void CreateModelFile(string path, string name, byte[] info, int length)
    {
        //文件流信息
        //StreamWriter sw;
        Stream sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.Create();
        }
        else
        {
            //如果此文件存在则打开
            //sw = t.Append();
            return;
        }
        //以行的形式写入信息
        //sw.WriteLine(info);
        sw.Write(info, 0, length);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }

    /// <summary>
    /// 读取XML的方法
    /// </summary>
    /// <param name="path">路径</param>
    void ReadXml(string path) 
    {
        string xpath = "program/name_list/row";
        XmlDocument xml = new XmlDocument();
        xml.Load(path);
        XmlElement xmlroot = xml.DocumentElement;           //得到根节点
        string str = xmlroot.Attributes["program"].Value;   //获取根节点属性值
        XmlNode str1 = xml.SelectSingleNode("program");     //获取单个节点
        XmlNodeList nodelist = xmlroot.ChildNodes;          //根节点下的所有子节点
        foreach (XmlElement item in str1)
        {
            if (item.Value.Equals("ssname"))
            {
                Debug.Log("finded");
            }
        }
        XmlNodeList topM = xml.DocumentElement.ChildNodes;
        XmlNodeList topN = xml.SelectNodes(xpath);
        Debug.Log("topM"+topM.Count+"topN"+topN.Count);
    }


    /// <summary>
    /// camera旋转方法与方式
    /// </summary>
    void Update()
    {
        //资源加载完毕
        //Debug.LogError("执行到这里了");
        //if (MsgCenter._instance.BeginToshow)
        //{
        //    Debug.LogError("这里了");
        //    target.transform.localEulerAngles += new Vector3(0, 0.1f, 0);
        //    if (target.transform.localEulerAngles.y > 359)
        //    {
        //        Camera.main.fieldOfView = Mathf.Lerp(90, 30, 1);
        //    }
        //}
    }


    /// <summary>
    /// 下载图片方法与方式
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    IEnumerator LoadPicture(string path)
    {
        Debug.LogError("执行？");
        WWW www = new WWW(path);
        yield return www;

        RawImage image = transform.GetComponent<RawImage>();
        if (image)
        {
            image.texture = www.texture;
        }
    }

	//	NOTE: Sprite can NOT use LoadAsync
	//	如果需要动态更新，又要使用Sprite Packer，最好的解决方案是用AssetBundle，
	//	将你需要Sprite Packer到一起的Sprites打入同一个Assetbundle中。更新的时候只需要更新这个Assetbundle即可。加载：

	//	编辑器加载图片方法：
	//UnityEditor.Assetdatabase.LoadAssetAtPath<Sprite>("your assetbundle path");

	public void BoundleToSprite()
	{
	WWW www = new WWW("your assetbundle path");
	yield return www;
	AssetBundle assetBundle = www.assetBundle;
		Sprite sprite = assetBundle.LoadAsset<Sprite>("spriteName");
	//	或者
	Sprite[] sprites = assetBundle.LoadAllAsset<Sprite>();
	}



    /// <summary>
    /// 下载movie的方法方式
    /// </summary>
    /// <param name="path">路径后缀</param>
    /// <param name="BGImage">gameobject</param>
    /// <param name="movie">MovieTexture类型变量public访问属性</param>
    /// <returns></returns>
    
    IEnumerator LoadMovie(string path, RawImage BGImage,MovieTexture movie)
    {
        path = "file://" + Application.dataPath + path;
        AudioSource audio = BGImage.gameObject.AddComponent<AudioSource>();
        Debug.Log(path);
        WWW www = new WWW(path);
        yield return www;

        movie = www.movie;
        BGImage.texture = movie;
        audio.clip = movie.audioClip;
        // movie.loop = true;
        movie.Play();
        BGImage.color = Color.white;
        audio.Play();
    }


   
}
