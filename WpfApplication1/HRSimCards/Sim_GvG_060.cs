using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_060 : SimTemplate //Quartermaster
    {

        //   Battlecry: Give your Silver Hand Recruits +2/+2.

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            List<Minion> temp = (own.own) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            foreach (Minion m in temp)
            {
                if (m.name == CardDB.cardName.silverhandrecruit) p.minionGetBuffed(m, 2, 2);
            }
        }

       
    }

}