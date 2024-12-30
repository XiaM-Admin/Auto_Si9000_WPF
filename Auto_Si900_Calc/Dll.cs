using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Si900_Calc
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BEMCalcProgressStructure
    {
        /// <summary>
        /// 当前计算步骤
        /// </summary>
        public long nCurrentCalculationStep;

        /// <summary>
        /// 最大计算步骤估计值
        /// </summary>
        public long nCurrentCalculationStepMax;

        /// <summary>
        /// 当前目标寻求迭代次数
        /// </summary>
        public long nCurrentGoalSeekIteration;

        /// <summary>
        /// 最大目标寻求迭代次数
        /// </summary>
        public long nCurrentGoalSeekIterationMax;

        /// <summary>
        /// 先前目标值（目标寻求器）
        /// </summary>
        public double dPrevGoalValue;

        /// <summary>
        /// 先前阻抗值（目标寻求器）
        /// </summary>
        public double dPrevImpedance;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BEMCalcResultStructure
    {
        /// <summary>
        /// 结果是否有效（0 == 无效）
        /// </summary>
        public double dResultValid;

        /// <summary>
        /// 计算的阻抗（单位：欧姆）
        /// </summary>
        public double dImpedance;

        /// <summary>
        /// 计算的时间延迟（单位：皮秒）
        /// </summary>
        public double dDelay;

        /// <summary>
        /// 计算的有效介电常数
        /// </summary>
        public double dErEff;

        /// <summary>
        /// 计算的电感值（单位：nH/m）
        /// </summary>
        public double dInductance;

        /// <summary>
        /// 计算的电容值（单位：pF/m）
        /// </summary>
        public double dCer;

        public override string ToString()
        {
            return $"BEMCalcResultStructure(dResultValid={dResultValid}, dImpedance={dImpedance}, " +
                   $"dDelay={dDelay}, dErEff={dErEff}, dInductance={dInductance}, dCer={dCer})";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BEMErrorStructure
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public long nError;

        /// <summary>
        /// 错误参数1
        /// </summary>
        public long nErrParam1;

        /// <summary>
        /// 错误参数2
        /// </summary>
        public long nErrParam2;

        /// <summary>
        /// 错误参数（用于VB）
        /// </summary>
        public int nErrorParamForVB;

        /// <summary>
        /// 错误参数3
        /// </summary>
        public double dErrParam3;

        /// <summary>
        /// 错误参数4
        /// </summary>
        public double dErrParam4;

        public override string ToString()
        {
            return $"BEMErrorStructure(nError={nError}, nErrParam1={nErrParam1}, nErrParam2={nErrParam2}, " +
                   $"nErrorParamForVB={nErrorParamForVB}, dErrParam3={dErrParam3}, dErrParam4={dErrParam4})";
        }
    }

    public static class Dll
    {
        [DllImport("CalcEngineBEMDll.dll")]
        public static extern long CalcEngineBEMDLLClaimFlexLicence(long licenceType);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern long CalcEngineBEMDLLIsCalculationModelAllowed(long type, long freqDependentCalc);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern long CalcEngineBEMDLLCoatedMicrostrip1B(
            double w2, double w1, double t1, double h1,
            double er1, double c2, double c1, double cer, long reserved);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern long CalcEngineBEMDLLCoatedCoplanarWaveguideWithLowerGnd1B(
            double w2, double w1, double t1, double d1, double h1,
            double er1, double c2, double c1, double cer, long reserved);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern long CalcEngineBEMDLLOffsetStripline1B1A(
            double w2, double w1, double t1, double h1,
            double er1, double h2, double er2, long reserved);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern long CalcEngineBEMDLLOffsetCoplanarWaveguide1B1A(
            double w2, double w1, double t1, double d1,
            double h1, double er1, double h2, double er2, long reserved);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern long CalcEngineBEMDLLDiffEdgeCoupledCoatedMicrostrip1B(
            double w2, double w1, double t1, double s1,
            double h1, double er1, double c2, double c1, double c3, double cer, double mode, long reserved);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern long CalcEngineBEMDLLDiffOffsetStripline1B1A(
              double w2, double w1, double t1, double s1,
              double h1, double er1, double h2, double er2, double mode, long reserved);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern long CalcEngineBEMDLLDiffCoatedCoplanarWaveguideWithLowerGnd1B(
              double w2, double w1, double t1, double s1,
              double d1, double h1, double er1, double c2, double c1, double c3, double cer, double mode, long reserved);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern long CalcEngineBEMDLLDiffOffsetCoplanarWaveguide1B1A(
              double w2, double w1, double t1, double s1,
              double d1, double h1, double er1, double h2, double er2, double mode, long reserved);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern bool CalcEngineBEMDLLQueryCalculationFinished(ref BEMCalcProgressStructure progress);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern bool CalcEngineBEMDLLQueryCalculationResult(ref BEMCalcResultStructure result);

        [DllImport("CalcEngineBEMDll.dll")]
        public static extern long CalcEngineBEMDLLGetErrorAsString(ref BEMErrorStructure error, ref byte errorCode, long maxLength);
    }
}