using System;

namespace Ens.Request
{
    namespace Client
    {
        public class ExitRoom : RequestClient
        {
            internal override string Header => "R4";

            public static Action OnRecvReply;
            public static Action OnTimeOut;
            public static Action NotInRoomException;

            private static ExitRoom Instance;
            internal ExitRoom() : base()
            {
                Instance = this;
            }
            public static void SendRequest()//提供静态方法用于调用
            {
                Instance.SendRequest("empty");
            }
            protected override void Error(int code, string data)
            {
                NotInRoomException?.Invoke();
            }
            protected override void HandleReply(string data)
            {
                EnsInstance.PresentRoomId = 0;
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
        internal class ExitRoom : RequestServer
        {
            internal override string Header => "R4";
            internal override string HandleRequest(EnsConnection conn, string data)
            {
                var b=EnsRoomManager.Instance.ExitRoom(conn, out int code);
                if(b)return "empty";
                return ThrowError(code);
            }
        }
    }
}