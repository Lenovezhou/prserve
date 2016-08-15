using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using System.Collections;

public class ReadXML
{
    public static List<Module> ReadInfo(string path, string Name)
    {
        List<Module> Info = new List<Module>();
        System.IO.StringReader stringReader = new System.IO.StringReader(path);
        stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        Debug.Log(Xmlroot[Name].ChildNodes.Count);
        ///只要调用这个方法，不管读取哪个节点都将电话号码赋值给  uimanager._instance.cellphonenumber
        UImanager._instance.cellphonenumber = Xmlroot["G"].Attributes["cellphonenumber"].Value;
        foreach (XmlNode item in Xmlroot[Name].ChildNodes)
        {
            Module info = new Module();
            
            info.Englishname = item.Attributes["englishname"].Value; 
            info.Name = item.Attributes["name"].Value;
            
            info.SubList = new List<Sub>();
            foreach (XmlNode item1 in item.ChildNodes)
            {
                Sub sub = new Sub();
                sub.Name = item1.Attributes["name"].Value;
                sub.ENglishname = item1.Attributes["englishname"].Value;
                info.SubList.Add(sub);
                sub.ProductList = new List<Product>();
                foreach (XmlNode item2 in item1.ChildNodes)
                {
                    Product prod = new Product();
                    prod.Name = item2.Attributes["name"].Value;
                    prod.ModleURL = item2.Attributes["modle"].Value;
                    prod.TextureURL = item2.Attributes["texture"].Value;
                    prod.Description =item2.Attributes["description"].Value;
                    sub.ProductList.Add(prod);
                }
            }
            Info.Add(info);
        }
        return Info;
    }
}
