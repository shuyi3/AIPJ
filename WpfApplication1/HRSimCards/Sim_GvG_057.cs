using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_057 : SimTemplate //Seal of Light
    {

        //   Restore #4 Health to your hero and gain +2 Attack this turn.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            if (ownplay)
            {
                int heal = p.getSpellHeal(4, ownplay);
                p.minionGetDamageOrHeal(p.playerFirst.ownHero, -heal);
                p.minionGetTempBuff(p.playerFirst.ownHero, 2, 0);
            }
            else
            {
                int heal = p.getSpellHeal(4, ownplay);
                p.minionGetDamageOrHeal(p.playerSecond.ownHero, -heal);
                p.minionGetTempBuff(p.playerSecond.ownHero, 2, 0);
            }

        }


    }

}