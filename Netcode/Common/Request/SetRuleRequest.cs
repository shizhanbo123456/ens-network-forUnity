using System;

namespace Ens.Request
{
    namespace Client
    {
        public class SetRule : RequestClient
        {
            protected internal override string Header => "R1";

            public static Action OnRecvReply;
            public static Action OnTimeOut;
            public static Action NotInRoomError;

            private static SetRule Instance;
            internal SetRule() : base()
            {
                Instance = this;
            }
            public static void SendRequest(string type,char op,int value)//提供静态方法用于调用
            {
                Instance.SendRequest(type+op+value);
            }
            protected override void Error(int code, string data)
            {
                NotInRoomError?.Invoke();
            }
            protected override void HandleReply(string data)
            {
                OnRecvReply?.Invoke();
            }
            protected internal override void TimeOut()
            {
                OnTimeOut?.Invoke();
            }
        }
    }
    namespace Server
    {
        internal class SetRule : RequestServer
        {
            protected internal override string Header => "R1";
            protected internal override string HandleRequest(EnsConnection conn, string data)
            {
                string[] s=null;
                char c = '1';
                if (conn.room == null) return ThrowError(0);
                if (data.Contains('>')) { s = data.Split('>'); c = '>'; }
                else if (data.Contains('<')) { s = data.Split('<'); c = '<'; }
                else if (data.Contains('=')) { s = data.Split('='); c = '='; }
                else if (data.Contains('!')) { s = data.Split('!'); c = '!'; }
                if (conn.room.Rule.ContainsKey(s[0])) conn.room.Rule[s[0]] = ('>', int.Parse(s[1]));
                else conn.room.Rule.Add(s[0], (c, int.Parse(s[1])));
                return "empty";
            }
        }
    }
}