using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class WebSocketServer
{
    private static ClientWebSocket ws;
    private static bool heartbeat = false;

    public async static void Connect(string url) {
        try
        {
            ws = new ClientWebSocket();

            Uri uri = new Uri(url);
            await ws.ConnectAsync(uri, new System.Threading.CancellationToken());

            heartbeat = true;
            StartHeart();


            while (true) {
                var result = new byte[1024 * 10];
                await ws.ReceiveAsync(new ArraySegment<byte>(result), new System.Threading.CancellationToken());
                //去除多余的数据
                var lastbyte = ByteCut(result, 0x00);

                var str = Encoding.UTF8.GetString(lastbyte, 0, lastbyte.Length);

                //判断心跳
                if (str[0] == ' ')
                {

                }
                else { 
                    
                }
            }
        }
        catch (Exception e) { 
            
        }
    }

    private static byte[] ByteCut(byte[] b, byte cut) {
        var list = new List<byte>();
        list.AddRange(b);
        for (var i = list.Count - 1; i >= 0; i--) {
            if (list[i] == cut) list.RemoveAt(i);
        }

        var lastbyte = new byte[list.Count];
        for (var i = 0; i < list.Count; i++) {
            lastbyte[i] = list[i];
        }

        return lastbyte;
    }

    private async static void StartHeart() {
        while (heartbeat) {
            await Task.Delay(30 * 1000);


        }
    }
}
