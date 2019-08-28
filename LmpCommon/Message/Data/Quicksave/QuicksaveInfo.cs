using Lidgren.Network;
using LmpCommon.Message.Base;

namespace LmpCommon.Message.Data.Quicksave
{
    public class QuicksaveInfo : QuicksaveBasicInfo
    {
        public int NumBytes;
        public byte[] Data;
        public double GameTime;

        public override void Serialize(NetOutgoingMessage lidgrenMsg)
        {
            base.Serialize(lidgrenMsg);

            Common.ThreadSafeCompress(this, ref Data, ref NumBytes);
            lidgrenMsg.Write(NumBytes);
            lidgrenMsg.Write(Data, 0, NumBytes);
            lidgrenMsg.Write(GameTime);
        }

        public override void Deserialize(NetIncomingMessage lidgrenMsg)
        {
            base.Deserialize(lidgrenMsg);

            NumBytes = lidgrenMsg.ReadInt32();
            Data = new byte[NumBytes];
            lidgrenMsg.ReadBytes(Data, 0, NumBytes);
            Common.ThreadSafeDecompress(this, ref Data, NumBytes, out NumBytes);
            GameTime = lidgrenMsg.ReadDouble();
        }

        public override int GetByteCount()
        {
            return base.GetByteCount() + Name.GetByteCount() + sizeof(long) + sizeof(int) + sizeof(byte) * NumBytes +
                   sizeof(double);
        }
    }
}