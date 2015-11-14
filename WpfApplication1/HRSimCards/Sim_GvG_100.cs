using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_100 : SimTemplate //* Floating Watcher
    {
        // Whenever your hero takes damage on your turn, gain +2/+2.  
		
        public override void onMinionGotDmgTrigger(Playfield p, Minion m, bool ownDmgdmin)
        {
            if (p.playerFirst.ownHero.anzGotDmg>=1 && m.own)
            {
                p.minionGetBuffed(m, 2 * p.playerFirst.ownHero.anzGotDmg, 2 * p.playerFirst.ownHero.anzGotDmg);
            }
            else if (p.playerSecond.ownHero.anzGotDmg>=1 && !m.own)
            {
                p.minionGetBuffed(m, 2 * p.playerSecond.ownHero.anzGotDmg, 2 * p.playerSecond.ownHero.anzGotDmg);
            }
        }
    }
}