#if !UNITY_2017_1_OR_NEWER
using ProtocolWrapper;
using System.Net;

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
#endif