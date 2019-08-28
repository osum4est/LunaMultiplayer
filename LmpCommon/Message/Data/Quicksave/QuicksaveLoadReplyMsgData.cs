using Lidgren.Network;
using LmpCommon.Message.Types;

namespace LmpCommon.Message.Data.Quicksave
{
    public class QuicksaveLoadReplyMsgData : QuicksaveBaseMsgData
    {
        internal QuicksaveLoadReplyMsgData() { }

        public QuicksaveInfo QuicksaveInfo = new QuicksaveInfo();

        public override QuicksaveMessageType MessageType => QuicksaveMessageType.LoadReply;

        public override string ClassName { get; } = nameof(QuicksaveLoadReplyMsgData);

        internal override void InternalSerialize(NetOutgoingMessage lidgrenMsg)
        {
            base.InternalSerialize(lidgrenMsg);
            QuicksaveInfo.Serialize(lidgrenMsg);
        }

        internal override void InternalDeserialize(NetIncomingMessage lidgrenMsg)
        {
            base.InternalDeserialize(lidgrenMsg);
            QuicksaveInfo.Deserialize(lidgrenMsg);
        }

        internal override int InternalGetMessageSize()
        {
            return base.InternalGetMessageSize() + QuicksaveInfo.GetByteCount();
        }
    }
}