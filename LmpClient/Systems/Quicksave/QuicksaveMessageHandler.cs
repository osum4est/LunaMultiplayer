using System;
using System.Collections.Concurrent;
using LmpClient.Base;
using LmpClient.Base.Interface;
using LmpClient.Systems.TimeSync;
using LmpClient.Systems.VesselProtoSys;
using LmpClient.Systems.Warp;
using LmpClient.VesselUtilities;
using LmpCommon.Message.Data.Quicksave;
using LmpCommon.Message.Interface;
using LmpCommon.Message.Types;

namespace LmpClient.Systems.Quicksave
{
    public class QuicksaveMessageHandler : SubSystem<QuicksaveSystem>, IMessageHandler
    {
        public ConcurrentQueue<IServerMessageBase> IncomingMessages { get; set; } =
            new ConcurrentQueue<IServerMessageBase>();

        public static event Action QuicksaveListLoaded;

        public void HandleMessage(IServerMessageBase msg)
        {
            if (!(msg.Data is QuicksaveBaseMsgData msgData)) return;

            switch (msgData.MessageType)
            {
                case QuicksaveMessageType.ListReply:
                    HandleList((QuicksaveListReplyMsgData)msgData);
                    break;
                case QuicksaveMessageType.LoadReply:
                    HandleLoad((QuicksaveLoadReplyMsgData)msgData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void HandleList(QuicksaveListReplyMsgData data)
        {
            System.Quicksaves.Clear();
            System.Quicksaves.AddRange(data.Quicksaves);
            QuicksaveListLoaded?.Invoke();
        }

        private static void HandleLoad(QuicksaveLoadReplyMsgData data)
        {
            // Make sure that the quicksave corresponds to current vessel
            if (data.QuicksaveInfo.VesselId != FlightGlobals.ActiveVessel.id)
            {
                LunaScreenMsg.PostScreenMessage("Got quicksave for wrong vessel!", 5,
                    ScreenMessageStyle.UPPER_CENTER);
                LunaLog.LogError(
                    $"[LMP]: Got quicksave for wrong vessel. Got id: {data.QuicksaveInfo.VesselId}. Current vessel id: {FlightGlobals.ActiveVessel.id}");
                return;
            }

            // Construct the ProtoVessel using the loaded byte data
            var vesselProto = new VesselProto
            {
                GameTime = data.QuicksaveInfo.GameTime,
                VesselId = data.QuicksaveInfo.VesselId,
                NumBytes = data.QuicksaveInfo.NumBytes,
                ForceReload = true,
                RawData = new byte[data.QuicksaveInfo.NumBytes]
            };
            Array.Copy(data.QuicksaveInfo.Data, vesselProto.RawData, data.QuicksaveInfo.NumBytes);
            var protoVessel = vesselProto.CreateProtoVessel();

            // Try to reload the vessel
            if (VesselLoader.LoadVessel(protoVessel, true))
            {
                // Create a new subspace with the loaded vessel's time
                TimeSyncSystem.Singleton.SetGameTime(protoVessel.launchTime + protoVessel.missionTime);
                WarpSystem.Singleton.WaitingSubspaceIdFromServer = true;
                WarpSystem.Singleton.MessageSender.SendNewSubspace();
                LunaScreenMsg.PostScreenMessage($"Loaded {data.QuicksaveInfo.Name}!", 5,
                    ScreenMessageStyle.UPPER_CENTER);
            }
            else
            {
                LunaScreenMsg.PostScreenMessage("Could not load quicksave!", 5,
                    ScreenMessageStyle.UPPER_CENTER);
                LunaLog.LogError(
                    $"[LMP]: Could not load quicksave {data.QuicksaveInfo.Name} for {data.QuicksaveInfo.VesselId}");
            }
        }
    }
}