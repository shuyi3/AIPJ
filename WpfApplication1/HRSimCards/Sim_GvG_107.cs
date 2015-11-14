using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_107 : SimTemplate //Enhance-o Mechano
    {

        //  Battlecry: Give your other minions Windfury Taunt or Divine Shield

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            List<Minion> temp = (own.own) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;

            foreach (Minion m in temp)
            {
                m.taunt = true;
            }

        }


    }

}