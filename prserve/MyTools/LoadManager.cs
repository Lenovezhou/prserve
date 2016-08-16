using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;
public enum InfoType : byte
{
    SendCompanyID = 0,
    SendHouseID,
    SendRoomID,
    SendProduceID,
    Bro_sendcompanyID,
    Bro_sendLayoutID,
    Bro_sendBooksID,
    Bro_sendPagesID,
    CompanyInfomation,
    Solution,
    ElementLayout
}
public class LoadManager : MonoBehaviour
{

    #region 初始化
    bool IsOnce;
    bool IsOnceCompany;
    private static LoadManager mInstance;
    /// <summary>
    /// 获取单例
    /// </summary>
    /// <returns></returns>
    public static LoadManager GetInstance()
    {
        if (mInstance == null)
        {
            mInstance = new GameObject("_LoadManager").AddComponent<LoadManager>();
        }
        return mInstance;
    }

    private LoadManager()
    {
        IsOnce = false;
    }
    #endregion

    #region 字段

    string strXMLModel = "";

    string localCompanyID = "";

    string HostServer = "http://101.231.255.158:8780/jzcl/wbp";

    #endregion


    #region SendXML

    /// <summary>
    /// 根据企业ID获取 House信息
    /// </summary>
    /// <param name="companyID"></param>
    public void BeginToLoadHouse(string companyID)
    {
        localCompanyID = companyID;
        strXMLModel = SendXML.SendCompanyId(companyID);
        start(strXMLModel, InfoType.SendCompanyID, companyID);
    }

    /// <summary>
    /// 根据HouseID获取Room信息
    /// </summary>
    /// <param name="houseID"></param>
    public void BeginToLoadRoom(string houseID)
    {
        strXMLModel = SendXML.SendHouseId(houseID);
        start(strXMLModel, InfoType.SendHouseID, houseID);
    }

    /// <summary>
    /// 根据HouseID和RoomID获取场景布局
    /// </summary>
    /// <param name="sceneId"></param>
    /// <param name="roomID"></param>
    public void BeginToLoadLayout(string sceneId, string roomID)
    {
        strXMLModel = SendXML.SendRoomId(sceneId, roomID);

        start(strXMLModel, InfoType.SendRoomID, roomID);
    }

    /// <summary>
    /// 根据企业ID和产品ID下载产品
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="produceID"></param>
    public void BeginToLoadProduce(string companyId, string produceID)
    {
        strXMLModel = SendXML.SendProduceID(companyId, produceID);

        start(strXMLModel, InfoType.SendProduceID, produceID);
    }

    /// <summary>
    /// 根据企业ID获取宣传册ID
    /// </summary>
    /// <param name="companyID"></param>
    public void BeginToLoadAllBooks(string companyID)
    {
        strXMLModel = SendXML.SendCompanyID_bro(companyID);

        start(strXMLModel, InfoType.Bro_sendcompanyID, companyID);
    }

    /// <summary>
    /// 根据宣传册ID获取布局
    /// </summary>
    /// <param name="companyID"></param>
    public void BeginToLoadBroLayout(string layoutId)
    {
        strXMLModel = SendXML.SendLayoutId(layoutId);

        start(strXMLModel, InfoType.Bro_sendLayoutID, layoutId);
    }

    /// <summary>
    /// 根据宣传册ID获取内页信息
    /// </summary>
    /// <param name="companyID"></param>
    public void BeginToLoadBook(string booksId)
    {
        strXMLModel = SendXML.SendBooksId(booksId);

        start(strXMLModel, InfoType.Bro_sendBooksID, booksId);
    }

    /// <summary>
    /// 根据内页ID获取内页信息
    /// </summary>
    /// <param name="companyID"></param>
    public void BeginToLoadPage(string pagesId)
    {
        strXMLModel = SendXML.SendPagesId(pagesId);

        start(strXMLModel, InfoType.Bro_sendPagesID, pagesId);
    }

    /// <summary>
    /// 根据企业ID获取企业信息
    /// </summary>
    /// <param name="companyID"></param>
    public void BeginToLoadCompany(string companyID)
    {
        strXMLModel = SendXML.SendCompanyInfomation(companyID);

        start(strXMLModel, InfoType.CompanyInfomation, companyID);
    }

    public void BeginToLoadSolution(string app_id)
    {
        strXMLModel = SendXML.SendCompanySolution(app_id);

        start(strXMLModel, InfoType.Solution, app_id);
    }

    public void BeginToLoadElement(string scene_id)
    {
        strXMLModel = SendXML.SendCompanyElement(scene_id);

        start(strXMLModel, InfoType.ElementLayout, scene_id);
    }
    #endregion


    /// <summary>
    /// 发送XML到服务器
    /// </summary>
    /// <param name="sendBuff">请求</param>
    /// <param name="infoTpye">类型</param>
    /// <param name="name">下载时候文件的名字</param>
    public void start(string sendBuff, InfoType infoTpye, string name = "1")
    {
        byte[] sendBuffs = Encoding.UTF8.GetBytes(sendBuff);
        StartCoroutine(LoadXML(HostServer, sendBuffs, infoTpye, name));
    }

    public IEnumerator LoadXML(string path, byte[] sendBuff, InfoType infoTpye, string name)
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
                List<string> strlist = new List<string>();
                strlist = ReadXML.ReadCompanyInfo(www.text, infoTpye);
                LoadNext(strlist, infoTpye);
            }
        }
        else
        {
            Debug.Log("下载失败.................");
        }
    }

    public void StartDownLoadXML(string file, string name, InfoType infoTpye)
    {

        byte[] model = Encoding.UTF8.GetBytes(file);
        int length = model.Length;

        string DirectoryPath = "";
        switch (infoTpye)
        {
            case InfoType.SendCompanyID:
                DirectoryPath = Application.dataPath + "/XML/PromotionHouse/";
                break;
            case InfoType.SendHouseID:
                DirectoryPath = Application.dataPath + "/XML/PromotionRoom/";
                break;
            case InfoType.SendRoomID:
                DirectoryPath = Application.dataPath + "/XML/PromotionLayout/";
                break;
            case InfoType.SendProduceID:
                DirectoryPath = Application.dataPath + "/XML/PromotionProduce/";
                break;
            case InfoType.Bro_sendcompanyID:
                DirectoryPath = Application.dataPath + "/XML/BrochureALL/";
                break;
            case InfoType.Bro_sendLayoutID:
                DirectoryPath = Application.dataPath + "/XML/BrochureLayout/";
                break;
            case InfoType.Bro_sendBooksID:
                DirectoryPath = Application.dataPath + "/XML/BrochureBooks/";
                break;
            case InfoType.Bro_sendPagesID:
                DirectoryPath = Application.dataPath + "/XML/BrochurePages/";
                break;
            case InfoType.CompanyInfomation:
                DirectoryPath = Application.dataPath + "/XML/ShowCompany/";
                break;
            case InfoType.Solution:
                DirectoryPath = Application.dataPath + "/XML/ShowSolution/";
                break;
            case InfoType.ElementLayout:
                DirectoryPath = Application.dataPath + "/XML/ShowLayout/";
                break;
            default:
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

    void CreateModelFile(string path, string name, byte[] info, int length)
    {
        Stream sw;
        FileInfo t = new FileInfo(path + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.Create();
            //以行的形式写入信息

        }
        else
        {
            //如果此文件存在则打开
            Debug.Log("文件已存在");
            return;
        }
        //sw.WriteLine(info);
        sw.Write(info, 0, length);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();

    }

    public void LoadNext(List<string> strlist, InfoType infoTpye)
    {
        switch (infoTpye)
        {
            case InfoType.SendCompanyID:
                foreach (string item in strlist)
                {
                    BeginToLoadRoom(item);
                }
                break;
            case InfoType.SendHouseID:
                foreach (string item in strlist)
                {
                    string[] temparray = item.Split('|');
                    BeginToLoadLayout(temparray[0], temparray[1]);
                }
                break;
            case InfoType.SendRoomID:
                foreach (string item in strlist)
                {
                    string[] temparray = item.Split('|');
                    //Debug.Log(temparray[0] + "        xxxxx    " + temparray[1]);
                    BeginToLoadProduce(temparray[0], temparray[1]);
                }
                break;
            case InfoType.SendProduceID:
                if (IsOnce == false)
                {
                    BeginToLoadAllBooks(localCompanyID);
                    IsOnce = true;
                }
                break;
            case InfoType.Bro_sendcompanyID:
                foreach (string item in strlist)
                {
                    BeginToLoadBroLayout(item);
                    BeginToLoadBook(item);
                }
                break;
            case InfoType.Bro_sendLayoutID:
                break;
            case InfoType.Bro_sendBooksID:
                foreach (string item in strlist)
                {
                    BeginToLoadPage(item);
                }
                break;

            case InfoType.Bro_sendPagesID:
                if (IsOnceCompany == false)
                {
                    BeginToLoadCompany(localCompanyID);
                    IsOnceCompany = true;
                }
                break;
            case InfoType.CompanyInfomation:
                foreach (var item in strlist)
                {
                    BeginToLoadSolution(item);
                }
                break;
            case InfoType.Solution:
                foreach (var item in strlist)
                {
                    BeginToLoadElement(item);
                }
                break;
            default:
                break;
        }

    }
}
