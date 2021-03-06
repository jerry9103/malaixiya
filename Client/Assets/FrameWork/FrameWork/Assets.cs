using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.UI;

public class Assets : MonoBehaviour
{

    private static Assets Inst;
    private Dictionary<string, Sprite> CachTexture = new Dictionary<string, Sprite>();//缓存的图片
    private List<string> CachTextureList = new List<string>();
    private const string mSendUrl = "http://h5future.com/interface.php";//发送图片链接


    private void Awake()
    {
        Inst = this;
    }


    #region 加载图片
    /// <summary>
    /// 加载本地图片
    /// </summary>
    /// <param name="url"></param>
    /// <param name="call"></param>
    public static void LoadLocalIcon(string url, Image img)
    {
        Inst.StartCoroutine(Inst.LoadTexture(true, url, (sp) =>
        {
            img.sprite = sp;
        }));
    }

    public static void LoadLocalSp(string url, CallBack<Sprite> call)
    {
        Inst.StartCoroutine(Inst.LoadTexture(true, url, call));
    }

    /// <summary>
    /// 加载头像
    /// </summary>
    /// <param name="url"></param>
    /// <param name="call"></param>
    public static void LoadIcon(string url, CallBack<Sprite> call, bool room = true)
    {
        Inst.StartCoroutine(Inst.LoadTexture(false, url, call, room));
    }

    /// <summary>
    /// 加载头像
    /// </summary>
    /// <param name="url"></param>
    /// <param name="tex"></param>
    /// <param name="room"></param>
    public static void LoadIcon(string url, Image tex, bool room = true)
    {
        LoadIcon(url, (t) =>
        {
            tex.sprite = t;
        }, room);
    }

    IEnumerator LoadTexture(bool isRes, string url, CallBack<Sprite> calback, bool room = true)
    {
        if (string.IsNullOrEmpty(url))
        {
            if (calback != null)
                calback(null);
        }
        else
        {
            if (room)
            {
                if (CachTexture.ContainsKey(url))
                {
                    calback(CachTexture[url]);
                }
                else
                {
                    Inst.StartCoroutine(LoadIcon(isRes, url, calback, room));
                }
            }
            else
            {
                Inst.StartCoroutine(LoadIcon(isRes, url, calback, room));
            }

        }
        yield return null;
    }


    private IEnumerator LoadIcon(bool isRes, string url, CallBack<Sprite> calback, bool room = true)
    {
        Texture2D obj = null;
        if (isRes)//在resource里面加载
        {
            obj = Resources.Load(url) as Texture2D;
            if (null == obj)
                SQDebug.LogError("加载错误：" + url);
            if (null != calback)
            {

                var sp = Sprite.Create(obj, new Rect(0, 0, obj.width, obj.height), new Vector2(0.5f, 0.5f));

                AddTexture(url, sp);
                calback(sp);
            }
        }
        else//在网络地址中加载
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                if (null != calback)
                {
                    var sp = Sprite.Create(www.texture, new Rect(0, 0, www.texture.texelSize.x, www.texture.texelSize.y), new Vector2(0.5f, 0.5f));
                    AddTexture(url, sp);
                    calback(sp);
                }

                Resources.UnloadUnusedAssets();
            }
            if (!string.IsNullOrEmpty(www.error))
            {
                SQDebug.LogError("加载错误：" + url + "      " + www.error);
            }
            www = null;
        }
    }

    private void AddTexture(string url, Sprite obj)
    {
        if (CachTexture.ContainsKey(url))
            return;
        if (obj != null)
        {
            CachTexture[url] = obj;
            CachTextureList.Add(url);
            if (CachTextureList.Count > 20)
            {
                string t = CachTextureList[0];
                CachTexture.Remove(t);
                CachTextureList.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// 刷新缓存图片
    /// </summary>
    /// <param name="url"></param>
    /// <param name="obj"></param>
    public static void UpdateTexture(string url, Sprite obj)
    {
        if (!string.IsNullOrEmpty(url) && obj == null && Inst.CachTexture.ContainsKey(url))
            Inst.CachTexture[url] = obj;

    }
    #endregion

    #region 加载配置表
    /// <summary>
    /// 添加配置表
    /// </summary>
    /// <param name="url"></param>
    /// <param name="call"></param>
    public static void LoadConfig(string url, CallBack<AssetBundle> call, int cachid)
    {
        Inst.StartCoroutine(Inst.LoadConfigIE(url, call, cachid));
    }


    IEnumerator LoadConfigIE(string url, CallBack<AssetBundle> call, int cachid)
    {
        WWW www = null;
        if (Application.platform == RuntimePlatform.WindowsEditor)
            www = new WWW(url);
        else
            www = WWW.LoadFromCacheOrDownload(url, cachid);

        yield return www;
        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            if (null != call)
                call(www.assetBundle);
        }
        if (!string.IsNullOrEmpty(www.error))
            SQDebug.LogError("加载错误：" + url + "      " + www.error);
    }

    #endregion


    #region 加载预制体
    /// <summary>
    /// 加载预制体
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static GameObject LoadPrefab(string path)
    {
        GameObject obj = Resources.Load(path) as GameObject;
        if (obj == null)
            SQDebug.LogWarning("加载路径：" + path + "  错误");
        return obj;
    }
    #endregion

    #region 上传图片
    /// <summary>
    /// 路径头
    /// </summary>
    public static string FinalPathHead
    {
        get
        {
            string url = "";
            if (Application.platform == RuntimePlatform.Android)
                url = "jar:file://";
            else
                url = "file:///";
            return url;
        }
    }

    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="img">图片</param>
    /// <param name="call">回调方法，返回的链表长度为0表示上传失败</param>
    public static void SendPng(string[] img, CallBack<List<string>> call)
    {
        Inst.StartCoroutine(Inst.SendPngToServer(img, call));
    }
    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="img"></param>
    /// <param name="call"></param>
    /// <returns></returns>
    IEnumerator SendPngToServer(string[] img, CallBack<List<string>> call)
    {
        List<string> imgurl = new List<string>();
        if (img != null && img.Length > 0)
        {
            for (int i = 0; i < img.Length; i++)
            {
                WWWForm f = new WWWForm();
                f.AddField("act", "upPic");
                f.AddField("picBin", img[i]);

                WWW www = new WWW(mSendUrl, f);
                yield return www;
                if (www.isDone && string.IsNullOrEmpty(www.error))
                {
                    imgurl.Add(www.text);
                    SQDebug.Log("上传图片：" + www.text);
                }
                if (!string.IsNullOrEmpty(www.error))
                    SQDebug.Log("加载错误：第" + (i + 1) + "张      " + www.error);
                www = null;
            }
        }

        if (call != null)
            call(imgurl);
        yield return 0;
    }
    #endregion

    #region 加载战绩文件

    /// <summary>
    /// 加载txt文件
    /// </summary>
    /// <param name="url"></param>
    public static void LoadTxtFile(string url, CallBack<string> call)
    {
        Inst.StartCoroutine(Inst.IELoadTxtFile(url, call));
    }

    /// <summary>
    /// 协议加载txt文件
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator IELoadTxtFile(string url, CallBack<string> call)
    {
        Debug.Log("加载的text地址:" + url);
        WWW www = new WWW(url);
        yield return www;
        Debug.Log("加载的text 返回:" + url);
        if (www.isDone && www.error == null)
        {
            if (call != null)
            {
                SQDebug.Print("从连接地址:--->" + url + "   加载的txt文件内容为:--->" + www.text);
                call(www.text);
            }
            www.Dispose();
        }
        else
        {
            if (call != null)
            {
                SQDebug.Print("从连接地址：--->" + url + "   加载txt数据失败");
                call("");
            }
        }
        www = null;
    }


    /// <summary>
    /// 加载Byte文件
    /// </summary>
    /// <param name="url"></param>
    public static void LoadByteFile(string url, CallBack<byte[]> call)
    {
        Inst.StartCoroutine(Inst.IELoadByteFile(url, call));
    }

    /// <summary>
    /// 协议加载Byte[]文件
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator IELoadByteFile(string url, CallBack<byte[]> call)
    {
        yield return null;
        WWW www = new WWW(url);
        yield return www;
        if (www.isDone && www.error == null)
        {
            if (call != null)
            {
                call(www.bytes);
            }
            www.Dispose();
        }
        else
        {
            if (call != null)
            {
                SQDebug.Print("从连接地址：--->" + url + "   加载Byte[]数据失败");
                call(null);
            }
        }
        www = null;
    }

    #endregion


}

