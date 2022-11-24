using System;
using System.Collections.Generic;
using System.Text;
using MMONetworkServer.Core;
using System.Reflection;
using MMONetworkServer;
using MMONetworkServer.net;
namespace ServerLoginHotfix {
    //处理连接消息 ，具体是登录前的逻辑，比如:用户名密码校验、注册账号
    public partial class HandleConnMsg {

        static HandlePlayerEvent handlePlayerEvent = new HandlePlayerEvent();
        static LogicManager logicManager ;
        public HandleConnMsg() {
            logicManager = new LogicManager();
        }
        public void MsgHeatBeat(Conn conn, ProtocolBase protoBase) {
            conn.lastTickTime = Sys.GetTimeStamp();
            Console.WriteLine("[更新心跳时间]" + conn.GetAdress());

        }
        public void MsgRegister(Conn conn, ProtocolBase protoBase) {
            //获取数值
            int start = 0;
            ProtocolBytes protocol = (ProtocolBytes)protoBase;
            
            string protoName = protocol.GetString(start, ref start);
        

            string id = protocol.GetString(start, ref start);
            string pw = protocol.GetString(start, ref start);
            string strFormat = "[收到注册协议]" + conn.GetAdress();
            Console.WriteLine(strFormat + " 用户名：" + id + " 密码：" + pw);
            //构建返回协议
            protocol = new ProtocolBytes();
            protocol.AddString("MsgRegister");
            //注册
            if (DataMgr.instance.Register(id, pw)) {
                protocol.AddInt(0);
            }
            else {
                protocol.AddInt(-1);
            }
            //创建角色

            // DataMgr.instance.CreatePlayer(id);
            logicManager.CreatePlayer(id, conn);
            //返回协议给客户端
            conn.Send(protocol);
        }
        //public void MsgLogin(Conn conn, ProtocolBase protoBase) {
        //    //获取数值
        //    //int start = 0;
        //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
        //    //string protoName = protocol.GetString(start, ref start);
        //    MsgLogin msgLogin = new MsgLogin();
        //    msgLogin = (MsgLogin)msgLogin.Decode(protocol);
        //    string id = msgLogin.id;
        //    string pw = msgLogin.pw;
        //    //string id = protocol.GetString(start, ref start);
        //    //string pw = protocol.GetString(start, ref start);
        //    string strFormat = "[收到登录协议]" + conn.GetAdress();
        //    Console.WriteLine(strFormat + " 用户名：" + id + " 密码：" + pw);
        //    //构建返回协议
        //    ProtocolBytes protocolRet = new ProtocolBytes();
        //    protocolRet.AddString("Login");
        //    //验证
        //    if (!DataMgr.instance.CheckPassWord(id, pw)) {
        //        protocolRet.AddInt(-1);
        //        conn.Send(protocolRet);
        //        Console.WriteLine(strFormat + " 用户名：" + id + " 验证");
        //        return;
        //    }
        //    //是否已经登录
        //    ProtocolBytes protocolLogout = new ProtocolBytes();
        //    protocolLogout.AddString("Logout");
        //   // logicManager = new LogicManager();
        //    if (!logicManager.KickOff(id, protocolLogout)) {
        //        protocolRet.AddInt(-1);
        //        conn.Send(protocolRet);
        //        Console.WriteLine(strFormat + " 用户名：" + id + " 是否已经登录");

        //        return;
        //    }
        //    //获取玩家数据
        //    conn.player = new Player(id, conn);

        //    if (!conn.player.GetPlayerData()) {
        //        protocolRet.AddInt(-1);
        //        conn.Send(protocolRet);
        //        Console.WriteLine(strFormat + " 用户名：" + id + " 获取玩家数据");

        //        return;
        //    }
        //    handlePlayerEvent.OnLogin(conn.player);
        //    //返回
        //    protocolRet.AddInt(0);
        //    conn.Send(protocolRet);
        //    return;
        //}
        public void MsgLogin(Conn conn, ProtocolBase protoBase) {

            ProtocolBytes protocol = (ProtocolBytes)protoBase;
            MsgLogin msgLogin = new MsgLogin();
            msgLogin = (MsgLogin)msgLogin.Decode(protocol);
            string id = msgLogin.id;
            string pw = msgLogin.pw;
           
            string strFormat = "[收到登录协议]" + conn.GetAdress();
            Console.WriteLine(strFormat + " 用户名：" + id + " 密码：" + pw);
            //构建返回协议
            //ProtocolBytes protocolRet = new ProtocolBytes();
            //protocolRet.AddString("Login");
            //验证
            if (!DataMgr.instance.CheckPassWord(id, pw)) {
                //protocolRet.AddInt(-1);
                msgLogin.result = -1;
                protocol = msgLogin.Encode();
                conn.Send(protocol);
                Console.WriteLine(strFormat + " 用户名：" + id + " 验证");
                return;
            }
            //是否已经登录
            ProtocolBytes protocolLogout = new ProtocolBytes();
            protocolLogout.AddString("MsgLogout");
            // logicManager = new LogicManager();
            if (!logicManager.KickOff(id, protocolLogout)) {
                msgLogin.result = -1;
                protocol = msgLogin.Encode();
                conn.Send(protocol);
                Console.WriteLine(strFormat + " 用户名：" + id + " 是否已经登录");

                return;
            }
            //获取玩家数据
            conn.player = new Player(id, conn);

            if (!conn.player.GetPlayerData()) {
                msgLogin.result = -1;
                protocol = msgLogin.Encode();
                conn.Send(protocol);
                Console.WriteLine(strFormat + " 用户名：" + id + " 获取玩家数据");

                return;
            }
            handlePlayerEvent.OnLogin(conn.player);
            //返回
            msgLogin.result = 0;
            protocol = msgLogin.Encode();
            conn.Send(protocol);
            return;
        }



        public void MsgLogout(Conn conn, ProtocolBase protoBase) {
            ProtocolBytes protocol = new ProtocolBytes();
            protocol.AddString("MsgLogout");
            protocol.AddInt(0);
            if (conn.player == null) {
                conn.Send(protocol);
                conn.Close();
            }
            else {
                conn.Send(protocol);
                conn.player.Logout();
            }
        }

        public void MsgWWWW(Conn conn, ProtocolBase protoBase) {
            ProtocolBytes protocol = (ProtocolBytes)protoBase;
            ProtocolBytes  str = new ProtocolBytes();
            str.AddString("ssss");
            str.AddString("dsadsadas");
            conn.AsySend(str);
            Console.WriteLine(protocol.GetDesc());
        }
    }
}
