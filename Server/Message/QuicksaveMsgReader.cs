using System;
using LmpCommon.Message.Data.Quicksave;
using LmpCommon.Message.Interface;
using LmpCommon.Message.Types;
using Server.Client;
using Server.Message.Base;
using Server.System;

namespace Server.Message
{
    public class QuicksaveMsgReader : ReaderBase
    {
        public override void HandleMessage(ClientStructure client, IClientMessageBase message)
        {
            var data = (QuicksaveBaseMsgData)message.Data;

            switch (data.MessageType)
            {
                case QuicksaveMessageType.ListRequest:
                    QuicksaveSystem.SendQuicksaveList(client, (QuicksaveListRequestMsgData)data);
                    break;
                case QuicksaveMessageType.LoadRequest:
                    QuicksaveSystem.LoadQuicksave(client, (QuicksaveLoadRequestMsgData)data);
                    break;
                case QuicksaveMessageType.SaveRequest:
                    QuicksaveSystem.SaveQuicksave(client, (QuicksaveSaveRequestMsgData)data);
                    break;
                case QuicksaveMessageType.RemoveRequest:
                    QuicksaveSystem.RemoveQuicksave(client, (QuicksaveRemoveRequestMsgData)data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}