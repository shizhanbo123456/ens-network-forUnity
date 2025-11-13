using System;

namespace Ens.Request
{
    namespace Client
    {
        public class CreateRoom:RequestClient
        {
            internal override string Header => "R0";

            public static Action OnCreateRoom;
            public static Action OnTimeOut;
            public static Action AlreadyInRoomException;

            private static CreateRoom Instance;
            internal CreateRoom():base()
            {
                Instance = this;
            }
            public static void SendRequest()//提供静态方法用于调用
            {
                Instance.SendRequest("empty");
            }
            protected override void Error(int code, string data)
            {
                AlreadyInRoomException?.Invoke();
            }
            protected override void HandleReply(string data)
            {
                EnsInstance.PresentRoomId = int.Parse(data);
                OnCreateRoom?.Invoke();
            }
            internal override void TimeOut()
            {
                OnTimeOut?.Invoke();
            }
        }
    }
    namespace Server
    {
        internal class CreateRoom:RequestServer
        {
            internal override string Header => "R0";
            internal override string HandleRequest(EnsConnection conn, string data)
            {
                var b = EnsRoomManager.Instance.CreateRoom(conn, out int code);
                if (b) return code.ToString();
                else return ThrowError(code);
            }
        }
    }
}