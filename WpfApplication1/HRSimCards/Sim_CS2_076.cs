﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_CS2_076 : SimTemplate//Assassinate
    {

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.minionGetDestroyed(target);
        }

    }
}
