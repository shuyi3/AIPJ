using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_NAX5_03 : SimTemplate //* Mindpocalypse
    {
        // Both players draw 2 cards and gain a Mana Crystal.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.drawACard(p.getArandomCardFromDeck(true), true);
            p.drawACard(p.getArandomCardFromDeck(false), false);
			
			p.playerFirst.mana = Math.Min(10, p.playerFirst.mana+1);
			p.playerFirst.ownMaxMana = Math.Min(10, p.playerFirst.ownMaxMana+1);
			p.playerSecond.ownMaxMana = Math.Min(10, p.playerSecond.ownMaxMana+1);
        }
    }
}