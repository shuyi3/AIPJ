using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{

public class RandomRoller
{
    public static int RANDOM_ROLLOUT = 0;

    //Rollout type.
    private int m_rolloutType;
    
    private Random m_r;

    private int m_numActions;

    public RandomRoller(int a_rT, Random a_r, int a_numActions)
    {
        m_rolloutType = a_rT;
        m_r = a_r;
        m_numActions = a_numActions;
    }

    public int roll(Playfield a_gameState)
    {
        if(this.m_rolloutType == RANDOM_ROLLOUT)
            return m_r.Next(m_numActions);
        else throw new Exception("Unknown rollout mode: " + this.m_rolloutType);
    }
    
}

}
