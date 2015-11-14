using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_028t : SimTemplate //Gallywix's Coin
    {

        //    Gain 1 Mana Crystal this turn only.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.playerFirst.mana++;
        }


    }

}