using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class Global : MonoBehaviour {
    public GridContorll gridcontorll;
	public static List<string> childtexts = new List<string> ();
    public static List<int> sceneindex=new List<int>();
    public static Dictionary<string, string> MSGdic = new Dictionary<string, string>();
    public static int scene0=0,scene1=1,scene2=2,scene3=3;
    public static List<string> roomname=new List<string>();
    public static Dictionary<int, string> roomid=new Dictionary<int,string>();
    private static List<string> xmlnames=new List<string>();
	private static string testssss;
	private static List<string> testxmlnames=new List<string>();
    
	void Start () {
     
		//string xmlname = "20160528000143";
		//  string xmlpath="file://" + Application.dataPath + "/XML/RoomXML/" + xmlname + ".xml";    
		Debug.Log ("file://" + Application.dataPath + "/XML/RoomXML");
		GetDirectory (Application.dataPath + "/XML/RoomXML", xmlnames);
		testssss = GetFoldALL (Application.dataPath + "/XML");
		for (int m = 0; m < testxmlnames.Count; m++) {
//			Debug.Log (testxmlnames[m]);
		}
//		Debug.Log ("ssssssss" + testssss);
		//for (int q = 0; q < testxmlnames.Count; q++) {
			
		Debug.Log (testxmlnames.Count);
		for (int i = 0; i < testxmlnames.Count; i++) {
			ReadXml ("file://" + testxmlnames [i], testxmlnames [i]);
			if (testxmlnames [i].Contains ("RoomXML")) {
				gridcontorll.sceneindex = scene0;
			} else {
				gridcontorll.sceneindex = scene1;
			}

		}
	//  }
	}
    
	//测试获取指定路径下的文件夹及路径
	public static string GetFoldALL(string path)
	{
		string str = "";
		DirectoryInfo thisone = new DirectoryInfo (path);
		str = ListTreeShow (thisone,0,str);
		return str;
	}


	public static string ListTreeShow(DirectoryInfo theDir, int nLevel, string Rn)//递归目录 文件
	{
		DirectoryInfo[] subDirectories = theDir.GetDirectories();//获得目录
		foreach (DirectoryInfo dirinfo in subDirectories)
		{
			

			if (nLevel == 0)
			{
				Rn += "├";
			}
			else
			{
				string _s = "";
				for (int i = 1; i <= nLevel; i++)
				{
					_s += "│&nbsp;";
				}
				Rn += _s + "├";
			}
			Rn += "<b>" + dirinfo.Name.ToString() + "</b><br />";
			FileInfo[] fileInfo = dirinfo.GetFiles();   //目录下的文件
			foreach (FileInfo fInfo in fileInfo)
			{
				if (!fInfo.ToString().Contains("meta"))
				testxmlnames.Add (fInfo.ToString());
//				if (nLevel == 0)
//				{
//					Rn += "│&nbsp;├";
//				}
//				else
//				{
//					string _f = "";
//					for (int i = 1; i <= nLevel; i++)
//					{
//						_f += "│&nbsp;";
//					}
//					Rn += _f + "│&nbsp;├";
//				}
//				Rn += fInfo.Name.ToString() + " <br />";
			}
//			Rn = ListTreeShow(dirinfo, nLevel + 1, Rn);

		}
	

		return Rn;
	}

    //获取文件夹下的所有文件路径
	public static bool _VerifyPath(string path)
    {
        return System.IO.Directory.Exists(path);
    }
	public static void GetDirectory(string strDirName,List<string> XmlNames)
    {
   
        if (!_VerifyPath(strDirName)) { return; }
       
        string[] diArr = System.IO.Directory.GetDirectories(strDirName, "*", System.IO.SearchOption.AllDirectories);
        string[] rootfiArr = System.IO.Directory.GetFiles(strDirName);
	
        {
            if (rootfiArr != null)
            {
                foreach (string fi in rootfiArr) 
                {
                   
                        if (!fi.Contains("meta"))
                        {
						XmlNames.Add(fi);
                        }
                }
            }
        }
        foreach (string dri in diArr)
        {
          // Debug.Log(dri);
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dri);
            {
                if (dri != null)
                {
                    string[] fiArr = System.IO.Directory.GetFiles(dri);
                    if (fiArr != null)
                    {
                        foreach (string fi in fiArr) { Debug.Log(fi+"diarrrrrr"); }
                    }
                }
            }
        }
    }



    // 获取字符串中的数字；
    public static decimal GetNumber(string str)
    {
        decimal result = 0;
        if (str != null && str != string.Empty)
        {
            // 正则表达式剔除非数字字符（不包含小数点.）
            str = Regex.Replace(str, @"[^\d.\d]", "");
            // 如果是数字，则转换为decimal类型
            if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
            {
                result = decimal.Parse(str);
            }
        }
        return result;
    }

	public static void ReadXml(string path,string xmlname)
    {
        XmlDocument xmldoc = new XmlDocument();
        //WWW m_tempXml = new WWW(path);
        //yield return m_tempXml;
        
        
        xmldoc.Load(path);
        XmlNodeList topM = xmldoc.DocumentElement.ChildNodes;
            foreach (XmlNode nd in topM)
            {
			Debug.Log (path);
                if (nd.Name == "room_list")
                {
                    XmlNodeList nodelist = nd.ChildNodes;
                    if (nodelist.Count>0)
                    {
                        foreach (XmlElement el in nodelist)//读元素值
                        {
                            if (el.Attributes["room_id"].Value!="")
                            {
                              //  Debug.Log(el.Attributes["room_id"].Value);
                                roomname.Add(el.Attributes["room_id"].Value);
								childtexts.Add (el.Attributes["room_name"].Value);
                                if (!MSGdic.ContainsKey(xmlname))
                                {
                                    MSGdic.Add(xmlname, el.Attributes["room_id"].Value);

                                }
                               
                                foreach (string item in MSGdic.Keys)
                                {
                                    Debug.Log(item+"itemsssssssssssss");
                                }
                                foreach (string item in MSGdic.Values)
                                {
                                    Debug.Log(item + "valuessssssssssss");
                                }
                            }
                        }
                    }
                }
            }      
    }
  

    

    //遍历子物体，保存字典用于配置信息：
    public static void Loadinformation(GameObject parent) 
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
			if (parent.transform.GetChild(i).childCount>0)
			{
	            SortImage sortimage = parent.transform.GetChild(i).GetChild(0).GetComponent<SortImage>();
		            if (!roomid.ContainsValue(sortimage.id))
		            {
		                roomid.Add(i, sortimage.id);
		                sceneindex.Add(sortimage.scenetype);
		            }
	            Debug.Log(sortimage.scenetype);
	            Debug.Log(sortimage.id);
			}
        }
    }


    public static void CreatXML() 
    {
       // XmlDocument xml = new XmlDocument();
      //  xml.Load(path);

        Debug.Log("CreatXML()");
        XmlDocument doc = new XmlDocument();
        XmlNode declare = doc.CreateXmlDeclaration("1.0", "utf-8", "");
        doc.AppendChild(declare);
        XmlElement root = doc.CreateElement("Program");
        //root.SetAttribute("id", "http://www.test.com/test");
       // root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
        doc.AppendChild(root);
      //  XmlElement id = doc.CreateElement("id");
        Debug.Log(roomid.Count+"................");
        Debug.Log(sceneindex.Count);
        for (int i = 0; i < roomid.Count; i++)
        {
            XmlElement id = doc.CreateElement("id");
            //id.SetAttributeNode("layoutID", roomid[i]);
            Debug.Log("Roomid +++++++++++" + roomid[i]);
            id.SetAttribute("layoutID", roomid[i]);
            Debug.Log("id 的数值为" + id);
            id.SetAttribute("type", sceneindex[i].ToString());
            root.AppendChild(id);
            //id.SetAttribute("idtype", sceneindex[i].ToString());
        }
        //for (int j = 0; j < sceneindex.Count; j++)
        //{
        //    id.SetAttribute("idtype", sceneindex[j].ToString());
        //}
        
       

        //XmlElement classid = doc.CreateElement("classid");
        //classid.InnerText = "1000";
        //XmlElement classname = doc.CreateElement("classname");
        //classname.InnerText = "一年级";

        //classinfo.AppendChild(classid);
        //classinfo.AppendChild(classname);
        
        doc.Save(Application.dataPath+"/000.xml");
        Debug.Log(Application.dataPath + "/000.xml");
    }


	void Update () {
	
	}
}
