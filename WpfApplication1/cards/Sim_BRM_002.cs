using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
    class Sim_BRM_002 : SimTemplate //* Flamewaker
    {
        // After you cast a spell, deal 2 damage randomly split among all enemies.

        public override void onCardIsGoingToBePlayed(Playfield p, CardDB.Card c, bool ownplay, Minion m)
        {
            if (m.own == ownplay && c.type == CardDB.cardtype.SPELL)
            {
                Minion target = (ownplay) ? p.enemyHero : p.ownHero;
                p.minionGetDamageOrHeal(target, 1);

                List<Minion> temp = (ownplay) ? p.enemyMinions : p.ownMinions;
                if (temp.Count > 0) target = p.searchRandomMinion(temp, Playfield.searchmode.searchLowestHP);
                if (target == null) target = (ownplay) ? p.enemyHero : p.ownHero;
                p.minionGetDamageOrHeal(target, 1);
            }
        }
    }
}