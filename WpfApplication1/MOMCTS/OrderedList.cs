using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{

public class OrderedList
{
    public List<Double> m_members;

    public OrderedList()
    {
        m_members = new List<Double>();
    }

    public void clear() {m_members.Clear();}
    public int size() {return m_members.Count;}
    public Double get(int index) {return m_members[index];}
    public void remove(int index) {m_members.RemoveAt(index);}

    public void add(Double a_element)
    {
        //Additions are (asc) ordered by the value of the first element of the array.
        bool here = false;
        int i = 0;
        while (!here && i < m_members.Count)
        {
            Double m = m_members[i];
            
            if(a_element < m)
            {
                here = true;
                m_members.Insert(i, a_element);
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
