using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_NEW1_008: SimTemplate//ancient of lore
    {

        //Zieht 2 Karten; oder stellt 5 Leben wieder her
        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            if (choice == 2)
            {
                int heal = p.getMinionHeal(5, own.own);
                p.minionGetDamageOrHeal(target, -heal);
            }
            else
            {
                p.drawACard(p.getArandomCardFromDeck(own.own), own.own);
                p.drawACard(p.getArandomCardFromDeck(own.own), own.own);
            }
        }

    }
}
