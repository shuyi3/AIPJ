using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_032a : SimTemplate //* Grove Tender
    {

        //   Give each player a Mana Crystal.

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            p.playerFirst.mana = Math.Min(10, p.playerFirst.mana+1);
            p.playerFirst.ownMaxMana = Math.Min(10, p.playerFirst.ownMaxMana+1);
            p.playerSecond.ownMaxMana = Math.Min(10, p.playerSecond.ownMaxMana+1);
        }


    }

}