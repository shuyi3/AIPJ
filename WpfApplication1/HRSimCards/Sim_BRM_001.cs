using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_001 : SimTemplate //* Solemn Vigil
	{
		// Draw 2 cards. Costs (1) less for each minion that died this turn.
		
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}
	}
}