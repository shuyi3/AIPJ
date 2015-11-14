using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_NEW1_036 : SimTemplate//commanding shout
    {

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            List<Minion> temp = (ownplay) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            foreach (Minion t in temp)
            {
                t.cantLowerHPbelowONE = true;
            }
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
        }

    }
}
