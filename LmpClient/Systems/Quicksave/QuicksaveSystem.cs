using System.Collections.Generic;
using LmpClient.Base;
using LmpCommon.Message.Data.Quicksave;

namespace LmpClient.Systems.Quicksave
{
    public class QuicksaveSystem : MessageSystem<QuicksaveSystem, QuicksaveMessageSender, QuicksaveMessageHandler>
    {
        public List<QuicksaveBasicInfo> Quicksaves { get; } = new List<QuicksaveBasicInfo>();
        public override string SystemName { get; } = nameof(QuicksaveSystem);

        protected override void OnEnabled()
        {
            base.OnEnabled();
            Quicksaves.Clear();
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            Quicksaves.Clear();
        }
    }
}