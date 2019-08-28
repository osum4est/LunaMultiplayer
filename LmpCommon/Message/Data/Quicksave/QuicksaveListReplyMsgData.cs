using System.Linq;
using Lidgren.Network;
using LmpCommon.Message.Types;

namespace LmpCommon.Message.Data.Quicksave
{
    public class QuicksaveListReplyMsgData : QuicksaveBaseMsgData
    {
        internal QuicksaveListReplyMsgData() { }

        public int QuicksavesCount;
        public QuicksaveBasicInfo[] Quicksaves;

        public override QuicksaveMessageType MessageType => QuicksaveMessageType.ListReply;

        public override string ClassName { get; } = nameof(QuicksaveListReplyMsgData);

        internal override void InternalSerialize(NetOutgoingMessage lidgrenMsg)
        {
            base.InternalSerialize(lidgrenMsg);

            lidgrenMsg.Write(QuicksavesCount);
            for (var i = 0; i < QuicksavesCount; i++)
                Quicksaves[i].Serialize(lidgrenMsg);
        }

        internal override void InternalDeserialize(NetIncomingMessage lidgrenMsg)
        {
            base.InternalDeserialize(lidgrenMsg);

            QuicksavesCount = lidgrenMsg.ReadInt32();
            Quicksaves = new QuicksaveBasicInfo[QuicksavesCount];
            for (var i = 0; i < QuicksavesCount; i++)
            {
                Quicksaves[i] = new QuicksaveBasicInfo();
                Quicksaves[i].Deserialize(lidgrenMsg);
            }
        }

        internal override int InternalGetMessageSize()
        {
            return base.InternalGetMessageSize() + sizeof(int) + Quicksaves.Sum(q => q.GetByteCount());
        }
    }
}