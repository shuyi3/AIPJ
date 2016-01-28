using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{

public class Solution{

    public int m_card;
    public double[] m_data;
    private String m_pattern = "[%.3f,%.3f,%.3f]:%d";
    public int m_through = -1;
    public Playfield m_state;

    public Solution(double[] a_data, Playfield state)
    {
        m_card = a_data.Length;
        m_data = a_data;
        m_state = state;
    }

    public String toString()
    {
        //if(m_card == 3)
        return String.Format(m_pattern, m_data[0] , m_data[1] , m_data[2], m_through);
        //return ""
    }

    public Solution copy()
    {
        double []newArray = new double[m_card];
        Array.Copy(m_data, 0, newArray, 0, m_card);
        Solution s = new Solution(newArray, this.m_state);
        s.m_through = this.m_through;
        return s;
    }

}
}
