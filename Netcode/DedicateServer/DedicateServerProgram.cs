#if !UNITY_2017_1_OR_NEWER
using System.Net;

public class DedicateServerProgram
{
    public EnsDedicatedServer server;

    public float DisconnectThreshold = 3f;
    public float HeartbeatMsgInterval = 0.2f;
    public ProtocolWrapper.ProtocolType ProtocolType = ProtocolWrapper.ProtocolType.TCP;
    public bool PrintRoomData = true;

    //为空则视为IPAddress.Any
    public string IP;
    public int port = 12345;

    public void Start()
    {
        EnsInstance.DisconnectThreshold = DisconnectThreshold;
        EnsInstance.HeartbeatMsgInterval = HeartbeatMsgInterval;
        ProtocolWrapper.Protocol.type = ProtocolType;
        IPAddress ip = IP == string.Empty ? IPAddress.Any : IPAddress.Parse(IP);
        EnsRoomManager.PrintRoomData = PrintRoomData;

        EnsEventRegister.InitServer();
        EnsEventRegister.InitCommon();

        server=new EnsDedicatedServer(ip,port);
        server.StartListening();
    }
    public void Loop()
    {
        EnsEventRegister.LoopCommon();
        EnsEventRegister.LoopServer();

        server.Update();
    }
}
#endif