using System;
using System.Collections.Generic;
using Lidgren.Network;
using LmpCommon.Enums;
using LmpCommon.Message.Client.Base;
using LmpCommon.Message.Data.Quicksave;
using LmpCommon.Message.Types;

namespace LmpCommon.Message.Client
{
    public class QuicksaveCliMsg : CliMsgBase<QuicksaveBaseMsgData>
    {
        internal QuicksaveCliMsg() { }

        public override string ClassName => nameof(QuicksaveCliMsg);

        protected override Dictionary<ushort, Type> SubTypeDictionary { get; } = new Dictionary<ushort, Type>
        {
            [(ushort)QuicksaveMessageType.ListRequest] = typeof(QuicksaveListRequestMsgData),
            [(ushort)QuicksaveMessageType.LoadRequest] = typeof(QuicksaveLoadRequestMsgData),
            [(ushort)QuicksaveMessageType.SaveRequest] = typeof(QuicksaveSaveRequestMsgData),
            [(ushort)QuicksaveMessageType.RemoveRequest] = typeof(QuicksaveRemoveRequestMsgData),
        };

        public override ClientMessageType MessageType => ClientMessageType.Quicksave;
        protected override int DefaultChannel => 21;
        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;
    }
}