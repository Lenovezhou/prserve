using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml;
using UnityEngine.UI;
using System.Text;

public class LoadDictionary : MonoBehaviour
{
    public static LoadDictionary _instans;
   // private List<string> localxmlnames;
    private List<string> picpath=new List<string>();
    private string localpath = "http://ftp563246.host532.zhujiwu.me/temp.xml";
    private string picurl = "http://101.231.255.158:8780/jzcl/";
    private List<string> xmltext = new List<string>();
    private Button updatabutton;
  

    void Awake()
    {
       
       // updatabutton.onClick.AddListener();

        _instans = this;
      //  localxmlnames = new List<string>();
    }

    public void LoadXML()
    {
      //  StartCoroutine(StartDownLoadXML(localpath));
    }
    public void LoadPic(List<string> picpath)
    {
        for (int i = 0; i < picpath.Count; i++)
        {
            ReadXML(picpath[i]);//读取本地XML
        }
       
        StartCoroutine(StartDownLoadPic(xmltext));//获取的图片连接
        Debug.Log(xmltext.Count);
    }
    /// <summary>
    /// 从服务器下载图片文件并保存
    /// </summary>
    /// <param name="xmltext"></param>
    /// <returns></returns>
    IEnumerator StartDownLoadPic(List<string> xmltext)
    {
        Debug.Log(xmltext.Count+">>>>>>>>>>>>");
        for (int i = 0; i < xmltext.Count; i++)
        {
          //www(网址)
            WWW mywww = new WWW(picurl+xmltext[i]);
            yield return mywww;
            Debug.Log(mywww.isDone+"mywww.isdone"+mywww.error);
            //保险条件
            if (mywww.isDone)
            {
                Debug.Log(xmltext[i]+"oooowwwwwwwww");
                DirectoryInfo folder = new DirectoryInfo(Application.dataPath + "/Resources/" + xmltext[i].Substring(1, 6));                
                FileInfo fileinfo = new FileInfo(Application.dataPath + "/Resources"+ xmltext[i]);
                //如果不存在Resources文件夹则创建
                if (!folder.Exists)
                {
                    Debug.Log("文件夹不存在，创建中");
                    AssetDatabase.CreateFolder("Assets/Resources", xmltext[i].Substring(1, 6));
                }
                //如果不存在 name.xml文件则创建
                if (!fileinfo.Exists)
                {
                 
                    Texture2D myTexture = mywww.texture;
                    byte[] data = myTexture.EncodeToPNG();
                    string localpath = Application.dataPath + "/Resources/" + xmltext[i];
                    Debug.Log(localpath);
                    FileStream stream = new FileStream(localpath, FileMode.OpenOrCreate);
                    stream.Write(data, 0, data.Length);
                    Debug.Log("下载成功");
                   
                  
                  //  LoadPic("2015001");
                }
                }
               
        }
    }
    
    public void StartDownLoadXML(string filePath,string name)
    {
     
        //WWW w = new WWW(filePath);
        //yield return w;
        //if (w.isDone)
        {
            byte[] model = Encoding.UTF8.GetBytes(filePath);
            int length = model.Length;
            //写入xml到本地
            string localpath = Application.dataPath + name+".xml";
            DirectoryInfo xmlinfo = new DirectoryInfo(Application.dataPath + "/Resources/addXML");
            FileInfo fileinfo = new FileInfo(Application.dataPath + "/Resources/addXML/" + name + ".xml");
            //如果不存在Resources文件夹则创建
            if (!xmlinfo.Exists)
            {
                Debug.Log("文件夹不存在，创建中");
                AssetDatabase.CreateFolder("Assets/Resources", "addXML");
            }
            //如果不存在 name.xml文件则创建
            if (!fileinfo.Exists)
            {
                Debug.Log("文件不存在，创建中");
                CreateModelFile(Application.dataPath + "/Resources/addXML", name + ".xml", model, length);
                picpath.Add(Application.dataPath + "/Resources/addXML/"+name+".xml");
                Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"+picpath.Count);
                LoadPic(picpath);
               // localxmlnames.Add(name);
            }
            
        }
    }

    void CreateModelFile(string path, string name, byte[] info, int length)
    {
        // DirectoryInfo xmlinfo = new DirectoryInfo(Application.dataPath + "XML");
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
    /// "读取xml中的picture_URL"
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
        public void ReadXML(string path)
        {
            XmlDocument myXML = new XmlDocument();
            myXML.Load(path);
            //myXML(path);
            Debug.Log(path);
            XmlElement Xmlroot = myXML.DocumentElement;
            Debug.Log("8888888888888" + Xmlroot.Name);
            //XmlNode node = myXML.FirstChild;
            XmlNodeList nodelist = Xmlroot.ChildNodes;
            Debug.Log("7777777777" + nodelist.Count);
            foreach (XmlNode nd in nodelist)
            {
                if(nd.Name == "brochure_list")
                {
                    XmlElement element = (XmlElement)nd;

                    xmltext.Add(element.GetAttribute("catalog_audio_url"));
                    Debug.Log("6666666666666666666" + xmltext[0]);
                    //XmlAttributeCollection a = element.Attributes;
                    //Debug.Log("qqqqqqqqqqqqqqqqqqqq" + element.Attributes["catalog_audio_url"].Value);
                    //Debug.Log("66666666666666666 " + element.Attributes);
                    //foreach(XmlElement b in a)
                    //{
                    //    Debug.Log("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
                    //    if(b.Name == "catalog_audio_url")
                    //    {
                    //        Debug.Log("==================================");
                    //    }
                    //}
                }
               
            }
            //Debug.Log(" 1111111111111111111111111 -" + Xmlroot["brochure_list"].Value + "- 1111111111111111111111111");
            //if (Xmlroot["brochure_list"] != null)
            //{
            // Debug.Log("222222222222222222222");
            // XmlAttributeCollection a=  Xmlroot["brochure_list"].Attributes;
            // Debug.Log("555555555 --" + a.Count);
            // foreach (XmlElement item in a)
            //    {
            //        Debug.Log(item.Value+"33333333333333333333");
            //        if (item.Value.Contains("png"))
            //        {
            //            Debug.Log(item.Value+"4444444444444444444444");
            //            xmltext.Add(item.Value);
            //        }
            //    }
            //    foreach (XmlNode item in Xmlroot["catalog_audio_url"].ChildNodes)
            //    {

            ////        Debug.Log(item.Attributes["catalog_audio_url"].Value + "Valueeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
            //        xmltext.Add(item.Attributes["catalog_audio_url"].Value);
            //    }
                //Debug.Log( "Valueeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
            //}
          //  Debug.Log("Valueeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee"+path);
        }



 }
