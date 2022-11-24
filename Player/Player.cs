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
using MMONetworkServer;
namespace ServerLoginHotfix {
   // 游戏中的角色，功能包括:给角色发消息、踢下线、保存角色数据等
    public class Player : IPlayer {
        public string id;
        public Conn conn;
        public PlayerData data;
        public PlayerTempData tempData;
        HandlePlayerEvent handlePlayerEvent = new HandlePlayerEvent();
        LogicManager logicManager = new LogicManager();
        public Player() { }
        public Player(string id , Conn conn) {
            this.id = id;
            this.conn = conn;
            tempData = new PlayerTempData();
        }
        public  void Send(ProtocolBase protocol) {
            if (conn == null)
                return;
            conn.Send(protocol);
            //ServNet.instance.Send(conn, protocol);
        }


        public bool Logout() {
            //ServNet.instance.handlePlayerEvent.OnLogout(this);

            //CodeLoader.GetInstance().FindFunRun("MMONetworkServer.Logic.HandlePlayerEvent", "OnLogout", new object[] { this });
            handlePlayerEvent.OnLogout(this);
            if (!SavePlayer())
                return false;
            conn.player = null;
            conn.Close();
            return true;
        }

       
        public Conn GetConn() {
            return conn;
        }

        public string GetId() {
            return id;
        }



        public bool SavePlayer() {

            if (!DataMgr.instance.IsSafeStr(id))
                return false;
            //LogicManager logic = new LogicManager();
            byte[] buff = logicManager.Serialize(data);
            return DataMgr.instance.SavePlayerStream(id, buff, conn.GetAdress());
        }

        public bool GetPlayerData() {
            if (!DataMgr.instance.IsSafeStr(id))
                return false;
            byte[] buff = DataMgr.instance.GetDataStream(id);
            if (buff == null) {
                Console.WriteLine("buff is null");
                return false;
            }
            return logicManager.UnSerialize(buff,ref data);
        }

    }
}
