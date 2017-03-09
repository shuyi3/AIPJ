using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using Newtonsoft.Json;

namespace HRSim
{
    public static class IOUtils
    {
        public static void SaveState(Playfield state, string fileName)
        {
            //PlayerKeyInfo ownkeyInfo, enemykeyInfo;
            //if (state.isOwnTurn)
            //{
            //    ownkeyInfo = new PlayerKeyInfo(state.playerFirst, state.homeDeck, true);
            //    enemykeyInfo = new PlayerKeyInfo(state.playerSecond, state.awayDeck, false);
            //}
            //else
            //{
            //    ownkeyInfo = new PlayerKeyInfo(state.playerSecond, state.awayDeck, true);
            //    enemykeyInfo = new PlayerKeyInfo(state.playerFirst, state.homeDeck, false);
            //}
            //StateKeyInfo stateInfo = new StateKeyInfo(ownkeyInfo, enemykeyInfo);
            //Helpfunctions.Instance.WriteResultToFile(fileName, JsonConvert.SerializeObject(stateInfo));
        }
    }


    public static class PythonUtils
    {
        public static void AppendRecycle(PyList list, PyObject obj)
        {
            list.Append(obj);
            obj.Dispose();
        }
    }

    public static class MathUtils
    {
        public static void SoftMax(List<double> probList)
        {
            if (probList.Count == 0) return;

            double cMax = Double.MinValue;
            foreach (double v in probList)
            {
                cMax = Math.Max(v, cMax);
            }

            double[] expArray = new double[probList.Count];
            for (int i = 0; i < probList.Count; i++)
            {
                probList[i] = Math.Exp(probList[i] - cMax);
            }

            double expSum = 0.0;
            foreach (double v in probList)
            {
                expSum += v;
            }

            for (int i = 0; i < probList.Count; i++)
            {
                probList[i] /= expSum;
            }
        }
    }
}
