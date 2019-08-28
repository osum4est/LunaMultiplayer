using Lidgren.Network;
using LmpCommon.Message.Types;

namespace LmpCommon.Message.Data.Quicksave
{
    public class QuicksaveRemoveRequestMsgData : QuicksaveBaseMsgData
    {
        internal QuicksaveRemoveRequestMsgData() { }

        public QuicksaveBasicInfo QuicksaveInfo = new QuicksaveBasicInfo();

        public override QuicksaveMessageType MessageType => QuicksaveMessageType.RemoveRequest;

        public override string ClassName { get; } = nameof(QuicksaveRemoveRequestMsgData);

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