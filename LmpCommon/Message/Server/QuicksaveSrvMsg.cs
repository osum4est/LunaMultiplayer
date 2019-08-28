using System;
using System.Collections.Generic;
using Lidgren.Network;
using LmpCommon.Enums;
using LmpCommon.Message.Data.Quicksave;
using LmpCommon.Message.Server.Base;
using LmpCommon.Message.Types;

namespace LmpCommon.Message.Server
{
    public class QuicksaveSrvMsg : SrvMsgBase<QuicksaveBaseMsgData>
    {
        internal QuicksaveSrvMsg() { }

        public override string ClassName => nameof(QuicksaveSrvMsg);

        protected override Dictionary<ushort, Type> SubTypeDictionary { get; } = new Dictionary<ushort, Type>
        {
            [(ushort)QuicksaveMessageType.LoadReply] = typeof(QuicksaveLoadReplyMsgData),
            [(ushort)QuicksaveMessageType.ListReply] = typeof(QuicksaveListReplyMsgData),
        };

        public override ServerMessageType MessageType => ServerMessageType.Quicksave;
        protected override int DefaultChannel => 21;
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;
    }
}