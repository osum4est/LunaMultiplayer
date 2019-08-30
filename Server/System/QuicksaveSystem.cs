using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LmpCommon.Message.Data.Quicksave;
using LmpCommon.Message.Server;
using Server.Client;
using Server.Context;
using Server.Log;
using Server.Server;

namespace Server.System
{
    public class QuicksaveSystem
    {
        private static readonly string QuicksavesPath = Path.Combine(ServerContext.UniverseDirectory, "Quicksaves");

        public static void SendQuicksaveList(ClientStructure client, QuicksaveListRequestMsgData data)
        {
            Task.Run(() =>
            {
                SendQuicksaveList(client, data.VesselId);
                RemoveOldQuicksaves();
            });
        }

        public static void LoadQuicksave(ClientStructure client, QuicksaveLoadRequestMsgData data)
        {
            Task.Run(() =>
            {
                var quicksaveFile = GetQuicksaveFile(data.QuicksaveInfo.Name, data.QuicksaveInfo.VesselId);
                if (!File.Exists(quicksaveFile))
                {
                    LunaLog.Warning($"Could not find quicksave {quicksaveFile}");
                    return;
                }

                var vesselData = File.ReadAllText(quicksaveFile);
                var msgData = ServerContext.ServerMessageFactory.CreateNewMessageData<QuicksaveLoadReplyMsgData>();
                msgData.QuicksaveInfo = new QuicksaveInfo
                {
                    Name = data.QuicksaveInfo.Name,
                    Date = data.QuicksaveInfo.Date,
                    VesselId = data.QuicksaveInfo.VesselId,
                    Data = Encoding.UTF8.GetBytes(vesselData),
                    NumBytes = vesselData.Length
                };

                MessageQueuer.SendToClient<QuicksaveSrvMsg>(client, msgData);
                RemoveOldQuicksaves();
            });
        }

        public static void SaveQuicksave(ClientStructure client, QuicksaveSaveRequestMsgData data)
        {
            Task.Run(() =>
            {
                var quicksaveFile = GetQuicksaveFile(data.Name, data.VesselId);
                if (File.Exists(quicksaveFile))
                    File.Delete(quicksaveFile);

                if (!VesselStoreSystem.CurrentVessels.ContainsKey(data.VesselId))
                {
                    LunaLog.Warning($"Could save vessel with id {data.VesselId}");
                    return;
                }

                File.WriteAllText(quicksaveFile,
                    VesselStoreSystem.CurrentVessels[data.VesselId].ToString());

                SendQuicksaveList(client, data.VesselId);
                RemoveOldQuicksaves();
            });
        }

        public static void RemoveQuicksave(ClientStructure client, QuicksaveRemoveRequestMsgData data)
        {
            Task.Run(() =>
            {
                var quicksaveFile = GetQuicksaveFile(data.QuicksaveInfo.Name, data.QuicksaveInfo.VesselId);
                if (File.Exists(quicksaveFile))
                    File.Delete(quicksaveFile);

                SendQuicksaveList(client, data.QuicksaveInfo.VesselId);
                RemoveOldQuicksaves();
            });
        }

        private static string GetVesselFolder(Guid vesselId)
        {
            var folder = Path.Combine(QuicksavesPath, vesselId.ToString());
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        private static string GetQuicksaveFile(string name, Guid vesselId)
        {
            return Path.Combine(GetVesselFolder(vesselId), name) + ".txt";
        }

        private static void SendQuicksaveList(ClientStructure client, Guid vesselId)
        {
            var vesselFolder = GetVesselFolder(vesselId);
            var files = Directory.GetFiles(vesselFolder);

            var msgData = ServerContext.ServerMessageFactory.CreateNewMessageData<QuicksaveListReplyMsgData>();
            msgData.QuicksavesCount = files.Length;
            msgData.Quicksaves = new QuicksaveBasicInfo[files.Length];
            for (var i = 0; i < files.Length; i++)
                msgData.Quicksaves[i] = new QuicksaveBasicInfo
                {
                    Name = Path.GetFileNameWithoutExtension(files[i]),
                    Date = File.GetCreationTime(files[i]),
                    VesselId = vesselId
                };

            MessageQueuer.SendToClient<QuicksaveSrvMsg>(client, msgData);
        }

        private static void RemoveOldQuicksaves()
        {
            foreach (var vesselQuicksaveFolder in Directory.EnumerateDirectories(QuicksavesPath))
            {
                var vesselId = Path.GetFileName(vesselQuicksaveFolder);
                if (vesselId == null)
                    continue;
                
                if (!VesselStoreSystem.VesselExists(Guid.Parse(vesselId)))
                    Directory.Delete(vesselQuicksaveFolder, true);
            }
        }
    }
}