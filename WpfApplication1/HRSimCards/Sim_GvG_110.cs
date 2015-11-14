using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_110 : SimTemplate //Dr. Boom
    {

        //  Battlecry: Summon two 1/1 Boom Bots. WARNING: Bots may explode. 

        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.GVG_110t);//chillwind

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            int pos = (own.own) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
            p.callKid(kid, pos, own.own);
            pos = (own.own) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
            p.callKid(kid, pos, own.own);
        }


    }

}