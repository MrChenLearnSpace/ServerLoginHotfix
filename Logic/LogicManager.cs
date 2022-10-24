using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
//using MMONetworkServer.Logic;
using MMONetworkServer.net;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MMONetworkServer.Core;
using System.Text.RegularExpressions;
//using System.Web.Script.Serialization;
namespace MMONetworkServer.Logic {
    public class LogicManager {
        // public static LogicManager instance;
      //  JavaScriptSerializer Js = new JavaScriptSerializer();
        public LogicManager() {
            //if (instance == null)
            //    instance = this;
        }
        public byte[] Serialize(PlayerData playerData) {
            //序列化
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            //PlayerData playerData = new PlayerData();

            try {
               // string data = Js.Serialize(playerData);
                formatter.Serialize(stream, playerData);

                //return Encoding.UTF8.GetBytes(data);
                return stream.ToArray();
            }
            catch (Exception e) {
                Console.WriteLine("[DataMgr]CreatePlayer 序列化" + e.Message);
                return null;
            }
        }
        public bool UnSerialize(byte[] playerStream, ref PlayerData playerdata) {
            //CodeLoader.instance.hotfix.b
            MemoryStream stream = new MemoryStream(playerStream);
            try {
                BinaryFormatter formatter = new BinaryFormatter();
                //formatter.Binder = new AssemlyDiffuse();
                 playerdata = (PlayerData)formatter.Deserialize(stream);
               // string data = Encoding.UTF8.GetString(playerStream);
                //playerdata = Js.Deserialize<PlayerData>(data);
                return true;
            }
            catch (SerializationException e) {
                Console.WriteLine("[DataMgr]GetPlayerData 反序列化" + e.Message);
                return false;
            }
        }
        public bool CreatePlayer(string id ,Conn conn) {
            if (!DataMgr.instance.IsSafeStr(id))
                return false;
            PlayerData playerData = new PlayerData();
            byte[] buffer =Serialize(playerData);
            return DataMgr.instance.InsertPlayer(id, buffer, conn.GetAdress());
        }
        public  bool KickOff(string id, ProtocolBase proto) {
            Conn[] conns = ServNet.instance.conns;
            for (int i = 0; i < conns.Length; i++) {
                if (!conns[i].isUse) continue;
                if (conns[i].player == null) continue;
                if (conns[i].player == null) continue;
                if (conns[i].player.GetId() == id) {
                    lock (conns[i].player) {
                        if (proto != null) {
                            conns[i].player.Send(proto);
                        }
                        return conns[i].player.Logout();
                    }
                }
            }
            return true;
        }
    }
}
