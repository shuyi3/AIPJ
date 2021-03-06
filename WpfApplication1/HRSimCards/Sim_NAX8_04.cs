using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NAX8_04 : SimTemplate //* Unrelenting Warrior
	{
//    Deathrattle:: Summon a Spectral Warrior for your opponent.
        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.NAX8_04t); //Spectral Warrior
		
        public override void onDeathrattle(Playfield p, Minion m)
        {
            int place = (m.own) ? p.playerSecond.ownMinions.Count : p.playerFirst.ownMinions.Count;
            p.callKid(kid, place, !m.own);
        }
	}
}