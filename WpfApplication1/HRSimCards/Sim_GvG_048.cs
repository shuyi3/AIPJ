using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_048 : SimTemplate //Metaltooth Leaper
    {

        //   Battlecry: Give your other Mechs +2 Attack.

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            List<Minion> temp = (own.own) ? p.playerSecond.ownMinions : p.playerFirst.ownMinions;
            foreach (Minion m in temp)
            {
                if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MECHANICAL)
                {
                    p.minionGetBuffed(m, 2, 0);
                }
            }

        }

       

    }

}