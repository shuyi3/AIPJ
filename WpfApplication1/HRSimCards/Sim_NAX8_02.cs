using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NAX8_02 : SimTemplate //* Harvest
	{

//    Hero PowerDraw a card.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
        }
	}
}