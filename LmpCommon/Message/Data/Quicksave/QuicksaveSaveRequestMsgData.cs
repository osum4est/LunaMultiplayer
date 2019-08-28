using System;
using Lidgren.Network;
using LmpCommon.Message.Base;
using LmpCommon.Message.Types;

namespace LmpCommon.Message.Data.Quicksave
{
    public class QuicksaveSaveRequestMsgData : QuicksaveBaseMsgData
    {
        internal QuicksaveSaveRequestMsgData() { }

        public string Name;
        public Guid VesselId;

        public override QuicksaveMessageType MessageType => QuicksaveMessageType.SaveRequest;

        public override string ClassName { get; } = nameof(QuicksaveSaveRequestMsgData);

        internal override void InternalSerialize(NetOutgoingMessage lidgrenMsg)
        {
            base.InternalSerialize(lidgrenMsg);
            lidgrenMsg.Write(Name);
            GuidUtil.Serialize(VesselId, lidgrenMsg);
        }

        internal override void InternalDeserialize(NetIncomingMessage lidgrenMsg)
        {
            base.InternalDeserialize(lidgrenMsg);
            Name = lidgrenMsg.ReadString();
            VesselId = GuidUtil.Deserialize(lidgrenMsg);
        }

        internal override int InternalGetMessageSize()
        {
            return base.InternalGetMessageSize() + Name.GetByteCount() + GuidUtil.ByteSize;
        }
    }
}