using System.Collections.Generic;
using System.Linq;
using LmpClient.Base;
using LmpClient.VesselUtilities;
using LmpCommon.Enums;
using LmpCommon.Message.Data.Quicksave;

namespace LmpClient.Systems.Quicksave
{
    public class QuicksaveSystem : MessageSystem<QuicksaveSystem, QuicksaveMessageSender, QuicksaveMessageHandler>
    {
        public List<QuicksaveBasicInfo> Quicksaves { get; } = new List<QuicksaveBasicInfo>();
        public override string SystemName { get; } = nameof(QuicksaveSystem);

        public bool CanQuickSaveLoad => MainSystem.NetworkState >= ClientState.Running &&
                                        HighLogic.LoadedSceneIsFlight && !VesselCommon.IsSpectating;

        protected override void OnEnabled()
        {
            base.OnEnabled();
            Quicksaves.Clear();
            SetupRoutine(new RoutineDefinition(0, RoutineExecution.Update, CheckHotKeys));
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            Quicksaves.Clear();
        }

        public void CheckHotKeys()
        {
            if (GameSettings.QUICKSAVE.GetKeyDown())
            {
                if (CanQuickSaveLoad)
                {
                    MessageSender.SendQuicksaveListRequestMsg();
                    QuicksaveMessageHandler.QuicksaveListLoaded += Quicksave;
                }
            }
            else if (GameSettings.QUICKLOAD.GetKeyDown())
            {
                if (CanQuickSaveLoad)
                {
                    MessageSender.SendQuicksaveListRequestMsg();
                    QuicksaveMessageHandler.QuicksaveListLoaded += Quickload;
                }
            }
        }
        
        private void Quicksave()
        {
            QuicksaveMessageHandler.QuicksaveListLoaded -= Quicksave;
            MessageSender.SendQuicksaveSaveRequestMsg($"Quicksave #{Quicksaves.Count + 1}");
        }
        
        private void Quickload() {
            QuicksaveMessageHandler.QuicksaveListLoaded -= Quickload;
            if (Quicksaves.Count > 0)
                MessageSender.SendQuicksaveLoadRequestMsg(Quicksaves.OrderBy(q => q.Date).Last());
        }
    }
}