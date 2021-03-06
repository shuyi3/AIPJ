using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_018 : SimTemplate //* Dragon Consort
	{
		// Battlecry: The next Dragon you play costs (2) less.

		public override void getBattlecryEffect(Playfield p, Minion m, Minion target, int choice)
		{
            if (m.own)
            {
                p.playerFirst.anzOwnDragonConsort++;
            }
            else
            {
                p.playerSecond.anzOwnDragonConsort++;
            }
		}
	}
}