using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_323 : SimTemplate //lordjaraxxus
	{
        CardDB.Card card = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_tk33);
        CardDB.Card weapon = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_323w);
        
//    kampfschrei:/ vernichtet euren helden und ersetzt ihn durch lord jaraxxus.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            if (own.own)
            {
                p.playerFirst.ownHeroAblility.card = card;
                p.playerFirst.ownHeroName = HeroEnum.lordjaraxxus;
                p.playerFirst.ownHero.Hp = own.Hp;
                p.playerFirst.ownHero.maxHp = own.maxHp;

                p.equipWeapon(weapon, own.own);
            }
            else 
            {
                p.playerSecond.ownHeroAblility.card = card;
                p.playerSecond.ownHeroName = HeroEnum.lordjaraxxus;
                p.playerSecond.ownHero.Hp = own.Hp;
                p.playerSecond.ownHero.maxHp = own.maxHp;

                p.equipWeapon(weapon, own.own);
            }
		}

	}
}