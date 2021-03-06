﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
    class Sim_EX1_258 : SimTemplate//Unbound Elemental
    {
        // erhält jedes Mal +1/+1, wenn Ihr eine Karte mit uberladung&lt; ausspielt.
        public override void onCardIsGoingToBePlayed(Playfield p, CardDB.Card c, bool wasOwnCard, Minion triggerEffectMinion)
        {
            if (wasOwnCard == triggerEffectMinion.own && c.Recall)
            {
                p.minionGetBuffed(triggerEffectMinion, 1, 1);
            }
        }

    }
}
