using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using Newtonsoft.Json;//这是json.Net的命名空间
using System.Runtime.InteropServices;
using ProtoBuf;
using System.IO;

public class MessageData
{
    /// <summary>
    /// 网络返回的数据
    /// 
    /// </summary>
    public byte[] msgData { get; private set; }

    private delegate object ReadDelegate(Type t, byte[] data, int index, int readCount, bool needShowLog);
    private delegate byte[] WriteDelegate(object o);

    private string msgName { get; set; }

    public MessageData() { }

    public MessageData(byte[] data)
    {
        msgData = data;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o"></param>
    /// <param name="cmdNum"></param>
    public void Write<T>(object o, string msgName)
    {
        this.msgName = msgName;
        if (!msgName.Equals(MessageProtoId.HeartRes))
            SQDebug.Log("发送消息的Id：" + msgName + "   消息内容:" + JsonConvert.SerializeObject(o));

        ClientMsgHead msgHead = new ClientMsgHead();
        msgHead.msgname = msgName;
        msgHead.msgtype = 3;

        byte[] data = SerializeData<T>((T)o);
        byte[] headData = SerializeData(msgHead);
        short headSize = (short)headData.Length;
        int msgSize = 2 + headData.Length + data.Length;

        int msgAllLen = (int)(msgSize + 4);
        msgData = new byte[msgAllLen];
        //这里采用的是大端的存储方式 长度是数据的长度 不包括字节本身
        //包头存储包体大小
        byte[] len = new byte[4];
        len[0] = (byte)(msgSize >> 24);
        len[1] = (byte)(msgSize >> 16);
        len[2] = (byte)(msgSize >> 8);
        len[3] = (byte)(msgSize);
        Array.Copy(len, 0, msgData, 0, 4);
        //头部消息大小
        len = new byte[2];
        len[0] = (byte)(headSize >> 8);
        len[1] = (byte)headSize;
        Array.Copy(len, 0, msgData, 4, 2);
        //end
        //头部消息
        Array.Copy(headData, 0, msgData, 6, headSize);
        //消息体
        Array.Copy(data, 0, msgData, headSize + 6, data.Length);
    }

    /// <summary>
    /// 解析数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="showLog"></param>
    /// <returns></returns>
    public T Read<T>(bool showLog = false)
    {
        msgName = typeof(T).Name;
        T o = DeserializeData<T>(msgData);
        if (!msgName.Equals(MessageProtoId.HeartRes))
            SQDebug.Log(string.Format("接收到的消息：{0} {1}", msgName, JsonConvert.SerializeObject(o)));
        return o;
    }

    public ClientMsgHead ReadHead()
    {
        ClientMsgHead head = DeserializeData<ClientMsgHead>(msgData);
        return head;
    }

    #region protobuf 序列化和反序列化
    private byte[] SerializeData<T>(T t)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize<T>(ms, t);
                byte[] b = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(b, 0, b.Length);
                return b;
            }
        }
        catch (Exception e)
        {
            SQDebug.Log("序列化失败  " + e.ToString());
            return null;
        }
    }


    private T DeserializeData<T>(byte[] data)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                //将流的位置归0
                ms.Position = 0;

                //使用工具反序列化对象
                T result = ProtoBuf.Serializer.Deserialize<T>(ms);

                return result;
            }
        }
        catch (Exception e)
        {
            SQDebug.Log("反序列化失败  " + e.ToString());
            return default(T);
        }
    }
    #endregion












    public static byte[] Object2Bytes(object obj)
    {
        byte[] buff = new byte[Marshal.SizeOf(obj)];
        IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
        Marshal.StructureToPtr(obj, ptr, true);
        return buff;
    }



    public void WriteLua(byte[] data, string cmdNum)
    {
        short msgAllLen = (short)(data.Length + 6);
        msgData = new byte[msgAllLen];
        short WriteLen = (short)(data.Length + 4);
        //这里采用的是大端的存储方式 长度是数据的长度 不包括字节本身
        byte[] len = new byte[2];
        len[0] = (byte)(WriteLen >> 8);
        len[1] = (byte)(WriteLen);
        //end
        byte[] cmd = Encoding.ASCII.GetBytes(cmdNum.ToString());
        Array.Copy(len, 0, msgData, 0, 2);
        Array.Copy(cmd, 0, msgData, 2, 4);
        Array.Copy(data, 0, msgData, 6, data.Length);
    }





    public class NullClass
    { }


    #region Serial Data for Dictionary (与上面比较2组效率相差100毫秒左右）


    public Dictionary<string, object> ReadForDictionary(bool showLog = false)
    {
        string jsonString = Encoding.UTF8.GetString(msgData, 0, msgData.Length);
        return deserializeToDictionary(jsonString);
    }

    private static Dictionary<string, object> deserializeToDictionary(string jo)
    {
        SQDebug.Log("Dictionary==>" + jo);
        Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jo);
        Dictionary<string, object> values2 = new Dictionary<string, object>();
        foreach (KeyValuePair<string, object> d in values)
        {
            if (d.Value.GetType().FullName.Contains("Newtonsoft.Json.Linq.JObject"))
            {
                values2.Add(d.Key, deserializeToDictionary(d.Value.ToString()));
            }
            else
            {
                values2.Add(d.Key, d.Value);
            }


        }
        return values2;
    }

    #endregion

}
