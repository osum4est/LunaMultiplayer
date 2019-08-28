using System;
using LmpClient.Base;
using LmpClient.Base.Interface;
using LmpClient.Network;
using LmpCommon.Message.Client;
using LmpCommon.Message.Data.Quicksave;
using LmpCommon.Message.Interface;

namespace LmpClient.Systems.Quicksave
{
    public class QuicksaveMessageSender : SubSystem<QuicksaveSystem>, IMessageSender
    {
        private Guid VesselId => FlightGlobals.ActiveVessel.id;

        public void SendMessage(IMessageData msg)
        {
            TaskFactory.StartNew(() =>
                NetworkSender.QueueOutgoingMessage(MessageFactory.CreateNew<QuicksaveCliMsg>(msg)));
        }

        public void SendQuicksaveListRequestMsg()
        {
            var msgData = NetworkMain.CliMsgFactory.CreateNewMessageData<QuicksaveListRequestMsgData>();
            msgData.VesselId = VesselId;
            SendMessage(msgData);
        }

        public void SendQuicksaveLoadRequestMsg(QuicksaveBasicInfo quicksave)
        {
            var msgData = NetworkMain.CliMsgFactory.CreateNewMessageData<QuicksaveLoadRequestMsgData>();
            msgData.QuicksaveInfo = quicksave;
            SendMessage(msgData);
        }

        public void SendQuicksaveSaveRequestMsg(string name)
        {
            var msgData = NetworkMain.CliMsgFactory.CreateNewMessageData<QuicksaveSaveRequestMsgData>();
            msgData.Name = name;
            msgData.VesselId = VesselId;
            SendMessage(msgData);
        }

        public void SendQuicksaveRemoveRequestMsg(QuicksaveBasicInfo quicksave)
        {
            var msgData = NetworkMain.CliMsgFactory.CreateNewMessageData<QuicksaveRemoveRequestMsgData>();
            msgData.QuicksaveInfo = quicksave;
            SendMessage(msgData);
        }
    }
}