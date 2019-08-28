using System;
using Lidgren.Network;
using LmpCommon.Message.Base;
using LmpCommon.Message.Types;

namespace LmpCommon.Message.Data.Quicksave
{
    public abstract class QuicksaveBaseMsgData : MessageData
    {
        internal QuicksaveBaseMsgData() { }

        public override ushort SubType => (ushort)(int)MessageType;
        public virtual QuicksaveMessageType MessageType => throw new NotImplementedException();

        internal override void InternalSerialize(NetOutgoingMessage lidgrenMsg)
        {
            //Nothing to implement here
        }

        internal override void InternalDeserialize(NetIncomingMessage lidgrenMsg)
        {
            //Nothing to implement here
        }

        internal override int InternalGetMessageSize()
        {
            return 0;
        }
    }
}