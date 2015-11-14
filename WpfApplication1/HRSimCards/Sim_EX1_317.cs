using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_317 : SimTemplate //sensedemons
	{

//    fügt eurer hand zwei zufällige dämonen aus eurem deck hinzu.
        //TODO: sense demons instead of random cards

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
		}

	}
}