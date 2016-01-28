using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{
public class OrderedSolutionList
{
    public List<Solution> m_members;

    public OrderedSolutionList()
    {
        m_members = new List<Solution>();
    }

    public void clear() {m_members.Clear();}
    public int size() {return m_members.Count;}
    public Solution get(int index) {return m_members[index];}
    public void remove(int index) {m_members.RemoveAt(index);}

    public void add(Solution a_element)
    {
        //Additions are (asc) ordered by the value of the first element of the array.
        bool here = false;
        int i = 0;
        while(!here && i < m_members.Count)
        {
            Solution m = m_members[i];

            if(a_element.m_data[0] < m.m_data[0])
            {
                here = true;
                m_members.Insert(i,a_element);
            }
            ++i;
        }

        if(!here)
        {   //add at the end:
            m_members.Add(a_element);
        }

    }
    
    
}

}
