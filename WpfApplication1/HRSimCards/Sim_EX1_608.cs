using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_608 : SimTemplate //sorcerersapprentice
	{

//    eure zauber kosten (1) weniger.
        public override void onAuraStarts(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnsorcerersapprentice++;
            }
            else
            {
                p.playerSecond.anzOwnsorcerersapprentice++;
                
            }

        }

        public override void onAuraEnds(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnsorcerersapprentice--;
            }
            else
            {
                p.playerSecond.anzOwnsorcerersapprentice--;
            }
        }
	}
}