using System;
using System.Collections.Generic;

namespace Ens.Request
{
    namespace Client
    {
        public class GetRule : RequestClient
        {
            internal override string Header => "R3";

            public static Action<Dictionary<string,(char,int)>> OnRecvReply;
            public static Action OnTimeOut;
            public static Action RoomNotFoundError;

            private static GetRule Instance;
            internal GetRule() : base()
            {
                Instance = this;
            }
            public static void SendRequest(int id)//提供静态方法用于调用
            {
                Instance.SendRequest(id.ToString());
            }
            protected override void Error(int code, string data)
            {
                RoomNotFoundError?.Invoke();
            }
            protected override void HandleReply(string data)
            {
                var d=Format.StringToDictionary(data, s => s, valueconverter: t => (t[0], int.Parse(t.Substring(1, t.Length - 1))));
                OnRecvReply?.Invoke(d);
            }
            internal override void TimeOut()
            {
                OnTimeOut?.Invoke();
            }
        }
    }
    namespace Server
    {
        internal class GetRule : RequestServer
        {
            internal override string Header => "R3";
            internal override string HandleRequest(EnsConnection conn, string data)
            {
                if(EnsRoomManager.Instance.rooms.TryGetValue(int.Parse(data), out var room))
                {
                    return Format.DictionaryToString(room.Rule,valueconverter:t=>t.Item1.ToString()+t.Item2);
                }
                else
                {
                    return ThrowError(0);
                }
            }
        }
    }
}