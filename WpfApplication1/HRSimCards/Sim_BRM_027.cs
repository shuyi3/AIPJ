using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_027 : SimTemplate //* Majordomo Executus
	{
		//Deathrattle: Replace your hero with Ragnaros, the Firelord.
		
        CardDB.Card card = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.BRM_027p); // DIE, INSECT!
        
		public override void onDeathrattle(Playfield p, Minion m)
        {			
			if (m.own)
            {
                p.playerFirst.ownHeroAblility.card = card;
                p.playerFirst.ownAbilityReady = true;
                p.playerFirst.ownHeroName = HeroEnum.ragnarosthefirelord;
                p.playerFirst.ownHero.Hp = 8;
                p.playerFirst.ownHero.maxHp = 8;
            }
            else
            {
                p.playerSecond.ownHeroAblility.card = card;
                p.playerSecond.ownAbilityReady = true;
                p.playerSecond.ownHeroName = HeroEnum.ragnarosthefirelord;
                p.playerSecond.ownHero.Hp = 8;
                p.playerSecond.ownHero.maxHp = 8;
            }
        }
	}
}