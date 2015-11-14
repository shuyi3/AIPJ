using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_563 : SimTemplate //malygos
	{

//    zauberschaden +5/
        public override void onAuraStarts(Playfield p, Minion own)
		{
            if (own.own)
            {
                p.playerFirst.spellpower+=5;
            }
            else
            {
                p.playerSecond.spellpower+=5;
            }
		}

        public override void onAuraEnds(Playfield p, Minion m)
        {
            if (m.own)
            {
                p.playerFirst.spellpower-=5;
            }
            else
            {
                p.playerSecond.spellpower-=5;
            }
        }

	}
}