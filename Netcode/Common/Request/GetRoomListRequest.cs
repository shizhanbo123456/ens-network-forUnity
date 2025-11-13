using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ens.Request
{
    namespace Client
    {
        public class GetRoomList : RequestClient
        {
            internal override string Header => "R5";

            public static Action<int,List<int>> OnRecvReply;
            public static Action OnTimeOut;

            private static GetRoomList Instance;
            internal GetRoomList() : base()
            {
                Instance = this;
            }
            public static void SendRequest(int startIndex,int endIndex)//提供静态方法用于调用
            {
                Instance.SendRequest(startIndex+"-"+endIndex);
            }
            protected override void Error(int code, string data)
            {
                
            }
            protected override void HandleReply(string data)
            {
                var List = Format.StringToList(data, int.Parse);
                var count = List[0];
                List.RemoveAt(0);
                OnRecvReply?.Invoke(count,List);
            }
            internal override void TimeOut()
            {
                OnTimeOut?.Invoke();
            }
        }
    }
    namespace Server
    {
        internal class GetRoomList : RequestServer
        {
            internal override string Header => "R5";
            internal override string HandleRequest(EnsConnection conn, string data)
            {
                List<int>r=new List<int>();
                var rooms = EnsRoomManager.Instance.rooms.Keys.ToList();
                r.Add(rooms.Count);
                var s=data.Split('-');
                int start = int.Parse(s[0]);
                start=Math.Max(start,0);
                int end=int.Parse(s[1]);
                end=Math.Min(end,rooms.Count-1);
                for (int i = start; i <= end; i++) r.Add(rooms[i]);
                return Format.ListToString(r);
            }
        }
    }
}