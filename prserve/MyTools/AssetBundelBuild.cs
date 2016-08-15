using UnityEngine;
using UnityEditor;
using System.Collections;

public class BuildAssetbundle : Editor
{
   
    [MenuItem("Custom Editor/Build SingleOcject AssetBunldes")]
    static void CreateAssetBunldesMain()
    {
        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        //遍历所有的游戏对象
        foreach (Object obj in SelectedAsset)
        {
            string sourcePath = AssetDatabase.GetAssetPath(obj);
            string targetPath = Application.dataPath + "/StreamingAssets/" + SelectedAsset[0].name + ".assetbundle";
            if (BuildPipeline.BuildAssetBundle(obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies|BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android))
            { 
                Debug.Log(obj.name + "资源打包成功");
            }
            else
            {
                Debug.Log(obj.name + "资源打包失败");
            }
        }
        //刷新编辑器
        AssetDatabase.Refresh();

    }



    [MenuItem("Custom Editor/Create ALLOcject AssetBunldes")]
    static void CreateAssetBunldesALL()
    {

        Caching.CleanCache();
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        string Path = Application.dataPath + "/StreamingAssets/"+SelectedAsset[0].name.Substring(0,1)+".assetbundle";

        foreach (Object obj in SelectedAsset)
        {
            Debug.Log("Create AssetBunldes name :" + obj);
        }

        //这里注意第二个参数就行
        if (BuildPipeline.BuildAssetBundle(null, SelectedAsset, Path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android))
        {
            AssetDatabase.Refresh();
        }
        else
        {

        }
    }
    [MenuItem("Custom Editor/Create ALLPicture AssetBunldes")]
    static void CreateALLPicture()
    {

        Caching.CleanCache();
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        string Path = Application.dataPath + "/StreamingAssets/" + SelectedAsset[0].name.Substring(0, 1) + ".assetbundle";

        foreach (Object obj in SelectedAsset)
        {
            Debug.Log("Create AssetBunldes name :" + obj);
        }

        //这里注意第二个参数就行
        if (BuildPipeline.BuildAssetBundle(null, SelectedAsset, Path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android))
        {
            AssetDatabase.Refresh();
        }
        else
        {

        }
    }
}