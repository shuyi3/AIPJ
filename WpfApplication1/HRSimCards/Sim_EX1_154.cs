using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_154 : SimTemplate //wrath
    {

        //    wählt aus:/ fügt einem diener $3 schaden zu; oder fügt einem diener $1 schaden zu und zieht eine karte.
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int damage = 0;
            if (choice == 1)
            {
                damage = p.getSpellDamageDamage(3, ownplay);
                if (choice == 2)
                {
                    damage = p.getSpellDamageDamage(1, ownplay);
                }

                p.minionGetDamageOrHeal(target, damage);

                if (choice == 2)
                {
                    p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
                }
            }

        }
    }
}