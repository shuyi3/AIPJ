using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_017 : SimTemplate //* Resurrect //TODO: REAL ONE INSTEAD OF NONE
	{
		// Summon a random friendly minion that died this game.
		
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            if (p.playerFirst.ownMaxMana >= 6)
            {
                int posi = ownplay ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
                CardDB.Card kid = CardDB.Instance.getCardDataFromID((p.playerFirst.ownLastDiedMinion == CardDB.cardIDEnum.None) ? CardDB.cardIDEnum.EX1_345t : p.playerFirst.ownLastDiedMinion); // Shadow of Nothing 0:1 or ownMinion
                p.callKid(kid, posi, ownplay);
            }
		}
	}
}