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
                p.ownHeroAblility.card = card;
                p.ownAbilityReady = true;
                p.ownHeroName = HeroEnum.ragnarosthefirelord;
                p.ownHero.Hp = 8;
                p.ownHero.maxHp = 8;
            }
            else
            {
                p.enemyHeroAblility.card = card;
                p.enemyAbilityReady = true;
                p.enemyHeroName = HeroEnum.ragnarosthefirelord;
                p.enemyHero.Hp = 8;
                p.enemyHero.maxHp = 8;
            }
        }
	}
}