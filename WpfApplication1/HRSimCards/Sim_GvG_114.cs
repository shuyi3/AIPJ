using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_114 : SimTemplate //Sneed's Old Shredder
    {

        //   Deathrattle: Summon a random legendary minion.
        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_014);

        public override void onDeathrattle(Playfield p, Minion m)
        {
            int pos = (m.own) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
            p.callKid(kid, pos, m.own); 
        }


    }

}