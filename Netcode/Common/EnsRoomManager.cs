using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnsRoomManager:Disposable
{
    public static EnsRoomManager Instance;
    public Dictionary<int,EnsRoom> rooms = new Dictionary<int, EnsRoom>();
    private int RoomId = 10000;


    public EnsRoomManager()
    {
        Instance = this;
    }

    public bool CreateRoom(EnsConnection conn,out int code)
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
    public bool JoinRoom(EnsConnection conn, int id,List<string> info,out int code)
    {
        if (conn.room != null)
        {
            code = 2;
            return false;
        }
        if (!rooms.ContainsKey(id))
        {
            code = 0;
            return false;
        }
        var room = rooms[id];

        Dictionary<string, string> inputInfo = new Dictionary<string, string>();
        foreach (var infoStr in info)
        {
            string[] parts = infoStr.Split(':', 2); // 按第一个冒号分割
            inputInfo[parts[0]] = parts[1];
        }
        foreach(var i in room.Rule.Keys)
        {
            if (!inputInfo.ContainsKey(i))
            {
                code = 1;
                return false;
            }
        }
        List<string>unmatched= new List<string>();
        // 校验所有规则是否满足
        foreach (var rule in room.Rule)
        {
            string key = rule.Key;

            var inputNum = float.Parse(inputInfo[key]);
            var targetNum = float.Parse(rule.Value.Item2);
            switch (rule.Value.Item1)
            {
                case '>':if (inputNum <= targetNum) unmatched.Add(key);break;
                case '<':if (inputNum >= targetNum) unmatched.Add(key);break;
                case '=':if (inputNum != targetNum) unmatched.Add(key);break;
                case '!':if (inputNum == targetNum) unmatched.Add(key);break;
            }
        }

        room.Join(conn);
        code = room.RoomId;
        return true;
    }
    public bool ExitRoom(EnsConnection conn,out int id)
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
    public string PrintData()
    {
        string r = "房间信息：";
        foreach (var i in rooms)
        {
            r += i.ToString() + " ";
        }
        return r;
    }
}
