using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_012 : SimTemplate//bloodmage thalnos
    {
        public override void onAuraStarts(Playfield p, Minion own)
        {
           
            if (own.own)
            {
                p.playerFirst.spellpower++;
            }
            else
            {
                p.playerSecond.spellpower++;
            }
        }

        public override void onAuraEnds(Playfield p, Minion m)
        {
            if (m.own)
            {
                p.playerFirst.spellpower--;
            }
            else
            {
                p.playerSecond.spellpower--;
            }
        }

        public override void onDeathrattle(Playfield p, Minion m)
        {
            p.drawACard(p.getArandomCardFromDeck(m.own), m.own);
        }

    }
}
