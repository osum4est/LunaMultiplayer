namespace LmpCommon.Message.Types
{
    public enum QuicksaveMessageType
    {
        SaveRequest = 0,
        RemoveRequest = 1,
        LoadRequest = 2,
        LoadReply = 3,
        ListRequest = 4,
        ListReply = 5
    }
}