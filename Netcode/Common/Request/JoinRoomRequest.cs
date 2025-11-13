using System;
using System.Collections.Generic;
using System.Text;

namespace Ens.Request
{
    namespace Client
    {
        public class JoinRoom : RequestClient
        {
            internal override string Header => "R2";

            public static Action OnRecvReply;
            public static Action OnTimeOut;
            public static Action<int> OnRoomNotFoundError;

            private static JoinRoom Instance;
            internal JoinRoom() : base()
            {
                Instance = this;
            }
            public static void SendRequest(int id)//提供静态方法用于调用
            {
                Instance.SendRequest(id.ToString());
            }
            protected override void Error(int code, string data)
            {
                OnRoomNotFoundError?.Invoke(code);
            }
            protected override void HandleReply(string data)
            {
                EnsInstance.PresentRoomId=int.Parse(data);
                OnRecvReply?.Invoke();
            }
            internal override void TimeOut()
            {
                OnTimeOut?.Invoke();
            }
        }
    }
    namespace Server
    {
        internal class JoinRoom : RequestServer
        {
            internal override string Header => "R2";
            internal override string HandleRequest(EnsConnection conn, string data)
            {
                if (conn.room != null) return ThrowError(1);
                int id = int.Parse(data);
                var b=EnsRoomManager.Instance.JoinRoom(conn,id,out var code);
                if(b)return code.ToString();
                else return ThrowError(code);
            }
        }
    }
}