using System.Diagnostics;

namespace Auto_Si900_Calc
{
    /// <summary>
    /// 阻抗计算
    /// </summary>
    public class Calc
    {
        /// <summary>
        /// 阻抗计算实例
        /// </summary>
        private const int LicenceType = 3;

        /// <summary>
        /// 支持的叠构模型
        /// </summary>
        public enum PcbModel
        {
            外层单线不对地,
            外层单线对地,
            内层单线不对地,
            内层单线对地,
            外层双线不对地,
            外层双线对地,
            内层双线不对地,
            内层双线对地
        };

        /// <summary>
        /// 支持的计算模式
        /// </summary>
        public enum CalcMode
        {
            反算,
            正算
        };

        /// <summary>
        /// 计算模式
        /// 0：反算模式
        /// 1：正算模式
        /// </summary>
        public CalcMode Calc_Mode { get; set; } = CalcMode.正算;

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private static bool isInit = false;

        public Calc(CalcMode calcMode = CalcMode.正算)
        {
            Calc_Mode = calcMode;
            if (isInit)
                return;
            Dll.CalcEngineBEMDLLClaimFlexLicence(LicenceType);
            Debug.WriteLine($"初始化完成...计算模式：{Calc_Mode}");
            isInit = true;
        }

        private void CalcAllowedType(int type)
        {
            int freqDependantCalc = (LicenceType == 1 || LicenceType == 2) ? 0 : 1;
            long allowed = Dll.CalcEngineBEMDLLIsCalculationModelAllowed(type, freqDependantCalc);
            if (allowed == 0)
            {
                Debug.WriteLine("计算模型不允许");
            }
        }

        /// <summary>
        /// 正向计算
        /// </summary>
        /// <returns></returns>
        public double Do(PcbModel model, Dictionary<string, double> parameters)
        {
            switch (model)
            {
                case PcbModel.外层单线不对地:
                    外层单线不对地(parameters["H1"], parameters["Er1"], parameters["W1"], parameters["T1"]);
                    break;

                case PcbModel.外层单线对地:
                    外层单线对地(parameters["H1"], parameters["Er1"], parameters["W1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.内层单线不对地:
                    内层单线不对地(parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["T1"]);
                    break;

                case PcbModel.内层单线对地:
                    内层单线对地(parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.外层双线不对地:
                    外层双线不对地(parameters["H1"], parameters["Er1"], parameters["W1"], parameters["S1"], parameters["T1"]);
                    break;

                case PcbModel.外层双线对地:
                    外层双线对地(parameters["H1"], parameters["Er1"], parameters["W1"], parameters["S1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.内层双线不对地:
                    内层双线不对地(parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["S1"], parameters["T1"]);
                    break;

                case PcbModel.内层双线对地:
                    内层双线对地(parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["S1"], parameters["D1"], parameters["T1"]);
                    break;

                default:
                    Debug.WriteLine("不支持的计算模型");
                    return 0;
            }

            WaitCalcFinished();
            BEMCalcResultStructure results = new BEMCalcResultStructure();
            Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);

            Debug.WriteLine($"[{model.GetName()}] 阻抗计算结果：{results.dImpedance:F2},mm 参数：\r\n{parameters.Show()}");
            return Math.Round(results.dImpedance, 2);
        }

        /// <summary>
        /// 反向计算
        /// </summary>
        /// <param name="model"></param>
        /// <param name="parameters"></param>
        /// <param name="dImpedance">目标阻抗</param>
        /// <returns></returns>
        public (double Width, double Spacing, double Distance) Res_Do(PcbModel model, Dictionary<string, double> parameters, double dImpedance)
        {
            double Line_W = 0.0;
            double Line_S = 0.0;
            double Line_D = 0.0;

            switch (model)
            {
                case PcbModel.外层单线不对地:
                    Line_W = 外层单线不对地reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["W1"], parameters["T1"]);
                    break;

                case PcbModel.外层单线对地:
                    (Line_W, Line_D) = 外层单线对地reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["W1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.内层单线不对地:
                    Line_W = 内层单线不对地reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["T1"]);
                    break;

                case PcbModel.内层单线对地:
                    (Line_W, Line_D) = 内层单线对地reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.外层双线不对地:
                    (Line_W, Line_S) = 外层双线不对地reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["W1"], parameters["S1"], parameters["T1"]);
                    break;

                case PcbModel.外层双线对地:
                    (Line_W, Line_S, Line_D) = 外层双线对地reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["W1"], parameters["S1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.内层双线不对地:
                    (Line_W, Line_S) = 内层双线不对地reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["S1"], parameters["T1"]);
                    break;

                case PcbModel.内层双线对地:
                    (Line_W, Line_S, Line_D) = 内层双线对地reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["S1"], parameters["D1"], parameters["T1"]);
                    break;

                default:
                    Debug.WriteLine("不支持的计算模型");
                    return (0, 0, 0);
            }

            Debug.WriteLine($"[反算{model.GetName()}] 反向计算结果线宽、线距、对地距离：{(Line_W, Line_S, Line_D)}");
            return (Math.Round(Line_W, 3), Math.Round(Line_S, 3), Math.Round(Line_D, 3));
        }

        private void WaitCalcFinished()
        {
            BEMCalcProgressStructure progress = new BEMCalcProgressStructure();
            while (!Dll.CalcEngineBEMDLLQueryCalculationFinished(ref progress))
            { }
        }

        #region 正向计算模型

        private double 外层单线不对地(double H1, double Er1, double W1, double T1, double C1 = 0.04, double C2 = 0.012, double CEr = 3.5)
        {
            long result = Dll.CalcEngineBEMDLLCoatedMicrostrip1B(
                (W1 / 0.0254) - 1.0,
                W1 / 0.0254,
                T1 / 0.0254,
                H1 / 0.0254,
                Er1,
                C2 / 0.0254,
                C1 / 0.0254,
                CEr,
                0);

            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"阻抗计算失败，请检查参数 | {result} | {errorStructure}");
                return 0;
            }
            return 1;
        }

        private double 外层单线对地(double H1, double Er1, double W1, double D1, double T1, double C1 = 0.04, double C2 = 0.012, double CEr = 3.5)
        {
            long result = Dll.CalcEngineBEMDLLCoatedCoplanarWaveguideWithLowerGnd1B(
                (W1 / 0.0254) - 1.0,
                W1 / 0.0254,
                T1 / 0.0254,
                D1 / 0.0254,
                H1 / 0.0254,
                Er1,
                C2 / 0.0254,
                C1 / 0.0254,
                CEr,
                0);

            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"阻抗计算失败，请检查参数 | {result} | {errorStructure}");
                return 0;
            }
            return 1;
        }

        private double 内层单线不对地(double H1, double Er1, double H2, double Er2, double W1, double T1)
        {
            long result = Dll.CalcEngineBEMDLLOffsetStripline1B1A(
                (W1 / 0.0254) - 1.0,
                W1 / 0.0254,
                T1 / 0.0254,
                H1 / 0.0254,
                Er1,
                H2 / 0.0254,
                Er2,
                0);

            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"阻抗计算失败，请检查参数 | {result} | {errorStructure}");
                return 0;
            }
            return 1;
        }

        private double 内层单线对地(double H1, double Er1, double H2, double Er2, double W1, double D1, double T1)
        {
            long result = Dll.CalcEngineBEMDLLOffsetCoplanarWaveguide1B1A(
                (W1 / 0.0254) - 1.0,
                W1 / 0.0254,
                T1 / 0.0254,
                D1 / 0.0254,
                H1 / 0.0254,
                Er1,
                H2 / 0.0254,
                Er2,
                0);

            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"阻抗计算失败，请检查参数 | {result} | {errorStructure}");
                return 0;
            }
            return 1;
        }

        private double 外层双线不对地(double H1, double Er1, double W1, double S1, double T1, double C1 = 0.04, double C2 = 0.012, double C3 = 0.04, double CEr = 3.5)
        {
            long result = Dll.CalcEngineBEMDLLDiffEdgeCoupledCoatedMicrostrip1B(
                (W1 / 0.0254) - 1.0,
                W1 / 0.0254,
                T1 / 0.0254,
                S1 / 0.0254,
                H1 / 0.0254,
                Er1,
                C2 / 0.0254,
                C1 / 0.0254,
                C3 / 0.0254,
                CEr,
                3,  // 计算模式
                0);

            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"阻抗计算失败，请检查参数 | {result} | {errorStructure}");
                return 0;
            }

            return 1;
        }

        private double 内层双线不对地(double H1, double Er1, double H2, double Er2, double W1, double S1, double T1)
        {
            long result = Dll.CalcEngineBEMDLLDiffOffsetStripline1B1A(
                (W1 / 0.0254) - 1.0,
                W1 / 0.0254,
                T1 / 0.0254,
                S1 / 0.0254,
                H1 / 0.0254,
                Er1,
                H2 / 0.0254,
                Er2,
                3, // 计算模式
                0);

            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"阻抗计算失败，请检查参数 | {result} | {errorStructure}");
                return 0;
            }

            return 1; // 返回值需根据实际的阻抗计算结果替换
        }

        private double 外层双线对地(double H1, double Er1, double W1, double S1, double D1, double T1, double C1 = 0.04, double C2 = 0.012, double C3 = 0.04, double CEr = 3.5)
        {
            // 调用 DLL 函数进行计算
            long result = Dll.CalcEngineBEMDLLDiffCoatedCoplanarWaveguideWithLowerGnd1B(
                (W1 / 0.0254) - 1.0,
                W1 / 0.0254,
                T1 / 0.0254,
                S1 / 0.0254,
                D1 / 0.0254,
                H1 / 0.0254,
                Er1,
                C2 / 0.0254,
                C1 / 0.0254,
                C3 / 0.0254,
                CEr,
                3, // 计算模式
                0);

            // 如果计算失败
            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"阻抗计算失败，请检查参数 | {result} | {errorStructure}");
                return 0;
            }

            return 1;
        }

        private double 内层双线对地(double H1, double Er1, double H2, double Er2, double W1, double S1, double D1, double T1)
        {
            // 调用 DLL 进行计算
            long result = Dll.CalcEngineBEMDLLDiffOffsetCoplanarWaveguide1B1A(
                (W1 / 0.0254) - 1.0,
                W1 / 0.0254,
                T1 / 0.0254,
                S1 / 0.0254,
                D1 / 0.0254,
                H1 / 0.0254,
                Er1,
                H2 / 0.0254,
                Er2,
                3, // 计算模式
                0);

            // 检查计算是否成功
            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"阻抗计算失败，请检查参数 | {result} | {errorStructure}");
                return 0;
            }

            return 1;
        }

        #endregion 正向计算模型

        #region 反向计算模型

        /// <summary>
        /// 外层单线不对地反算
        /// </summary>
        /// <param name="Z">要求阻抗值</param>
        /// <param name="H1">介质厚度</param>
        /// <param name="Er1">介质介电常数</param>
        /// <param name="W1">线宽</param>
        /// <param name="T1">铜厚</param>
        /// <param name="C1">基材油墨厚度</param>
        /// <param name="C2">铜皮油墨厚度</param>
        /// <param name="CEr">油墨介电常数</param>
        /// <returns>计算得到的线宽</returns>
        public double 外层单线不对地reverse(
            double Z, double H1, double Er1, double W1, double T1,
            double C1 = 0.04, double C2 = 0.012, double CEr = 3.5)
        {
            double currentW = W1;
            double step = W1 * 0.1; // 初始步长为线宽的10%
            int lastDirection = 0; // 记录上一次调整方向: 1为增加，-1为减少
            int directionChanges = 0; // 记录方向改变次数

            while (true)
            {
                // 计算当前阻抗值
                double currentZ = 外层单线不对地(H1, Er1, currentW, T1, C1, C2, CEr);
                // 检查计算是否失败
                if (currentZ == 0)
                {
                    Debug.WriteLine("警告：阻抗计算失败，请检查参数");
                    return currentW;
                }
                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // 检查是否在允许范围内
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Debug.WriteLine($"找到满足条件的线宽: {currentW:F4}mm, 对应阻抗值: {currentZ:F2}Ω");
                    return currentW;
                }

                // 确定调整方向
                int currentDirection;
                double newW;
                if (currentZ < Z) // 阻抗值过小，需要减小线宽
                {
                    currentDirection = -1;
                    newW = currentW - step;
                }
                else // 阻抗值过大，需要增加线宽
                {
                    currentDirection = 1;
                    newW = currentW + step;
                }

                // 检查是否发生方向改变
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // 每次方向改变时，步长减半
                    Debug.WriteLine($"减小步长至: {step:F6}mm");
                }

                // 更新方向记录
                lastDirection = currentDirection;

                // 检查是否超出物理限制
                if (newW < 0.01)
                {
                    Debug.WriteLine($"警告：线宽已达到最小限制(0.01mm)，当前阻抗值: {currentZ:F2}Ω");
                    return 0.01;
                }
                if (newW > 10)
                {
                    Debug.WriteLine($"警告：线宽已达到最大限制(10mm)，当前阻抗值: {currentZ:F2}Ω");
                    return 10;
                }

                // 检查步长是否太小
                if (step < 0.0001) // 如果步长小于0.1微米
                {
                    Debug.WriteLine($"步长已经很小，停止调整。最终线宽: {currentW:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return currentW;
                }

                // 更新线宽
                currentW = newW;

                // 防止无限循环
                if (directionChanges > 10)
                {
                    Console.WriteLine($"调整方向改变次数过多，停止调整。最终线宽: {currentW:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return currentW;
                }
            }
        }

        /// <summary>
        /// 外层单线对地反算
        /// </summary>
        /// <param name="Z">要求阻抗值</param>
        /// <param name="H1">介质厚度</param>
        /// <param name="Er1">介质介电常数</param>
        /// <param name="W1">线宽</param>
        /// <param name="D1">对地距离</param>
        /// <param name="T1">铜厚</param>
        /// <param name="C1">基材油墨厚度</param>
        /// <param name="C2">铜皮油墨厚度</param>
        /// <param name="CEr">油墨介电常数</param>
        /// <returns>计算得到的线宽和对地距离</returns>
        public (double W, double D) 外层单线对地reverse(
            double Z, double H1, double Er1, double W1, double D1, double T1,
            double C1 = 0.04, double C2 = 0.012, double CEr = 3.5)
        {
            double currentW = W1;
            double currentD = D1;
            double step = W1 * 0.1; // 初始步长为线宽的10%
            int lastDirection = 0; // 记录上一次调整方向: 1为增加，-1为减少
            int directionChanges = 0; // 记录方向改变次数

            while (true)
            {
                // 计算当前阻抗值
                double currentZ = 外层单线对地(H1, Er1, currentW, currentD, T1, C1, C2, CEr);
                // 检查计算是否失败
                if (currentZ == 0)
                {
                    Console.WriteLine("警告：阻抗计算失败，请检查参数");
                    return (currentW, currentD);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // 检查是否在允许范围内
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"找到满足条件的参数: 线宽 {currentW:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentD);
                }

                // 确定调整方向
                int currentDirection;
                double newW, newD;
                if (currentZ < Z) // 阻抗值过小，需要减小线宽
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    double absChange = Math.Abs(newW - currentW);
                    newD = currentD + absChange / 2.0; // 线宽减小，对地距离增加
                }
                else // 阻抗值过大，需要增加线宽
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    double absChange = Math.Abs(newW - currentW);
                    newD = currentD - absChange / 2.0; // 线宽增加，对地距离减小
                }

                // 检查是否发生方向改变
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // 每次方向改变时，步长减半
                    Console.WriteLine($"减小步长至: {step:F6}mm");
                }

                // 更新方向记录
                lastDirection = currentDirection;

                // 检查是否超出物理限制
                if (newW < 0.01 || newD < 0.01)
                {
                    Console.WriteLine($"警告：尺寸已达到最小限制(0.01mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentD);
                }
                if (newW > 10 || newD > 10)
                {
                    Console.WriteLine($"警告：尺寸已达到最大限制(10mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentD);
                }

                // 检查步长是否太小
                if (step < 0.0001) // 如果步长小于0.1微米
                {
                    Console.WriteLine($"步长已经很小，停止调整。最终参数: 线宽 {currentW:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentD);
                }

                // 更新参数
                currentW = newW;
                currentD = newD;

                // 防止无限循环
                if (directionChanges > 10)
                {
                    Console.WriteLine($"调整方向改变次数过多，停止调整。最终参数: 线宽 {currentW:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentD);
                }
            }
        }

        /// <summary>
        /// 内层单线不对地反算
        /// </summary>
        /// <param name="Z">要求阻抗值</param>
        /// <param name="H1">介质厚度</param>
        /// <param name="Er1">介质介电常数</param>
        /// <param name="H2">PP+铜厚</param>
        /// <param name="Er2">PP介电常数</param>
        /// <param name="W1">线宽</param>
        /// <param name="T1">铜厚</param>
        /// <returns>计算得到的线宽</returns>
        public double 内层单线不对地reverse(
            double Z, double H1, double Er1, double H2, double Er2, double W1, double T1)
        {
            double currentW = W1;
            double step = W1 * 0.1; // 初始步长为线宽的10%
            int lastDirection = 0; // 记录上一次调整方向: 1为增加，-1为减少
            int directionChanges = 0; // 记录方向改变次数

            while (true)
            {
                // 计算当前阻抗值
                double currentZ = 内层单线不对地(H1, Er1, H2, Er2, currentW, T1);

                // 检查计算是否失败
                if (currentZ == 0)
                {
                    Console.WriteLine("警告：阻抗计算失败，请检查参数");
                    return currentW;
                }
                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // 检查是否在允许范围内
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"找到满足条件的线宽: {currentW:F4}mm, 对应阻抗值: {currentZ:F2}Ω");
                    return currentW;
                }

                // 确定调整方向
                int currentDirection;
                double newW;
                if (currentZ < Z) // 阻抗值过小，需要减小线宽
                {
                    currentDirection = -1;
                    newW = currentW - step;
                }
                else // 阻抗值过大，需要增加线宽
                {
                    currentDirection = 1;
                    newW = currentW + step;
                }

                // 检查是否发生方向改变
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // 每次方向改变时，步长减半
                    Console.WriteLine($"减小步长至: {step:F6}mm");
                }

                // 更新方向记录
                lastDirection = currentDirection;

                // 检查是否超出物理限制
                if (newW < 0.01)
                {
                    Console.WriteLine($"警告：线宽已达到最小限制(0.01mm)，当前阻抗值: {currentZ:F2}Ω");
                    return 0.01;
                }
                if (newW > 10)
                {
                    Console.WriteLine($"警告：线宽已达到最大限制(10mm)，当前阻抗值: {currentZ:F2}Ω");
                    return 10;
                }

                // 检查步长是否太小
                if (step < 0.0001) // 如果步长小于0.1微米
                {
                    Console.WriteLine($"步长已经很小，停止调整。最终线宽: {currentW:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return currentW;
                }

                // 更新线宽
                currentW = newW;

                // 防止无限循环
                if (directionChanges > 10)
                {
                    Console.WriteLine($"调整方向改变次数过多，停止调整。最终线宽: {currentW:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return currentW;
                }
            }
        }

        /// <summary>
        /// 内层单线对地反算
        /// </summary>
        /// <param name="Z">要求阻抗值</param>
        /// <param name="H1">介质厚度</param>
        /// <param name="Er1">介质介电常数</param>
        /// <param name="H2">PP+铜厚</param>
        /// <param name="Er2">PP介电常数</param>
        /// <param name="W1">线宽</param>
        /// <param name="D1">对地距离</param>
        /// <param name="T1">铜厚</param>
        /// <returns>计算得到的线宽和对地距离</returns>
        public (double Width, double Distance) 内层单线对地reverse(
            double Z, double H1, double Er1, double H2, double Er2, double W1, double D1, double T1)
        {
            double currentW = W1;
            double currentD = D1;
            double step = W1 * 0.1; // 初始步长为线宽的10%
            int lastDirection = 0; // 记录上一次调整方向: 1为增加，-1为减少
            int directionChanges = 0; // 记录方向改变次数

            while (true)
            {
                // 计算当前阻抗值
                double currentZ = 内层单线对地(H1, Er1, H2, Er2, currentW, currentD, T1);

                // 检查计算是否失败
                if (currentZ == 0)
                {
                    Console.WriteLine("警告：阻抗计算失败，请检查参数");
                    return (currentW, currentD);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // 检查是否在允许范围内
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"找到满足条件的参数: 线宽 {currentW:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentD);
                }

                // 确定调整方向
                int currentDirection;
                double newW, newD;
                if (currentZ < Z) // 阻抗值过小，需要减小线宽
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    // 计算新的对地距离（线宽减小，对地距离增加）
                    double absChange = Math.Abs(newW - currentW);
                    newD = currentD + absChange / 2.0;
                }
                else // 阻抗值过大，需要增加线宽
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    // 计算新的对地距离（线宽增加，对地距离减小）
                    double absChange = Math.Abs(newW - currentW);
                    newD = currentD - absChange / 2.0;
                }

                // 检查是否发生方向改变
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // 每次方向改变时，步长减半
                    Console.WriteLine($"减小步长至: {step:F6}mm");
                }

                // 更新方向记录
                lastDirection = currentDirection;

                // 检查是否超出物理限制
                if (newW < 0.01 || newD < 0.01)
                {
                    Console.WriteLine($"警告：尺寸已达到最小限制(0.01mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentD);
                }
                if (newW > 10 || newD > 10)
                {
                    Console.WriteLine($"警告：尺寸已达到最大限制(10mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentD);
                }

                // 检查步长是否太小
                if (step < 0.0001) // 如果步长小于0.1微米
                {
                    Console.WriteLine($"步长已经很小，停止调整。最终参数: 线宽 {currentW:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentD);
                }

                // 更新参数
                currentW = newW;
                currentD = newD;

                // 防止无限循环
                if (directionChanges > 10)
                {
                    Console.WriteLine($"调整方向改变次数过多，停止调整。最终参数: 线宽 {currentW:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentD);
                }
            }
        }

        /// <summary>
        /// 外层双线不对地反算
        /// </summary>
        /// <param name="Z">要求阻抗值</param>
        /// <param name="H1">介质厚度</param>
        /// <param name="Er1">介质介电常数</param>
        /// <param name="W1">线宽</param>
        /// <param name="S1">线距</param>
        /// <param name="T1">铜厚</param>
        /// <param name="C1">基材油墨厚度</param>
        /// <param name="C2">铜皮油墨厚度</param>
        /// <param name="C3">基材油墨厚度</param>
        /// <param name="CEr">油墨介电常数</param>
        /// <returns>计算得到的线宽和线距</returns>
        public (double Width, double Spacing) 外层双线不对地reverse(
            double Z, double H1, double Er1, double W1, double S1, double T1,
            double C1 = 0.04, double C2 = 0.012, double C3 = 0.04, double CEr = 3.5)
        {
            double currentW = W1;
            double currentS = S1;
            double step = W1 * 0.1; // 初始步长为线宽的10%
            int lastDirection = 0; // 记录上一次调整方向: 1为增加，-1为减少
            int directionChanges = 0; // 记录方向改变次数

            while (true)
            {
                // 计算当前阻抗值
                double currentZ = 外层双线不对地(H1, Er1, currentW, currentS, T1, C1, C2, C3, CEr);

                // 检查计算是否失败
                if (currentZ == 0)
                {
                    Console.WriteLine("警告：阻抗计算失败，请检查参数");
                    return (currentW, currentS);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // 检查是否在允许范围内
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"找到满足条件的参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS);
                }

                // 确定调整方向
                int currentDirection;
                double newW, newS;
                if (currentZ < Z) // 阻抗值过小，需要减小线宽
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    // 计算新的线距（线宽减小，线距增加）
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS + absChange;
                }
                else // 阻抗值过大，需要增加线宽
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    // 计算新的线距（线宽增加，线距减小）
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS - absChange;
                }

                // 检查是否发生方向改变
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // 每次方向改变时，步长减半
                    Console.WriteLine($"减小步长至: {step:F6}mm");
                }

                // 更新方向记录
                lastDirection = currentDirection;

                // 检查是否超出物理限制
                if (newW < 0.01 || newS < 0.01)
                {
                    Console.WriteLine($"警告：尺寸已达到最小限制(0.01mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS);
                }
                if (newW > 10 || newS > 10)
                {
                    Console.WriteLine($"警告：尺寸已达到最大限制(10mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS);
                }

                // 检查步长是否太小
                if (step < 0.0001) // 如果步长小于0.1微米
                {
                    Console.WriteLine($"步长已经很小，停止调整。最终参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS);
                }

                // 更新参数
                currentW = newW;
                currentS = newS;

                // 防止无限循环
                if (directionChanges > 10)
                {
                    Console.WriteLine($"调整方向改变次数过多，停止调整。最终参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS);
                }
            }
        }

        /// <summary>
        /// 内层双线不对地反算
        /// </summary>
        /// <param name="Z">要求阻抗值</param>
        /// <param name="H1">介质厚度</param>
        /// <param name="Er1">介质介电常数</param>
        /// <param name="H2">PP+铜厚</param>
        /// <param name="Er2">PP介电常数</param>
        /// <param name="W1">线宽</param>
        /// <param name="S1">线距</param>
        /// <param name="T1">铜厚</param>
        /// <returns>计算得到的线宽和线距</returns>
        public (double Width, double Spacing) 内层双线不对地reverse(
            double Z, double H1, double Er1, double H2, double Er2, double W1, double S1, double T1)
        {
            double currentW = W1;
            double currentS = S1;
            double step = W1 * 0.1; // 初始步长为线宽的10%
            int lastDirection = 0; // 记录上一次调整方向: 1为增加，-1为减少
            int directionChanges = 0; // 记录方向改变次数

            while (true)
            {
                // 计算当前阻抗值
                double currentZ = 内层双线不对地(H1, Er1, H2, Er2, currentW, currentS, T1);

                // 检查计算是否失败
                if (currentZ == 0)
                {
                    Console.WriteLine("警告：阻抗计算失败，请检查参数");
                    return (currentW, currentS);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // 检查是否在允许范围内
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"找到满足条件的参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS);
                }

                // 确定调整方向
                int currentDirection;
                double newW, newS;
                if (currentZ < Z) // 阻抗值过小，需要减小线宽
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    // 计算新的线距（线宽减小，线距增加）
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS + absChange;
                }
                else // 阻抗值过大，需要增加线宽
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    // 计算新的线距（线宽增加，线距减小）
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS - absChange;
                }

                // 检查是否发生方向改变
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // 每次方向改变时，步长减半
                    Console.WriteLine($"减小步长至: {step:F6}mm");
                }

                // 更新方向记录
                lastDirection = currentDirection;

                // 检查是否超出物理限制
                if (newW < 0.01 || newS < 0.01)
                {
                    Console.WriteLine($"警告：尺寸已达到最小限制(0.01mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS);
                }
                if (newW > 10 || newS > 10)
                {
                    Console.WriteLine($"警告：尺寸已达到最大限制(10mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS);
                }

                // 检查步长是否太小
                if (step < 0.0001) // 如果步长小于0.1微米
                {
                    Console.WriteLine($"步长已经很小，停止调整。最终参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS);
                }

                // 更新参数
                currentW = newW;
                currentS = newS;

                // 防止无限循环
                if (directionChanges > 10)
                {
                    Console.WriteLine($"调整方向改变次数过多，停止调整。最终参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS);
                }
            }
        }

        /// <summary>
        /// 外层双线对地反算
        /// </summary>
        /// <param name="Z">要求阻抗值</param>
        /// <param name="H1">介质厚度</param>
        /// <param name="Er1">介质介电常数</param>
        /// <param name="W1">线宽</param>
        /// <param name="S1">线距</param>
        /// <param name="D1">对地距离</param>
        /// <param name="T1">铜厚</param>
        /// <param name="C1">基材油墨厚度</param>
        /// <param name="C2">铜皮油墨厚度</param>
        /// <param name="C3">基材油墨厚度</param>
        /// <param name="CEr">油墨介电常数</param>
        /// <returns>计算得到的线宽、线距和对地距离</returns>
        public (double Width, double Spacing, double Distance) 外层双线对地reverse(
            double Z, double H1, double Er1, double W1, double S1, double D1, double T1,
            double C1 = 0.04, double C2 = 0.012, double C3 = 0.04, double CEr = 3.5)
        {
            double currentW = W1;
            double currentS = S1;
            double currentD = D1;
            double step = W1 * 0.1; // 初始步长为线宽的10%
            int lastDirection = 0; // 记录上一次调整方向: 1为增加，-1为减少
            int directionChanges = 0; // 记录方向改变次数

            while (true)
            {
                // 计算当前阻抗值
                double currentZ = 外层双线对地(H1, Er1, currentW, currentS, currentD, T1, C1, C2, C3, CEr);

                // 检查计算是否失败
                if (currentZ == 0)
                {
                    Console.WriteLine("警告：阻抗计算失败，请检查参数");
                    return (currentW, currentS, currentD);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // 检查是否在允许范围内
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"找到满足条件的参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS, currentD);
                }

                // 确定调整方向
                int currentDirection;
                double newW, newS, newD;
                if (currentZ < Z) // 阻抗值过小，需要减小线宽
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS + absChange; // 线距增加
                    newD = currentD + absChange / 2.0; // 对地距离增加
                }
                else // 阻抗值过大，需要增加线宽
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS - absChange; // 线距减小
                    newD = currentD - absChange / 2.0; // 对地距离减小
                }

                // 检查是否发生方向改变
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // 每次方向改变时，步长减半
                    Console.WriteLine($"减小步长至: {step:F6}mm");
                }

                // 更新方向记录
                lastDirection = currentDirection;

                // 检查是否超出物理限制
                if (newW < 0.01 || newS < 0.01 || newD < 0.01)
                {
                    Console.WriteLine($"警告：尺寸已达到最小限制(0.01mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS, currentD);
                }
                if (newW > 10 || newS > 10 || newD > 10)
                {
                    Console.WriteLine($"警告：尺寸已达到最大限制(10mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS, currentD);
                }

                // 检查步长是否太小
                if (step < 0.0001) // 如果步长小于0.1微米
                {
                    Console.WriteLine($"步长已经很小，停止调整。最终参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS, currentD);
                }

                // 更新参数
                currentW = newW;
                currentS = newS;
                currentD = newD;

                // 防止无限循环
                if (directionChanges > 10)
                {
                    Console.WriteLine($"调整方向改变次数过多，停止调整。最终参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS, currentD);
                }
            }
        }

        /// <summary>
        /// 内层双线对地反算
        /// </summary>
        /// <param name="Z">要求阻抗值</param>
        /// <param name="H1">介质厚度</param>
        /// <param name="Er1">介质介电常数</param>
        /// <param name="H2">PP+铜厚</param>
        /// <param name="Er2">PP介电常数</param>
        /// <param name="W1">线宽</param>
        /// <param name="S1">线距</param>
        /// <param name="D1">对地距离</param>
        /// <param name="T1">铜厚</param>
        /// <returns>计算得到的线宽、线距和对地距离</returns>
        public (double Width, double Spacing, double Distance) 内层双线对地reverse(
            double Z, double H1, double Er1, double H2, double Er2,
            double W1, double S1, double D1, double T1)
        {
            double currentW = W1;
            double currentS = S1;
            double currentD = D1;
            double step = W1 * 0.1; // 初始步长为线宽的10%
            int lastDirection = 0; // 记录上一次调整方向: 1为增加，-1为减少
            int directionChanges = 0; // 记录方向改变次数

            while (true)
            {
                // 计算当前阻抗值
                double currentZ = 内层双线对地(H1, Er1, H2, Er2, currentW, currentS, currentD, T1);

                // 检查计算是否失败
                if (currentZ == 0)
                {
                    Console.WriteLine("警告：阻抗计算失败，请检查参数");
                    return (currentW, currentS, currentD);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // 检查是否在允许范围内
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"找到满足条件的参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS, currentD);
                }

                // 确定调整方向
                int currentDirection;
                double newW, newS, newD;
                if (currentZ < Z) // 阻抗值过小，需要减小线宽
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS + absChange; // 线距增加
                    newD = currentD + absChange / 2.0; // 对地距离增加
                }
                else // 阻抗值过大，需要增加线宽
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS - absChange; // 线距减小
                    newD = currentD - absChange / 2.0; // 对地距离减小
                }

                // 检查是否发生方向改变
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // 每次方向改变时，步长减半
                    Console.WriteLine($"减小步长至: {step:F6}mm");
                }

                // 更新方向记录
                lastDirection = currentDirection;

                // 检查是否超出物理限制
                if (newW < 0.01 || newS < 0.01 || newD < 0.01)
                {
                    Console.WriteLine($"警告：尺寸已达到最小限制(0.01mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS, currentD);
                }
                if (newW > 10 || newS > 10 || newD > 10)
                {
                    Console.WriteLine($"警告：尺寸已达到最大限制(10mm)，当前阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS, currentD);
                }

                // 检查步长是否太小
                if (step < 0.0001) // 如果步长小于0.1微米
                {
                    Console.WriteLine($"步长已经很小，停止调整。最终参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS, currentD);
                }

                // 更新参数
                currentW = newW;
                currentS = newS;
                currentD = newD;

                // 防止无限循环
                if (directionChanges > 10)
                {
                    Console.WriteLine($"调整方向改变次数过多，停止调整。最终参数: 线宽 {currentW:F4}mm, 线距 {currentS:F4}mm, 对地距离 {currentD:F4}mm, 阻抗值: {currentZ:F2}Ω");
                    return (currentW, currentS, currentD);
                }
            }
        }

        #endregion 反向计算模型
    }
}