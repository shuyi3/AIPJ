using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{

public class ParetoMCTSParameters
{

              //THIS IS PTSP
    public static int MACRO_ACTION_LENGTH = 15;
    public static int ROLLOUT_DEPTH = 20;
    public static int NUM_ACTIONS = 6;
    public static double K = Math.Sqrt(2);
    public static bool EXPLORATION_VIEW_ON;
    public static bool PARETO_VIEW_ON;

    /*       //THIS IS LUNA LANDER

    public static int MACRO_ACTION_LENGTH = 5;
    public static int ROLLOUT_DEPTH = 20;
    public static int NUM_ACTIONS = 6;
    public static double K = Math.sqrt(2);
    public static boolean EXPLORATION_VIEW_ON;
    public static boolean PARETO_VIEW_ON;

    /*/

    /** WEIGHTS **/

    public static double[] targetWeights=
                            new double[]{0.45,0.35,0.2};
                            //new double[]{0.5,0.5};
    //                        new double[]{0.1,0.3,0.6};
    //                        new double[]{0.1,0.6,0.3};


                            //new double[]{0.25, 0.75};
                            //new double[]{0.5, 0.5, 0.5};
                            //new double[]{1.0, 0.0, 0.0};
                            //new double[]{0.0,1.0};
                            //new double[]{1.0,0.0};

                            //new double[]{1.0};


    public static int NUM_TARGETS = targetWeights.Length;

}

}
