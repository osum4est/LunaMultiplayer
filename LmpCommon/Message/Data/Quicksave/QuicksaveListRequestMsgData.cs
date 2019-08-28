using System;
using Lidgren.Network;
using LmpCommon.Message.Base;
using LmpCommon.Message.Types;

namespace LmpCommon.Message.Data.Quicksave
{
    public class QuicksaveListRequestMsgData : QuicksaveBaseMsgData
    {
        internal QuicksaveListRequestMsgData() { }

        public Guid VesselId;

        public override QuicksaveMessageType MessageType => QuicksaveMessageType.ListRequest;

        public override string ClassName { get; } = nameof(QuicksaveListRequestMsgData);

        internal override void InternalSerialize(NetOutgoingMessage lidgrenMsg)
        {
            base.InternalSerialize(lidgrenMsg);
            GuidUtil.Serialize(VesselId, lidgrenMsg);
        }

        internal override void InternalDeserialize(NetIncomingMessage lidgrenMsg)
        {
            base.InternalDeserialize(lidgrenMsg);
            VesselId = GuidUtil.Deserialize(lidgrenMsg);
        }

        internal override int InternalGetMessageSize()
        {
            return base.InternalGetMessageSize() + GuidUtil.ByteSize;
        }
    }
}