using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_006 : SimTemplate //adrenalinerush
	{

//    draw a card. combo:/ draw 2 cards instead.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.drawACard(CardDB.cardName.unknown, ownplay);
            if (p.cardsPlayedThisTurn >= 1) p.drawACard(CardDB.cardName.unknown, ownplay);
		}

	}
}