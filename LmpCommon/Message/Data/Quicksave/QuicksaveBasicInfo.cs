using System;
using Lidgren.Network;
using LmpCommon.Message.Base;

namespace LmpCommon.Message.Data.Quicksave
{
    public class QuicksaveBasicInfo
    {
        public string Name;
        public DateTime Date;
        public Guid VesselId;

        public virtual void Serialize(NetOutgoingMessage lidgrenMsg)
        {
            lidgrenMsg.Write(Name);
            lidgrenMsg.Write(Date.Ticks);
            GuidUtil.Serialize(VesselId, lidgrenMsg);
        }

        public virtual void Deserialize(NetIncomingMessage lidgrenMsg)
        {
            Name = lidgrenMsg.ReadString();
            Date = new DateTime(lidgrenMsg.ReadInt64());
            VesselId = GuidUtil.Deserialize(lidgrenMsg);
        }

        public virtual int GetByteCount()
        {
            return Name.GetByteCount() + sizeof(long) + GuidUtil.ByteSize;
        }
    }
}