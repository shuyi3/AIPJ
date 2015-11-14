using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS2_026 : SimTemplate //frostnova
    {

        //    friert/ alle feindlichen diener ein.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            List<Minion> temp = (ownplay) ? p.playerSecond.ownMinions : p.playerFirst.ownMinions;
            foreach (Minion t in temp)
            {
                t.frozen = true;
            }
        }
    }
}