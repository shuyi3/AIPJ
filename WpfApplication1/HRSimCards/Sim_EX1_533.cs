﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_533 : SimTemplate//Misdirection
    {
        public override void onSecretPlay(Playfield p, bool ownplay, Minion attacker, Minion target, out int number)
        {
            number = 0;
            Minion newTarget = null;
            if (ownplay)
            {
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if (target.entitiyID != m.entitiyID && attacker.entitiyID != m.entitiyID)
                    {
                        newTarget = m;
                    }
                }

                if (newTarget == null)
                {
                    foreach (Minion m in p.playerFirst.ownMinions)
                    {
                        if (target.entitiyID != m.entitiyID && attacker.entitiyID != m.entitiyID)
                        {
                            newTarget = m;
                        }
                    }
                }

                if (newTarget == null)
                {
                    newTarget = p.playerSecond.ownHero;
                }
            }

            else
            {
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if (target.entitiyID != m.entitiyID && attacker.entitiyID != m.entitiyID)
                    {
                        newTarget = m;
                    }
                }

                if (newTarget == null)
                {
                    foreach (Minion m in p.playerSecond.ownMinions)
                    {
                        if (target.entitiyID != m.entitiyID && attacker.entitiyID != m.entitiyID)
                        {
                            newTarget = m;
                        }
                    }
                }

                if (newTarget == null)
                {
                    newTarget = p.playerFirst.ownHero;
                }
            }


            if (newTarget != null)
            {
                number = newTarget.entitiyID;
            }
        }

    }

}
