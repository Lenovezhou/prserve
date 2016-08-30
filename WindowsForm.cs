using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class WindowsForm : MonoBehaviour
{
    //public WebCamTexture cameraTexture;
    //public string cameraName = "";
    // Use this for initialization  
    public static string filePath = "";
    public Button mybutton;
    void Start()
    {
        mybutton.onClick.AddListener(ClickButton);
    }

    public void ClickButton()
    {
        OpenFileName ofn = new OpenFileName();

        ofn.structSize = Marshal.SizeOf(ofn);

        //ofn.filter = "All Files\0*.*\0\0";


        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;

        ofn.initialDir = UnityEngine.Application.dataPath;//默认路径  

        ofn.title = "Open Project";

        ofn.defExt = "OGG";//显示文件的类型  

        ofn.filter = "视频文件(*.ogg)\0*.ogg;*.mp4;*.mov;*.avi;";
        //ofn.filter = "音频文件(*.ogg)\0*.ogg;*.wav;*.mp3";
        //注意 一下项目不一定要全选 但是0x00000008项不要缺少  
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  

        if (WindowDll.GetOpenFileName(ofn))
        {
            filePath = ofn.file;
            Debug.Log("找到文件了");
            filePath = "file://" + filePath;
           // LoadMusic._instance.Loadmusic(filePath);
            Debug.Log("Selected file with full path:  " + ofn.file);
        }
    }

}
