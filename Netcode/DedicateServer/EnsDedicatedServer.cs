using ProtocolWrapper;
using System.Net;
using UnityEngine;

public class EnsDedicatedServer:ServerBase
{
    public EnsDedicatedServer(IPAddress ip,int port)
    {
        EnsEventRegister.RegistDedicateServer();

        Listener = Protocol.GetListener(ip, port);
        RoomManager = new EnsRoomManager();
        On = true;

        Protocol.OnRecvConnection = (conn, index) => OnRecvConnection(conn, index);
    }



    public override void ShutDown()
    {
        if (!On) return;
        base.ShutDown();
    }
}
