using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_031 : SimTemplate //animalcompanion
	{

//    ruft einen zufälligen wildtierbegleiter herbei.
        CardDB.Card c2 = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.NEW1_032);//misha
        
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int placeoffather = (ownplay)?  p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
            p.callKid(c2, placeoffather, ownplay);
		}

	}
}