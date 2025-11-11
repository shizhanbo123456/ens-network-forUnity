using Utils;

public static class RequestRoomClient
{
    public static void RegistEvents()
    {
        EnsClientRequst.RegistRequest("R0", R0CallBack,()=>EnsInstance.OnCreateRoomTimeOut?.Invoke());
        EnsClientRequst.RegistRequest("R1", R1CallBack,()=>EnsInstance.OnSetRoomRuleTimeOut?.Invoke());
        EnsClientRequst.RegistRequest("R2", R2CallBack,()=>EnsInstance.OnJoinRoomTimeOut?.Invoke());
        EnsClientRequst.RegistRequest("R3", R3CallBack,()=>EnsInstance.OnGetRoomRuleTimeOut?.Invoke());
        EnsClientRequst.RegistRequest("R4", R4CallBack,()=>EnsInstance.OnExitRoomTimeOut?.Invoke());
    }
    private static bool IfError(string data,out int id)
    {
        if (data.StartsWith("error"))
        {
            id = int.Parse(data.Substring(5, data.Length - 5));
            return true;
        }
        else
        {
            id = -1;
            return false;
        }
    }
    public static void R0CallBack(string content)//创建
    {
        if(IfError(content,out int id))
        {
            Debug.LogError("已经在房间里");
            return;
        }
        EnsInstance.PresentRoomId=int.Parse(content);
        EnsInstance.OnJoinRoom?.Invoke();
    }
    public static void R1CallBack(string content)//设置规则
    {
        if (IfError(content, out int id))
        {
            Debug.LogError("未加入房间");
            return;
        }
        EnsInstance.OnSetRomeRuleSucceed?.Invoke();
    }
    public static void R2CallBack(string content)//加入
    {
        if (IfError(content, out int id))
        {
            switch (id)
            {
                case 0:EnsInstance.OnRoomNotFound?.Invoke(); return;//不存在
                case 1:EnsInstance.OnJoinFailedByInfoLack?.Invoke();return;//信息不足
                case 2: Debug.LogError("已经在房间里");return;
            }
            return;
        }
        EnsInstance.PresentRoomId = int.Parse(content);
        EnsInstance.OnJoinRoom?.Invoke();
    }
    public static void R3CallBack(string content)//获取规则
    {
        if (IfError(content, out int id))
        {
            EnsInstance.OnRoomNotFound?.Invoke();
            return;
        }
        var s=Format.SplitWithBoundaries(content, '#');
        EnsInstance.OnGetRoomRule?.Invoke(s);
    }
    public static void R4CallBack(string content)//离开
    {
        if (IfError(content, out int id))
        {
            Debug.LogError("不在房间里");
            return;
        }
        EnsInstance.PresentRoomId = 0;
        EnsInstance.OnExitRoom?.Invoke();
    }
}