using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class SQGPSLoader : MonoBehaviour {

    //经度
    private float mGpsN = 0f;
    //续度
    private float mGpnE = 0f;
    //是否获取完毕
    private bool isFinished = false;

    public delegate void GetGpsInformationEvent(GPSType type, float n, float e, double time);
    public GetGpsInformationEvent OnGetGpsInfoCall;
	void Start () {

       // SendGPSInfo(null);


    }
    #region 获取经纬度 无地址
    public void StartGpsInfo()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            return;
        }
#if GPS
        StartCoroutine(StartGPS());
#endif
    }

    IEnumerator StartGPS()
    {
        // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置  
        // LocationService.isEnabledByUser 用户设置里的定位服务是否启用  
        if (!Input.location.isEnabledByUser)
        {
            SQDebug.Log( "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS");
            if (OnGetGpsInfoCall != null)
            {
                OnGetGpsInfoCall(GPSType.Error, mGpsN, mGpsN, Input.location.lastData.timestamp);
            }
            yield return false;
        }

        // LocationService.Start() 启动位置服务的更新,最后一个位置坐标会被使用  
        Input.location.Start(10.0f, 10.0f);

       // SQDebug.PrintToScreen("Input.location.status=" + Input.location.status);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
           // SQDebug.PrintToScreen("status=" + Input.location.status);
            // 暂停协同程序的执行(1秒)  
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            SQDebug.Log("Init GPS service time out");
            yield return false;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            SQDebug.Log("Unable to determine device location");
            yield return false;
        }
        else
        {
            //SQDebug.PrintToScreen("N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude);
           
            mGpsN = Input.location.lastData.latitude;
            mGpnE = Input.location.lastData.longitude;

            if(OnGetGpsInfoCall != null)
            {
                OnGetGpsInfoCall(GPSType.Normal, mGpsN, mGpnE, Input.location.lastData.timestamp);
            }
            //this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp;

            //UpLoadingAddress req = new UpLoadingAddress();
            //req.latitude = mGpsN;
            //req.longitude = mGpnE;
            //req.address = "";
            //SQDebug.Log("latitude" + mGpsN+ "longitude:" + mGpnE);
           
            //MainPlayerModel.Inst.address = req;

            //NetProcess.SendRequest<UpLoadingAddress>(req, ProtoIdMap.CMD_SendUpdateAddres, null, false);
            yield return new WaitForSeconds(100);
        }
    }
    #endregion

    #region GPS定位

    private string url = "http://api.map.baidu.com/location/ip?ak=GwyB6ctag3dEl9sXiUZoBizuSM3tcFZF&coor=bd09ll";
    private string phoneurl = "";
    private string phoneurl1 = "http://api.map.baidu.com/geocoder?location=";
    private string phoneurl2 = "&output=json&key=GwyB6ctag3dEl9sXiUZoBizuSM3tcFZF";
    private string GetGps = ""; //GPS定位的位置
    private Coroutine gpsCoroutine;
    /// <summary>
    /// 向服务器发送gps定位信息
    /// </summary>
    public  void SendGPSInfo(CallBack call)
    {
        
        //if (Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    StartCoroutine(GPSPosRequest(call));
        //}
        //else
        //{
        //    StartCoroutine(StartGPS(call));
        //}

    }
    /// <summary>
    /// 发送位置
    /// </summary>
    /// <param name="req"></param>
    //private void SendAddress(UpLoadingAddress req, CallBack call)
    //{
    //    SQDebug.Log("latitude" + mGpsN + "longitude:" + mGpnE);
    //    MainPlayerModel.Inst.address = req;
    //    NetProcess.SendRequest<UpLoadingAddress>(req, ProtoIdMap.CMD_SendUpdateAddres, (msgData) =>
    //    {



    //        if (call != null)
    //        {
    //            call();
    //        }

    //    }, false);

    //}

    IEnumerator GPSPosRequest(CallBack call)
    {
        WWW www = new WWW(url);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {

            ResponseBody req = JsonConvert.DeserializeObject<ResponseBody>(www.text);
            string temp = req.content.address_detail.city;
           
            Debug.Log("x" + req.content.point.x + "y" + req.content.point.y + "城市:" + temp);
            //if (temp.Contains("市"))
            //{
            //    temp = temp.Replace("市", "").Trim();
            //}
            //UpLoadingAddress data = new UpLoadingAddress();
            //data.longitude = float.Parse(req.content.point.x);
            //data.latitude = float.Parse(req.content.point.y);
            //data.address = temp;
            
            //SendAddress(data, call);



        }
    }
    IEnumerator StartGPS(CallBack call)
    {
        // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置  
        // LocationService.isEnabledByUser 用户设置里的定位服务是否启用  
        if (!Input.location.isEnabledByUser)
        {
            GetGps = "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS";
            yield return false;
        }

        // LocationService.Start() 启动位置服务的更新,最后一个位置坐标会被使用  
        Input.location.Start(10.0f, 10.0f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            // 暂停协同程序的执行(1秒)  
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            GetGps = "Init GPS service time out";
            yield return false;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GetGps = "Unable to determine device location";
            yield return false;
        }
        else
        {
            GetGps = Input.location.lastData.latitude + "," + Input.location.lastData.longitude;
            //GetGps = GetGps + " Time:" + Input.location.lastData.timestamp;
            //yield return new WaitForSeconds(100);
            //GetGps = "30.67994285,104.06792346";
            phoneurl = phoneurl1 + GetGps + phoneurl2;
            Input.location.Stop();
            StartCoroutine(PhoneGPS(call));
            yield break;
        }
    }
    IEnumerator PhoneGPS(CallBack call)
    {
        WWW www = new WWW(phoneurl);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {

            PhonePos req = JsonConvert.DeserializeObject<PhonePos>(www.text);


            Debug.Log("手机gps:" + req.result.addressComponent.city);
            //UpLoadingAddress data = new UpLoadingAddress();
            //data.longitude = req.result.location.lng;
            //data.latitude = req.result.location.lat;
            //data.address = req.result.addressComponent.city;

            //SendAddress(data, call);

        }
    }

    #endregion
}

public enum GPSType
{
    Normal = 1,
    Error =2
}

#region  GPS 类
public class PhonePos
{
    public string status;
    public Result result;
}

public class Result
{
    public Locations location;
    public string formatted_address;
    public string business;
    public AddressComponent addressComponent;
    public int cityCode;
}
public class Locations
{
    public float lng;
    public float lat;
    public Locations(float _lng, float _lat)
    {
        this.lng = _lng;
        this.lat = _lat;
    }
}
public class AddressComponent
{
    public string city;
    public string direction;
    public string distance;
    public string district;
    public string province;
    public string street;
    public string street_number;
    public AddressComponent(string _city, string _direction, string _distance, string _district,
        string _province, string _street, string _street_number)
    {
        city = _city;
        direction = _direction;
        distance = _distance;
        district = _district;
        province = _province;
        street = _street;
        street_number = _street_number;
    }
}

public class ResponseBody
{

    public string address;
    public Content content;
    public int status;

}

public class Content
{
    public string address;
    public Address_Detail address_detail;
    public Point point;
}
public class Address_Detail
{
    public string city;
    public int city_code;
    public string district;
    public string province;
    public string street;
    public string street_number;
    public Address_Detail(string city, int city_code, string district, string province, string street, string street_number)
    {
        this.city = city;
        this.city_code = city_code;
        this.district = district;
        this.province = province;
        this.street = street;
        this.street_number = street_number;
    }
}
public class Point
{
    public string x;
    public string y;
    public Point(string x, string y)
    {
        this.x = x;
        this.y = y;
    }
}
#endregion