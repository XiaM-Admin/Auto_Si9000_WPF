using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Si900_Calc
{
    /// <summary>
    /// 模型参数配置
    /// </summary>
    public static class ModelConfig
    {
        public static Dictionary<string, double> 外层单线不对地(double H1, double Er1, double W1, double T1, double C1 = 0.04, double C2 = 0.012, double Cer = 3.5)
            => new Dictionary<string, double>()
            {
                {"H1", H1},
                {"Er1", Er1},
                {"W1", W1},
                {"T1", T1},
                {"C1", C1},
                {"C2", C2},
                {"Cer", Cer}
            };

        public static Dictionary<string, double> 外层单线对地(double H1, double Er1, double W1, double D1, double T1, double C1 = 0.04, double C2 = 0.012, double Cer = 3.5)
            => new Dictionary<string, double>()
            {
                {"H1", H1},
                {"Er1", Er1},
                {"W1", W1},
                {"D1", D1},
                {"T1", T1},
                {"C1", C1},
                {"C2", C2},
                {"Cer", Cer}
            };

        public static Dictionary<string, double> 内层单线不对地(double H1, double Er1, double H2, double Er2, double W1, double T1)
            => new Dictionary<string, double>()
            {
                {"H1", H1},
                {"Er1", Er1},
                {"H2", H2},
                {"Er2", Er2},
                {"W1", W1},
                {"T1", T1},
            };

        public static Dictionary<string, double> 内层单线对地(double H1, double Er1, double H2, double Er2, double W1, double D1, double T1)
            => new Dictionary<string, double>()
            {
                {"H1", H1},
                {"Er1", Er1},
                {"H2", H2},
                {"Er2", Er2},
                {"W1", W1},
                {"D1", D1},
                {"T1", T1},
            };

        public static Dictionary<string, double> 外层双线不对地(double H1, double Er1, double W1, double S1, double T1, double C1 = 0.04, double C2 = 0.012, double C3 = 0.04, double Cer = 3.5)
            => new Dictionary<string, double>()
            {
                {"H1", H1},
                {"Er1", Er1},
                {"W1", W1},
                {"S1", S1},
                {"T1", T1},
                {"C1", C1},
                {"C2", C2},
                {"C3", C3},
                {"Cer", Cer}
            };

        public static Dictionary<string, double> 外层双线对地(double H1, double Er1, double W1, double S1, double D1, double T1, double C1 = 0.04, double C2 = 0.012, double C3 = 0.04, double Cer = 3.5)
            => new Dictionary<string, double>()
            {
                {"H1", H1},
                {"Er1", Er1},
                {"W1", W1},
                {"S1", S1},
                {"D1", D1},
                {"T1", T1},
                {"C1", C1},
                {"C2", C2},
                {"C3", C3},
                {"Cer", Cer}
            };

        public static Dictionary<string, double> 内层双线不对地(double H1, double Er1, double H2, double Er2, double W1, double S1, double T1)
            => new Dictionary<string, double>()
            {
                {"H1", H1},
                {"Er1", Er1},
                {"H2", H2},
                {"Er2", Er2},
                {"W1", W1},
                {"S1", S1},
                {"T1", T1},
            };

        public static Dictionary<string, double> 内层双线对地(double H1, double Er1, double H2, double Er2, double W1, double S1, double D1, double T1)
            => new Dictionary<string, double>()
            {
                { "H1", H1},
                { "Er1", Er1},
                { "H2", H2},
                { "Er2", Er2},
                { "W1", W1},
                { "S1", S1},
                { "D1", D1},
                { "T1", T1},
            };
    }
}