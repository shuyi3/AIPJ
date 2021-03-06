using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NAX8_05 : SimTemplate //* Unrelenting Rider
	{
//    Deathrattle:: Summon a Spectral Rider for your opponent.
        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.NAX8_05t); //Spectral Rider
		
        public override void onDeathrattle(Playfield p, Minion m)
        {
            int place = (m.own) ? p.playerSecond.ownMinions.Count : p.playerFirst.ownMinions.Count;
            p.callKid(kid, place, !m.own);
        }
	}
}