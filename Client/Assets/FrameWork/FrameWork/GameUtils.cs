using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public static class GameUtils
{
    /// <summary>
    /// 清除子节点
    /// </summary>
    /// <param name="tran"></param>
    public static void ClearChildren(Transform tran)
    {
        int len = tran.childCount;
        //List<GameObject> list = new List<GameObject>();
        GameObject obj;
        for (int i = 0; i < len; i++)
        {
            obj = tran.GetChild(i).gameObject;
            obj.SetActive(false);
            GameObject.Destroy(obj);
        }
    }

    /// <summary>
    /// 获取裁剪后的文字
    /// </summary>
    /// <param name="s">文字</param>
    /// <param name="label">label的长宽需要是固定的</param>
    /// <returns></returns>
    public static string GetClampText(string s, Text label)
    {
        string strOut = string.Empty;
        if (!string.IsNullOrEmpty(s))
        {
            label.text = s;

            //bool bWarp = label.Wrap(s, out strOut, label.height);
            //if (!bWarp)
            //{
            //    strOut = strOut.Substring(0, strOut.Length - 1);
            //    strOut += "...";
            //}
        }
        return strOut;
    }

    /// <summary>
    /// 获取裁剪后的文字
    /// </summary>
    /// <param name="s">文字</param>
    /// <param name="label">label的长宽需要是固定的</param>
    /// <param name="changeLen">替换文字的长度</param>
    /// <param name="changeStr">替换文字</param>
    /// <returns></returns>
    public static string GetClampText(string s, Text label, int changeLen, string changeStr)
    {
        string strOut = string.Empty;
        if (!string.IsNullOrEmpty(s))
        {
            label.text = s;
            //NGUIText.dynamicFont = label.trueTypeFont;
            //NGUIText.fontSize = label.finalFontSize;
            //NGUIText.finalSize = label.finalFontSize;
            //// 当前配置下的UILabel是否能够包围Text内容
            //// Wrap是NGUI中自带的方法，其中strContent表示要在UILabel中显示的内容，strOur表示处理好后返回的字符串，uiLabel.height是字符串的高度 。
            //bool bWarp = label.Wrap(s, out strOut, label.height);
            //// 如果不能，就是说Text内容不能全部显示，这个时候，我们把最后一个字符去掉，换成省略号"..."
            //if (!bWarp && !string.IsNullOrEmpty(strOut))
            //{
            //    strOut = strOut.Substring(0, strOut.Length - changeLen);
            //    strOut += changeStr;
            //}
        }
        return strOut;
    }


    /// <summary>
    /// 显示错误提示
    /// </summary>
    /// <param name="code"></param>
    public static void ShowErrorTips(int code)
    {
        //Global.Inst.GetController<CommonTipsController>().ShowTips(CodeErrorTips.GetTips(code));
    }

    public static void ShowErrorTips(string tips)
    {
        //Global.Inst.GetController<CommonTipsController>().ShowTips(tips);
    }

    /// <summary>
    /// 获取图片转成base64
    /// </summary>
    /// <param name="pic"></param>
    /// <returns></returns>
    public static string GetTextureBase64(Texture pic)
    {
        Texture2D tex = GameObject.Instantiate(pic) as Texture2D;
        byte[] b = null;
        string f = tex.format.ToString();
        if (f.Contains("ARGB") || f.Contains("RGBA") || f.Contains("Alpha"))
        {
            b = tex.EncodeToPNG();
        }
        else
        {
            b = tex.EncodeToJPG();
        }

        GameObject.Destroy(tex);
        return System.Convert.ToBase64String(b);
    }


    /// <summary>
    /// 获取图片转成base64
    /// </summary>
    /// <param name="pic"></param>
    /// <returns></returns>
    public static string[] GetTextureBase64(List<Texture> pic)
    {
        if (pic == null)
        {
            return null;
        }

        string[] picStr = new string[pic.Count];
        for (int i = 0; i < pic.Count; i++)
        {
            picStr[i] = GetTextureBase64(pic[i]);
        }
        return picStr;
    }


    #region 通过int得到一个倒计时的string标识：hh:mm:ss

    /// <summary>
    /// 通过int得到一个倒计时的string标识：hh:mm:ss
    /// </summary>
    /// <param name="time">倒计时时间，int</param>
    /// <returns>倒计时格式------>hh:mm:ss</returns>
    public static string GetLastTime(int time)
    {
        int hh = (int)(time / 3600.0f);
        int mm = (int)(((time - hh * 3600) / 3600.0f) * 60);
        int ss = time - hh * 3600 - mm * 60;

        string shh;
        string smm;
        string sss;

        if (hh < 10)
        {
            shh = "0" + hh;
        }
        else
        {
            shh = hh + "";
        }

        if (mm < 10)
        {
            smm = "0" + mm;
        }
        else
        {
            smm = mm + "";
        }


        if (ss < 10)
        {
            sss = "0" + ss;
        }
        else
        {
            sss = ss + "";
        }

        return shh + ":" + smm + ":" + sss;
    }

    #endregion

    /// <summary>
    /// 将秒转化成hh:mm
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string GetTimeChangeHHMM(int time)
    {
        int h = time / 3600;//小时
        int m = (time - h * 3600) / 60;//分钟
        string t = (h < 10 ? ("0" + h) : h.ToString()) + ":" + (m < 10 ? ("0" + m) : m.ToString());
        return t;
    }

    /// <summary>
    /// 通过hh:mm的数据，得到int类型的时间
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int GetTimeIntByString(string str)
    {
        string[] temp = str.Split(':');
        int hh = int.Parse(temp[0]);//1小时是3600秒
        int mm = int.Parse(temp[1]);//1分钟是60秒
        return hh * 3600 + mm * 60;
    }
    /// <summary>
    /// 设置父物体   初始化Transform
    /// </summary>
    /// <param name="childTran">设置的子物体</param>
    /// <param name="paretTran">父物体</param>
    /// <param name="targetTran">目标位置</param>
    public static void SetItemParet(Transform childTran, Transform paretTran, Transform targetTran)
    {
        childTran.gameObject.transform.parent = paretTran;
        childTran.localPosition = targetTran.localPosition;
        childTran.localRotation = targetTran.localRotation;
        childTran.localScale = targetTran.localScale;
    }
    /// <summary>
    /// 限制输入的数
    /// </summary>
    /// <param name="inputNum">需要限制的数</param>
    /// <param name="bet"> 获得的货币</param>
    /// <returns></returns>
    public static int LimitInputNum(int inputNum, int getNum)
    {
        if (inputNum <= 0)
        {
            return 0;
        }
        if (inputNum <= getNum)
        {
            return getNum;
        }
        return (inputNum / getNum) * getNum;

    }
    /// <summary>
    /// 单位 改变
    /// </summary>
    /// <param name="num">输入的数</param> 
    /// <param name="betnum"> 倍数</param>
    /// <param name="numf">小数点位数</param>
    /// <returns></returns>
    public static string ChangeNUM(int num, int betnum = 1000)
    {
        double newNum;
        if (Mathf.Abs(num) < Mathf.Abs(betnum))
        {
            return num.ToString();
        }
        else
        {
            newNum = num * 1.0 / betnum;
        }
        return new StringBuilder(newNum.ToString("0.0")).Append("k").ToString();
    }

    /// <summary>
    /// 复制手机粘贴板
    /// </summary>
    /// <param name="str">复制内容</param>
    public static void CopyStrPhone(string str,bool show=true)
    {
#if UNITY_EDITOR
        TextEditor textEditor = new TextEditor();
        textEditor.text = str;
        textEditor.OnFocus();
        textEditor.Copy();
#else
        //SixqinSDKManager.Inst.SendMsg(SixqinSDKManager.COPY_TEXT, str);
#endif
        if (!string.IsNullOrEmpty(str))
        {
            if (show)
            {
                ShowErrorTips("复制成功！");
            }
           
        }
    }

    /// <summary>
    /// 获取复制文本
    /// </summary>
    /// <returns></returns>
    public static void GetPhoneCopy(CallBack<string> call = null)
    {

#if UNITY_ANDROID

        string str = "";

        //str = SixqinSDKManager.Inst.SendMsg<string>(SixqinSDKManager.Get_COPY_TEXT) as string;
        //if (call != null)
        //{
        //    call(str);
        //}


#elif UNITY_IPHONE
     SixqinSDKManager.Inst.GetPhoneCopy(call); 
#endif
    }

    /// <summary>
    /// 通过经纬度获取距离
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    //public static float GetDistance(Location from, Location to)
    //{
    //    float dis = 0;
    //    float R = 6371.004f;  //按公里算
    //    float angle = Mathf.Sin(from.latitude) * Mathf.Sin(to.latitude) * Mathf.Cos(from.longitude - to.longitude)
    //        + Mathf.Cos(from.latitude) * Mathf.Cos(to.latitude);
    //    dis = R * Mathf.Acos(angle) * Mathf.PI / 180;

    //    dis = float.Parse(string.Format("{0:N2}", dis));

    //    return dis;
    //}

    /// <summary>
    /// 转换输赢分数
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public static string ToScoreStr(this int score)
    {
        if (score >= 0)
        {
            return "+" + score;
        }

        return score.ToString();
    }

    /// <summary>
    /// 转换输赢分数
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public static string ToScoreStr(this float score)
    {
        string str;
        if (score >= 0)
        {
            str = "+" + score;
        }
        else
        {
            str = score.ToString();
        }

        str.TrimEnd('0');

        return str;
    }
    /// <summary>
    /// 判断两个列表是否相同
    /// </summary>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <returns></returns>
    public static bool IsSame(List<int> list1, List<int> list2)
    {

        if (list1 == null || list2 == null)
        {
            return false;
        }
        if (list1.Count != list2.Count)
        {
            return false;
        }
        bool b = true;
        list1.Sort();
        list2.Sort();

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i])
            {
                b = false;
                break;
            }
        }


        return b;
    }
    /// <summary>
    /// 字符串中提取数字
    /// </summary>
    public static int GetNumFromStr(string str)
    {
        int num = 0;

        string result = Regex.Replace(str, @"[^0-9]+", "");
        Debug.Log("lalla" + result);
        if (int.TryParse(result, out num))
        {

        }
        return num;
    }
    /// <summary>
    /// 从字符串中拿房间号
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int GetRoomIdFromStr(string str)
    {
        int roomId = 0;
        string temp = "";
        if (str.Contains("房间:"))
        {
            int index = str.IndexOf("房间:", 0);

            temp = str.Substring(index + 3, 6);

        }

        int.TryParse(temp, out roomId);
        return roomId;
    }
    /// <summary>
    /// 获取当前时间戳
    /// </summary>
    /// <returns></returns>
    public static double GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }

    /// <summary>
    /// 通过时间戳转换时间
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime ConvertStringToDateTime(string timeStamp)
    {


        long begtime = Convert.ToInt64(timeStamp) * 10000000;
        DateTime dt_1970 = new DateTime(1970, 1, 1, 8, 0, 0);
        long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度
        long time_tricks = tricks_1970 + begtime;//日志日期刻度
        DateTime dt = new DateTime(time_tricks);//转化为DateTime
        return dt;
    }

}
