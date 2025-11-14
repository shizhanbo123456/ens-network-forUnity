using System;
using System.Collections.Generic;

namespace Ens.Request
{
    namespace Client
    {
        public class GetRuleList : RequestClient
        {
            protected override string Header => "RE1";

            public static Action OnRecvReply;
            public static Action OnTimeOut;
            public static Action<int> OnError;

            private static GetRuleList Instance;
            public GetRuleList() : base()
            {
                Instance = this;
                EnsClientRequest.RegistRequest(this);
            }
            public static void SendRequest(List<int>rooms)//提供静态方法用于调用
            {
                Instance.SendRequest(Format.ListToString(rooms));
            }
            protected override void Error(int code,string data)
            {
                OnError?.Invoke(code);
            }
            protected override void HandleReply(string data)
            {
                OnRecvReply?.Invoke();
            }
            protected override void TimeOut()
            {
                OnTimeOut?.Invoke();
            }
        }
    }
    namespace Server
    {
        public class GetRuleList : RequestServer
        {
            protected override string Header => "RE1";
            protected override string HandleRequest(EnsConnection conn, string data)
            {
                var s = Format.StringToList(data, int.Parse);
                return "empty";
            }
        }
    }
}