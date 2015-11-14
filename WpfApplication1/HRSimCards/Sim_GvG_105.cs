using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_105 : SimTemplate //Piloted Sky Golem
    {

        // Deathrattle: Summon a random 4-Cost minion.  

        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.CS2_182);//chillwind

        public override void onDeathrattle(Playfield p, Minion m)
        {

            int pos = (m.own) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
            p.callKid(kid, pos, m.own);
        }


    }

}