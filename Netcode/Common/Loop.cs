using ProtocolWrapper;
using Utils;

public class Loop
{
    public static void InitCommon()
    {
        Time.Init();
    }
    public static void InitClient()
    {

    }
    public static void InitServer()
    {

    }
    public static void LoopCommon()
    {
        Time.Update();
        FrameCounter.Update();
    }
    public static void LoopClient()
    {
        Broadcast.Update();
        EnsClientRequest.Update();
    }
    public static void LoopServer()
    {

    }
}