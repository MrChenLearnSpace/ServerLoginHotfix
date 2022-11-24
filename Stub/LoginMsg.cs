using System;
using System.Collections.Generic;
using System.Text;
[Serializable]
public class MsgLogin : MsgBase {
    public MsgLogin() { protoName = "MsgLogin"; }
    public string id = "";
    public string pw ="";
    public int result = 0;
}

