using Ens.Request.Client;
using System;
using System.Collections.Generic;

public class EnsInstance
{
    public static EnsCorrespondent Corr;
    public static EnsSpawner NOMSpawner;


    public static bool MessyLog;
    public static bool DevelopmentDebug;
    public static bool ShowGeneralEvent;

    public static float DisconnectThreshold = 3f;// 上次接收心跳检测时间超过此阈值会认为断开了连接
    public static float HeartbeatMsgInterval = 0.2f;// 发送心跳检测消息的间隔

    public static float KeyExistTime = 3f;//关键信息发送生效时长
    public static float KeySendInterval = 0.05f;//未确认的关键信息发送间隔
    public static float RKeyExistTime = 5f;//对方对关键信息的忽略时长

    public static Action OnConnectionRejected;
    public static Action<int> OnServerConnect;
    public static Action OnServerDisconnect;
    public static Action<int> OnClientEnter;//有新用户连接到服务器时触发(新用户自身不调用)
    public static Action<int> OnClientExit;//有用户与服务器断开时调用(断开的用户自身不调用)
    public static Action<bool> OnAuthorityChanged;

    public static Action OnCreateRoom => CreateRoom.OnCreateRoom;


    //确保断开连接事件只会触发一次(避免多次调用ShutDown)
    //用户不需要注意此参数
    internal static bool RoomExitInvoke = true;
    internal static bool ServerDisconnectInvoke = true;
    internal static bool ClientConnectRejected = false;


    internal static Action<string, EnsConnection> ServerRecvData;
    internal static Action<string> ClientRecvData;

    public static int LocalClientId = -1;
    public static int PresentRoomId = 0;
    public static bool HasAuthority = false;
}
