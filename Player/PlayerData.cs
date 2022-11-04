using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLoginHotfix {
    [Serializable]
    public class PlayerData:ActorData {
        public List<WeaponData> weaponDatas =new List<WeaponData>();
        public List<FoodData> foodDatas = new List<FoodData>();
    }
}
