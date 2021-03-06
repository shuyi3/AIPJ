using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_101 : SimTemplate //Scarlet Purifier
    {

        //   Battlecry: Deal 2 damage to all minions with Deathrattle.

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {

            foreach (Minion m in p.playerFirst.ownMinions)
            {
                if (m.handcard.card.deathrattle) p.minionGetDamageOrHeal(m, 2);
            }
            foreach (Minion m in p.playerSecond.ownMinions)
            {
                if (m.handcard.card.deathrattle) p.minionGetDamageOrHeal(m, 2);
            }

        }


    }

}