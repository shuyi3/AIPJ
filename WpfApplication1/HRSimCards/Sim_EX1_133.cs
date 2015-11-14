using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{

    class Sim_EX1_133 : SimTemplate//pertitions blade
    {
        CardDB.Card w = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_133);

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(1, ownplay);
            if (p.playerFirst.cardsPlayedThisTurn >= 1) dmg = p.getSpellDamageDamage(2, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            p.equipWeapon(w, ownplay);
        }

    }

    
}
