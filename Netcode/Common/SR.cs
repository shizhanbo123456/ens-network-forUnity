using Utils;

internal abstract class SR : Disposable//具有信息收发功能
{
    internal ReachTime hbRecvTime = new ReachTime(EnsInstance.DisconnectThreshold, ReachTime.InitTimeFlagType.ReachAfter);
    internal ReachTime hbSendTime = new ReachTime(EnsInstance.HeartbeatMsgInterval, ReachTime.InitTimeFlagType.ReachAfter);

    internal abstract bool On();

    internal abstract void SendData(string data);
    //CircularQueue ReceiveData();
    internal abstract void Update();
    internal abstract void ShutDown();

    protected override void ReleaseUnmanagedMenory()
    {
        hbRecvTime = null;
        hbSendTime = null;
    }
}