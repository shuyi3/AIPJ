using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using Newtonsoft.Json;

namespace HRSim
{

    public static class PythonUtils
    {
        public static void AppendRecycle(PyList list, PyObject obj)
        {
            list.Append(obj);
            obj.Dispose();
        }

        public static PyList CleanTuple(int a, int b)
        {
            PyList ret = new PyList();
            AppendRecycle(ret, new PyInt(a));
            AppendRecycle(ret, new PyInt(b));
            return ret;
        }

        public static PyList CleanTriple(int a, int b, int c)
        {
            PyList ret = new PyList();
            AppendRecycle(ret, new PyInt(a));
            AppendRecycle(ret, new PyInt(b));
            AppendRecycle(ret, new PyInt(c));
            return ret;
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

        public static void SoftMaxCummulate(double[] probArr)
        {
            if (probArr.Length == 0) return;

            double cMax = Double.MinValue;
            foreach (double v in probArr)
            {
                cMax = Math.Max(v, cMax);
            }

            double[] expArray = new double[probArr.Length];
            for (int i = 0; i < probArr.Length; i++)
            {
                probArr[i] = Math.Exp(probArr[i] - cMax);
            }

            double expSum = 0.0;
            foreach (double v in probArr)
            {
                expSum += v;
            }

            for (int i = 0; i < probArr.Length; i++)
            {
                if (i == probArr.Length - 1)
                {
                    probArr[i] = 1.0;
                }
                else if (i == 0)
                {
                    probArr[i] /= expSum;
                }
                else
                {
                    probArr[i] = probArr[i] / expSum + probArr[i - 1];
                }
            }

        }

        public static void SoftMax(List<Handmanager.Handcard> cardList)
        {
            if (cardList.Count == 0) return;

            double cMax = Double.MinValue;
            foreach (Handmanager.Handcard hc in cardList)
            {
                cMax = Math.Max(hc.playProb, cMax);
            }

            double[] expArray = new double[cardList.Count];
            foreach (Handmanager.Handcard hc in cardList)
            {
                hc.playProb = Math.Exp(hc.playProb - cMax);
            }

            double expSum = 0.0;
            foreach (Handmanager.Handcard hc in cardList)
            {
                expSum += hc.playProb;
            }

            for (int i = 0; i < cardList.Count; i++)
            {
                //if (i == cardList.Count - 1)
                //{
                //    cardList[i].playProb = 1.0;
                //}
                //else if (i == 0)
                //{
                //    cardList[i].playProb /= expSum;
                //}
                //else
                //{
                //    cardList[i].playProb = cardList[i].playProb / expSum + cardList[i - 1].playProb;
                //}
                cardList[i].playProb /= expSum;
            }
        }
    }
}
