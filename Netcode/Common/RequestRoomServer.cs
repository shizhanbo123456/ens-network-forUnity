public static class RequestRoomServer
{
    public static void RegistEvents()
    {
        EnsServerRequest.RegistRequestHeader("R0", R0Reply);
        EnsServerRequest.RegistRequestHeader("R1", R1Reply);
        EnsServerRequest.RegistRequestHeader("R2", R2Reply);
        EnsServerRequest.RegistRequestHeader("R3", R3Reply);
        EnsServerRequest.RegistRequestHeader("R4", R4Reply);
    }
    private static string R0Reply(EnsConnection conn,string content)
    {
        if(EnsRoomManager.Instance.CreateRoom(conn,out int code))
        {
            return code.ToString();
        }
        else
        {
            return "error" + code;
        }
    }
    private static string R1Reply(EnsConnection conn, string content)
    {
        if (EnsRoomManager.Instance.CreateRoom(conn, out int code))
        {
            return code.ToString();
        }
        else
        {
            return "error" + code;
        }
    }
    private static string R2Reply(EnsConnection conn, string content)
    {
        if (EnsRoomManager.Instance.CreateRoom(conn, out int code))
        {
            return code.ToString();
        }
        else
        {
            return "error" + code;
        }
    }
    private static string R3Reply(EnsConnection conn, string content)
    {
        if (EnsRoomManager.Instance.CreateRoom(conn, out int code))
        {
            return code.ToString();
        }
        else
        {
            return "error" + code;
        }
    }
    private static string R4Reply(EnsConnection conn, string content)
    {
        if (EnsRoomManager.Instance.CreateRoom(conn, out int code))
        {
            return code.ToString();
        }
        else
        {
            return "error" + code;
        }
    }
}