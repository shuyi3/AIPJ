using System;
using System.Collections.Generic;
using System.Text;

namespace HREngine.Bots
{
    class Sim_NAX11_04 : SimTemplate//* Mutating Injection
    {
	// Give a minion +4/+4 and Taunt.
	
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            target.taunt = true;
            p.minionGetBuffed(target, 4, 4);
        }
    }
}
