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
    }
}
