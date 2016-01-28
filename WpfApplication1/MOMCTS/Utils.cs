using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{

public class Utils
{
    //Normalizes a value between its MIN and MAX.
    public static double normalise(double a_value, double a_min, double a_max)
    {
        return (a_value - a_min)/(a_max - a_min);
    }

    public static double distanceEuq(double[] a, double[] b)
    {
        double acum = 0.0;
        for(int i = 0; i < a.Length; ++i)
        {
            double diffSq = (a[i]-b[i])*(a[i]-b[i]);
            acum += diffSq;
        }
        return Math.Sqrt(acum);
    }


    public static bool crowded(double[] a, double[] b, double epsilon)
    {
        double distance = distanceEuq(a,b);
        return distance < epsilon;
    }

    /**
     * @param a_one first element to compare.
     * @param a_two second element to compare.
     * @return -1: one dominates two, 0: no dominance, 1: two dominates one . 2: they are equal.
     */
    public static int dominates(double[] a_one, double[] a_two)
    {
        int numTargets = a_one.Length;
        if (numTargets != a_two.Length)
            throw new Exception("Dominance check failed: number of objectives mismatch! " + a_one.Length + " != " + a_two.Length);

        int dominance = 0;
        bool equal = false;
        for(int i = 0; i < numTargets; ++i)
        {
            if(a_one[i] > a_two[i])
            {
                if(dominance == 1)
                    return 0;  //There is one objective where two > one. There can't be dominance:

                dominance = -1;
                equal = false;

            }else if(a_one[i] < a_two[i]){

                if(dominance == -1)
                    return 0; //There is one objective where one > two. There can't be dominance:

                dominance = 1;
                equal = false;
            }else if(dominance == 0)
            {
                //They are equal:
                equal = true;
            }
        }

        if(equal)
            return 2;
        else
            return dominance;
    }
}

}
