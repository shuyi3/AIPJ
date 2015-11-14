using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_080 : SimTemplate //Druid of the Fang
    {

        //   Battlecry:If you have a Beast, transform this minion into a 7/7.
        CardDB.Card betterguy = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.GVG_080t);
        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            List<Minion> temp = (own.own) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            bool hasbeast = false;
            foreach (Minion m in temp)
            {
                if ((TAG_RACE)m.handcard.card.race == TAG_RACE.PET)
                {
                    hasbeast = true;
                }
            }
            if(hasbeast) p.minionTransform(own, betterguy);
        }


    }

}