using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS2_003 : SimTemplate//Mind Vision //TODO: steal
    {

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int anz = (ownplay) ? p.playerSecond.owncards.Count : p.playerFirst.owncards.Count;
            if (anz >= 1)
            {
                p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay, true);
            }
        }

    }
}
