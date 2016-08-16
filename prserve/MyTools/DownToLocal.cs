using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class DownToLocal : MonoBehaviour
{

    public Button myButton;
    void Start()
    {
        myButton.onClick.AddListener(DownAsset);
    }

    void Update()
    {

    }
    string path = "http://ftp563246.host532.zhujiwu.me/Picture/dimian/0.png";
    void DownAsset()
    {
        StartCoroutine(StartDownLoadPic(path));
    }
    
    IEnumerator StartDownLoadPic(string filePath)
    {
        WWW mywww = new WWW(filePath);
        yield return mywww;
        Texture2D myTexture = mywww.texture;
        byte[] data=myTexture.EncodeToPNG();
        string localpath=Application.dataPath+"/image.PNG";
        FileStream stream = new FileStream(localpath, FileMode.OpenOrCreate);
        stream.Write(data, 0, data.Length);
        Debug.Log("下载成功");
    }

    IEnumerator StartDownLoadMovie(string filePath)
    {
        WWW w = new WWW(filePath);
        yield return w;
        if (w.isDone)
        {
            byte[] model = w.bytes;
            int length = model.Length;
            //写入模型到本地
            CreateModelFile(Application.dataPath, "1.ogg", model, length);
        }
    }

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
}
