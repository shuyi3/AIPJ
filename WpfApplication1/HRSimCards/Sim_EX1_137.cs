using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_137 : SimTemplate //headcrack
    {

        //    fügt dem feindlichen helden $2 schaden zu. combo:/ lasst die karte in eurem nächsten zug wieder auf eure hand zurückkehren.

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(2, ownplay);

            p.minionGetDamageOrHeal(ownplay ? p.playerSecond.ownHero : p.playerFirst.ownHero, dmg);

            //if (p.playerFirst.cardsPlayedThisTurn >= 1) //p.evaluatePenality -= 5;
        }

    }
}