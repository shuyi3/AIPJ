using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_123 : SimTemplate //Soot Spewer
    {

        //   Spell Damage +1

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

    }

}