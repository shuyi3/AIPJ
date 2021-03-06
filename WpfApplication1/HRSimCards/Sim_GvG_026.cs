using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_026 : SimTemplate //Feign Death
    {

        //   Trigger all Deathrattles on your minions.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            if (ownplay)
            {
                p.doDeathrattles(new List<Minion>(p.playerFirst.ownMinions));
            }
            else
            {
                p.doDeathrattles(new List<Minion>(p.playerSecond.ownMinions));
            }
        }


    }

}