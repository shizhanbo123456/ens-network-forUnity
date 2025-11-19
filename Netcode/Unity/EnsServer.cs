using ProtocolWrapper;
using System.Collections.Generic;
using System.Net;

public class EnsServer : Disposable
{
    internal static EnsServer Instance;

    internal Dictionary<int, EnsConnection> ClientConnections = new Dictionary<int, EnsConnection>();
    internal EnsRoomManager RoomManager;
    internal ListenerBase Listener;

    public bool On;

    public EnsServer(IPAddress ip, int port)
    {
        Instance = this;

        Protocol.OnRecvConnection = (conn, index) => OnRecvConnection(conn, index);

        Listener = Protocol.GetListener(ip, port);
        RoomManager = new EnsRoomManager();
        On = true;
    }
    internal virtual void OnRecvConnection(ProtocolBase conn, int index)
    {
        EnsConnection connection = new EnsConnection(conn, index);
        connection.OnShutDown += OnConnectionShutDown;
        ClientConnections.Add(index, connection);
    }
    internal void OnConnectionShutDown(EnsConnection conn)
    {
        if (!On) return;//房间管理器关闭时不会发送退出房间
        if (conn.room != null) conn.room.Exit(conn);
        ClientConnections.Remove(conn.ClientId);
    }
    public void StartListening()
    {
        if (!Listener.Listening) Listener.StartListening();
    }
    public void EndListening()
    {
        if (Listener.Listening) Listener.EndListening();
    }
    public virtual void Update()
    {
        for (int index = ClientConnections.Count - 1; index >= 0; index--)
        {
            var i = ClientConnections[index];
            if (i.hbRecvTime.Reached)
            {
                i.ShutDown();
                i.Dispose();
                ClientConnections.Remove(i.ClientId);
                continue;
            }
            if (i.hbSendTime.Reached)
            {
                i.SendData(Header.H + ((int)(Utils.Time.time * 1000)).ToString());
                i.hbSendTime.ReachAfter(EnsInstance.HeartbeatMsgInterval);
            }
            i.Update();
        }
    }
    public virtual void ShutDown()
    {
        if (!On) return;
        On = false;
        EndListening();
        foreach (var i in ClientConnections.Values) i.ShutDown();
        Listener.ShutDown();
        RoomManager.ShutDown();
    }
    protected override void ReleaseManagedMenory()
    {
        foreach (var i in ClientConnections.Values) i.Dispose();
        ClientConnections.Clear();
        Listener.Dispose();
        RoomManager.Dispose();
        base.ReleaseManagedMenory();
    }
    protected override void ReleaseUnmanagedMenory()
    {
        ClientConnections = null;
        Listener = null;
        RoomManager = null;
        Instance = null;
        base.ReleaseUnmanagedMenory();
    }
}
