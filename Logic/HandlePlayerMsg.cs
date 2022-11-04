using System;
using System.Collections.Generic;
using System.Text;
using MMONetworkServer.Core;
using Newtonsoft.Json;
namespace ServerLoginHotfix {
    //处理角色消息 ，具体是登录成功后的逻辑,比如:强化装备、打副本
    public partial class HandlePlayerMsg {
        public void MsgGetScore(Player player, ProtocolBase protoBase) {

            /*ProtocolBytes protocolRet = new ProtocolBytes();
            protocolRet.AddString("GetScore");
            protocolRet.AddInt(player.data.score);
            player.Send(protocolRet);
            Console.WriteLine("MsgGetScore " + player.id + player.data.score);*/
        }
        public void MsgAddScore(Player player, ProtocolBase protoBase) {
            /*//获取数值
            int start = 0;
            ProtocolBytes protocol = (ProtocolBytes)protoBase;
            string protoName = protocol.GetString(start, ref start);
            //处理
            player.data.score += 1;
            Console.WriteLine("MsgAddScore " + player.id + " " + player.data.score.ToString());*/
        }
        public void MsgWWWW(Player player, ProtocolBase protoBase) {
            Console.WriteLine("wwwwwww");
        }
        public void MsgGetPlayerData(Player player, ProtocolBase protoBase) {
            string data = JsonConvert.SerializeObject(player.data);
            ProtocolBytes protocolGetPlayerData = new ProtocolBytes();
            protocolGetPlayerData.AddString("GetPlayerData");
            protocolGetPlayerData.AddString(data);
            //player.Send(protocolGetPlayerData);
            player.conn.AsySend(protocolGetPlayerData);
        }
        public void MsgAddWeapon(Player player, ProtocolBase protoBase) {

        }

    }
}
