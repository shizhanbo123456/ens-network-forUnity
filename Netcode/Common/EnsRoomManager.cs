using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
internal class EnsRoomManager:Disposable
{
    internal static EnsRoomManager Instance;
    internal SortedDictionary<int,EnsRoom> rooms = new SortedDictionary<int, EnsRoom>();
    private int RoomId = 10000;


    internal EnsRoomManager()
    {
        Instance = this;
    }

    internal bool CreateRoom(EnsConnection conn,out int code)
    {
        if (conn.room != null)
        {
            code = 0;
            return false;
        }
        rooms.Add(RoomId,new EnsRoom(RoomId));
        rooms[RoomId].Join(conn);
        RoomId += 1;
        if (EnsProgram.Instance.RoomDebug) Debug.Log("创建了房间 " + (RoomId - 1).ToString());
        code= conn.room.RoomId;
        return true;
    }
    internal bool JoinRoom(EnsConnection conn, int id,out int code)
    {
        if (conn.room != null)
        {
            code = 1;
            return false;
        }
        if (!rooms.ContainsKey(id))
        {
            code = 0;
            return false;
        }
        var room = rooms[id];

        room.Join(conn);
        code = room.RoomId;
        return true;
    }
    internal bool ExitRoom(EnsConnection conn,out int id)
    {
        if (conn.room == null)
        {
            id= 0;
            return false;
        }
        if (EnsProgram.Instance.RoomDebug) Debug.Log("离开了房间 " + conn.room.RoomId);
        conn.room.Exit(conn);
        id = 0;
        return true;
    }







    protected override void ReleaseManagedMenory()
    {
        foreach (var r in rooms.Values) r.Dispose();
        rooms.Clear();
        base.ReleaseManagedMenory();
    }
    protected override void ReleaseUnmanagedMenory()
    {
        Instance = null;
        rooms = null;
        base.ReleaseUnmanagedMenory();
    }
    public override string ToString()
    {
        string r = "房间信息：";
        foreach (var i in rooms)
        {
            r += i.ToString() + " ";
        }
        return r;
    }
}
