using System.Diagnostics;

namespace Auto_Si900_Calc
{
    /// <summary>
    /// �迹����
    /// </summary>
    public class Calc
    {
        /// <summary>
        /// �迹����ʵ��
        /// </summary>
        private const int LicenceType = 3;

        /// <summary>
        /// ֧�ֵĵ���ģ��
        /// </summary>
        public enum PcbModel
        {
            ��㵥�߲��Ե�,
            ��㵥�߶Ե�,
            �ڲ㵥�߲��Ե�,
            �ڲ㵥�߶Ե�,
            ���˫�߲��Ե�,
            ���˫�߶Ե�,
            �ڲ�˫�߲��Ե�,
            �ڲ�˫�߶Ե�
        };

        /// <summary>
        /// ֧�ֵļ���ģʽ
        /// </summary>
        public enum CalcMode
        {
            ����,
            ����
        };

        /// <summary>
        /// ����ģʽ
        /// 0������ģʽ
        /// 1������ģʽ
        /// </summary>
        public CalcMode Calc_Mode { get; set; } = CalcMode.����;

        /// <summary>
        /// �Ƿ��Ѿ���ʼ��
        /// </summary>
        private static bool isInit = false;

        public Calc(CalcMode calcMode = CalcMode.����)
        {
            Calc_Mode = calcMode;
            if (isInit)
                return;
            Dll.CalcEngineBEMDLLClaimFlexLicence(LicenceType);
            Debug.WriteLine($"��ʼ�����...����ģʽ��{Calc_Mode}");
            isInit = true;
        }

        private void CalcAllowedType(int type)
        {
            int freqDependantCalc = (LicenceType == 1 || LicenceType == 2) ? 0 : 1;
            long allowed = Dll.CalcEngineBEMDLLIsCalculationModelAllowed(type, freqDependantCalc);
            if (allowed == 0)
            {
                Debug.WriteLine("����ģ�Ͳ�����");
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public double Do(PcbModel model, Dictionary<string, double> parameters)
        {
            switch (model)
            {
                case PcbModel.��㵥�߲��Ե�:
                    ��㵥�߲��Ե�(parameters["H1"], parameters["Er1"], parameters["W1"], parameters["T1"]);
                    break;

                case PcbModel.��㵥�߶Ե�:
                    ��㵥�߶Ե�(parameters["H1"], parameters["Er1"], parameters["W1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.�ڲ㵥�߲��Ե�:
                    �ڲ㵥�߲��Ե�(parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["T1"]);
                    break;

                case PcbModel.�ڲ㵥�߶Ե�:
                    �ڲ㵥�߶Ե�(parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.���˫�߲��Ե�:
                    ���˫�߲��Ե�(parameters["H1"], parameters["Er1"], parameters["W1"], parameters["S1"], parameters["T1"]);
                    break;

                case PcbModel.���˫�߶Ե�:
                    ���˫�߶Ե�(parameters["H1"], parameters["Er1"], parameters["W1"], parameters["S1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.�ڲ�˫�߲��Ե�:
                    �ڲ�˫�߲��Ե�(parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["S1"], parameters["T1"]);
                    break;

                case PcbModel.�ڲ�˫�߶Ե�:
                    �ڲ�˫�߶Ե�(parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["S1"], parameters["D1"], parameters["T1"]);
                    break;

                default:
                    Debug.WriteLine("��֧�ֵļ���ģ��");
                    return 0;
            }

            WaitCalcFinished();
            BEMCalcResultStructure results = new BEMCalcResultStructure();
            Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);

            Debug.WriteLine($"[{model.GetName()}] �迹��������{results.dImpedance:F2},mm ������\r\n{parameters.Show()}");
            return Math.Round(results.dImpedance, 2);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="model"></param>
        /// <param name="parameters"></param>
        /// <param name="dImpedance">Ŀ���迹</param>
        /// <returns></returns>
        public (double Width, double Spacing, double Distance) Res_Do(PcbModel model, Dictionary<string, double> parameters, double dImpedance)
        {
            double Line_W = 0.0;
            double Line_S = 0.0;
            double Line_D = 0.0;

            switch (model)
            {
                case PcbModel.��㵥�߲��Ե�:
                    Line_W = ��㵥�߲��Ե�reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["W1"], parameters["T1"]);
                    break;

                case PcbModel.��㵥�߶Ե�:
                    (Line_W, Line_D) = ��㵥�߶Ե�reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["W1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.�ڲ㵥�߲��Ե�:
                    Line_W = �ڲ㵥�߲��Ե�reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["T1"]);
                    break;

                case PcbModel.�ڲ㵥�߶Ե�:
                    (Line_W, Line_D) = �ڲ㵥�߶Ե�reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.���˫�߲��Ե�:
                    (Line_W, Line_S) = ���˫�߲��Ե�reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["W1"], parameters["S1"], parameters["T1"]);
                    break;

                case PcbModel.���˫�߶Ե�:
                    (Line_W, Line_S, Line_D) = ���˫�߶Ե�reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["W1"], parameters["S1"], parameters["D1"], parameters["T1"]);
                    break;

                case PcbModel.�ڲ�˫�߲��Ե�:
                    (Line_W, Line_S) = �ڲ�˫�߲��Ե�reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["S1"], parameters["T1"]);
                    break;

                case PcbModel.�ڲ�˫�߶Ե�:
                    (Line_W, Line_S, Line_D) = �ڲ�˫�߶Ե�reverse(dImpedance, parameters["H1"], parameters["Er1"], parameters["H2"], parameters["Er2"], parameters["W1"], parameters["S1"], parameters["D1"], parameters["T1"]);
                    break;

                default:
                    Debug.WriteLine("��֧�ֵļ���ģ��");
                    return (0, 0, 0);
            }

            Debug.WriteLine($"[����{model.GetName()}] ����������߿��߾ࡢ�Եؾ��룺{(Line_W, Line_S, Line_D)}");
            return (Math.Round(Line_W, 3), Math.Round(Line_S, 3), Math.Round(Line_D, 3));
        }

        private void WaitCalcFinished()
        {
            BEMCalcProgressStructure progress = new BEMCalcProgressStructure();
            while (!Dll.CalcEngineBEMDLLQueryCalculationFinished(ref progress))
            { }
        }

        #region �������ģ��

        private double ��㵥�߲��Ե�(double H1, double Er1, double W1, double T1, double C1 = 0.04, double C2 = 0.012, double CEr = 3.5)
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

                Debug.WriteLine($"�迹����ʧ�ܣ�������� | {result} | {errorStructure}");
                return 0;
            }
            return 1;
        }

        private double ��㵥�߶Ե�(double H1, double Er1, double W1, double D1, double T1, double C1 = 0.04, double C2 = 0.012, double CEr = 3.5)
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

                Debug.WriteLine($"�迹����ʧ�ܣ�������� | {result} | {errorStructure}");
                return 0;
            }
            return 1;
        }

        private double �ڲ㵥�߲��Ե�(double H1, double Er1, double H2, double Er2, double W1, double T1)
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

                Debug.WriteLine($"�迹����ʧ�ܣ�������� | {result} | {errorStructure}");
                return 0;
            }
            return 1;
        }

        private double �ڲ㵥�߶Ե�(double H1, double Er1, double H2, double Er2, double W1, double D1, double T1)
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

                Debug.WriteLine($"�迹����ʧ�ܣ�������� | {result} | {errorStructure}");
                return 0;
            }
            return 1;
        }

        private double ���˫�߲��Ե�(double H1, double Er1, double W1, double S1, double T1, double C1 = 0.04, double C2 = 0.012, double C3 = 0.04, double CEr = 3.5)
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
                3,  // ����ģʽ
                0);

            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"�迹����ʧ�ܣ�������� | {result} | {errorStructure}");
                return 0;
            }

            return 1;
        }

        private double �ڲ�˫�߲��Ե�(double H1, double Er1, double H2, double Er2, double W1, double S1, double T1)
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
                3, // ����ģʽ
                0);

            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"�迹����ʧ�ܣ�������� | {result} | {errorStructure}");
                return 0;
            }

            return 1; // ����ֵ�����ʵ�ʵ��迹�������滻
        }

        private double ���˫�߶Ե�(double H1, double Er1, double W1, double S1, double D1, double T1, double C1 = 0.04, double C2 = 0.012, double C3 = 0.04, double CEr = 3.5)
        {
            // ���� DLL �������м���
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
                3, // ����ģʽ
                0);

            // �������ʧ��
            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"�迹����ʧ�ܣ�������� | {result} | {errorStructure}");
                return 0;
            }

            return 1;
        }

        private double �ڲ�˫�߶Ե�(double H1, double Er1, double H2, double Er2, double W1, double S1, double D1, double T1)
        {
            // ���� DLL ���м���
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
                3, // ����ģʽ
                0);

            // �������Ƿ�ɹ�
            if (result == 0)
            {
                BEMErrorStructure errorStructure = new BEMErrorStructure();
                byte errorCode = 0;
                Dll.CalcEngineBEMDLLGetErrorAsString(ref errorStructure, ref errorCode, 255);

                Debug.WriteLine($"�迹����ʧ�ܣ�������� | {result} | {errorStructure}");
                return 0;
            }

            return 1;
        }

        #endregion �������ģ��

        #region �������ģ��

        /// <summary>
        /// ��㵥�߲��Եط���
        /// </summary>
        /// <param name="Z">Ҫ���迹ֵ</param>
        /// <param name="H1">���ʺ��</param>
        /// <param name="Er1">���ʽ�糣��</param>
        /// <param name="W1">�߿�</param>
        /// <param name="T1">ͭ��</param>
        /// <param name="C1">������ī���</param>
        /// <param name="C2">ͭƤ��ī���</param>
        /// <param name="CEr">��ī��糣��</param>
        /// <returns>����õ����߿�</returns>
        public double ��㵥�߲��Ե�reverse(
            double Z, double H1, double Er1, double W1, double T1,
            double C1 = 0.04, double C2 = 0.012, double CEr = 3.5)
        {
            double currentW = W1;
            double step = W1 * 0.1; // ��ʼ����Ϊ�߿��10%
            int lastDirection = 0; // ��¼��һ�ε�������: 1Ϊ���ӣ�-1Ϊ����
            int directionChanges = 0; // ��¼����ı����

            while (true)
            {
                // ���㵱ǰ�迹ֵ
                double currentZ = ��㵥�߲��Ե�(H1, Er1, currentW, T1, C1, C2, CEr);
                // �������Ƿ�ʧ��
                if (currentZ == 0)
                {
                    Debug.WriteLine("���棺�迹����ʧ�ܣ��������");
                    return currentW;
                }
                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // ����Ƿ�������Χ��
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Debug.WriteLine($"�ҵ������������߿�: {currentW:F4}mm, ��Ӧ�迹ֵ: {currentZ:F2}��");
                    return currentW;
                }

                // ȷ����������
                int currentDirection;
                double newW;
                if (currentZ < Z) // �迹ֵ��С����Ҫ��С�߿�
                {
                    currentDirection = -1;
                    newW = currentW - step;
                }
                else // �迹ֵ������Ҫ�����߿�
                {
                    currentDirection = 1;
                    newW = currentW + step;
                }

                // ����Ƿ�������ı�
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // ÿ�η���ı�ʱ����������
                    Debug.WriteLine($"��С������: {step:F6}mm");
                }

                // ���·����¼
                lastDirection = currentDirection;

                // ����Ƿ񳬳���������
                if (newW < 0.01)
                {
                    Debug.WriteLine($"���棺�߿��Ѵﵽ��С����(0.01mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return 0.01;
                }
                if (newW > 10)
                {
                    Debug.WriteLine($"���棺�߿��Ѵﵽ�������(10mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return 10;
                }

                // ��鲽���Ƿ�̫С
                if (step < 0.0001) // �������С��0.1΢��
                {
                    Debug.WriteLine($"�����Ѿ���С��ֹͣ�����������߿�: {currentW:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return currentW;
                }

                // �����߿�
                currentW = newW;

                // ��ֹ����ѭ��
                if (directionChanges > 10)
                {
                    Console.WriteLine($"��������ı�������ֹ࣬ͣ�����������߿�: {currentW:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return currentW;
                }
            }
        }

        /// <summary>
        /// ��㵥�߶Եط���
        /// </summary>
        /// <param name="Z">Ҫ���迹ֵ</param>
        /// <param name="H1">���ʺ��</param>
        /// <param name="Er1">���ʽ�糣��</param>
        /// <param name="W1">�߿�</param>
        /// <param name="D1">�Եؾ���</param>
        /// <param name="T1">ͭ��</param>
        /// <param name="C1">������ī���</param>
        /// <param name="C2">ͭƤ��ī���</param>
        /// <param name="CEr">��ī��糣��</param>
        /// <returns>����õ����߿�ͶԵؾ���</returns>
        public (double W, double D) ��㵥�߶Ե�reverse(
            double Z, double H1, double Er1, double W1, double D1, double T1,
            double C1 = 0.04, double C2 = 0.012, double CEr = 3.5)
        {
            double currentW = W1;
            double currentD = D1;
            double step = W1 * 0.1; // ��ʼ����Ϊ�߿��10%
            int lastDirection = 0; // ��¼��һ�ε�������: 1Ϊ���ӣ�-1Ϊ����
            int directionChanges = 0; // ��¼����ı����

            while (true)
            {
                // ���㵱ǰ�迹ֵ
                double currentZ = ��㵥�߶Ե�(H1, Er1, currentW, currentD, T1, C1, C2, CEr);
                // �������Ƿ�ʧ��
                if (currentZ == 0)
                {
                    Console.WriteLine("���棺�迹����ʧ�ܣ��������");
                    return (currentW, currentD);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // ����Ƿ�������Χ��
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"�ҵ����������Ĳ���: �߿� {currentW:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentD);
                }

                // ȷ����������
                int currentDirection;
                double newW, newD;
                if (currentZ < Z) // �迹ֵ��С����Ҫ��С�߿�
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    double absChange = Math.Abs(newW - currentW);
                    newD = currentD + absChange / 2.0; // �߿��С���Եؾ�������
                }
                else // �迹ֵ������Ҫ�����߿�
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    double absChange = Math.Abs(newW - currentW);
                    newD = currentD - absChange / 2.0; // �߿����ӣ��Եؾ����С
                }

                // ����Ƿ�������ı�
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // ÿ�η���ı�ʱ����������
                    Console.WriteLine($"��С������: {step:F6}mm");
                }

                // ���·����¼
                lastDirection = currentDirection;

                // ����Ƿ񳬳���������
                if (newW < 0.01 || newD < 0.01)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ��С����(0.01mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentD);
                }
                if (newW > 10 || newD > 10)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ�������(10mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentD);
                }

                // ��鲽���Ƿ�̫С
                if (step < 0.0001) // �������С��0.1΢��
                {
                    Console.WriteLine($"�����Ѿ���С��ֹͣ���������ղ���: �߿� {currentW:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentD);
                }

                // ���²���
                currentW = newW;
                currentD = newD;

                // ��ֹ����ѭ��
                if (directionChanges > 10)
                {
                    Console.WriteLine($"��������ı�������ֹ࣬ͣ���������ղ���: �߿� {currentW:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentD);
                }
            }
        }

        /// <summary>
        /// �ڲ㵥�߲��Եط���
        /// </summary>
        /// <param name="Z">Ҫ���迹ֵ</param>
        /// <param name="H1">���ʺ��</param>
        /// <param name="Er1">���ʽ�糣��</param>
        /// <param name="H2">PP+ͭ��</param>
        /// <param name="Er2">PP��糣��</param>
        /// <param name="W1">�߿�</param>
        /// <param name="T1">ͭ��</param>
        /// <returns>����õ����߿�</returns>
        public double �ڲ㵥�߲��Ե�reverse(
            double Z, double H1, double Er1, double H2, double Er2, double W1, double T1)
        {
            double currentW = W1;
            double step = W1 * 0.1; // ��ʼ����Ϊ�߿��10%
            int lastDirection = 0; // ��¼��һ�ε�������: 1Ϊ���ӣ�-1Ϊ����
            int directionChanges = 0; // ��¼����ı����

            while (true)
            {
                // ���㵱ǰ�迹ֵ
                double currentZ = �ڲ㵥�߲��Ե�(H1, Er1, H2, Er2, currentW, T1);

                // �������Ƿ�ʧ��
                if (currentZ == 0)
                {
                    Console.WriteLine("���棺�迹����ʧ�ܣ��������");
                    return currentW;
                }
                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // ����Ƿ�������Χ��
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"�ҵ������������߿�: {currentW:F4}mm, ��Ӧ�迹ֵ: {currentZ:F2}��");
                    return currentW;
                }

                // ȷ����������
                int currentDirection;
                double newW;
                if (currentZ < Z) // �迹ֵ��С����Ҫ��С�߿�
                {
                    currentDirection = -1;
                    newW = currentW - step;
                }
                else // �迹ֵ������Ҫ�����߿�
                {
                    currentDirection = 1;
                    newW = currentW + step;
                }

                // ����Ƿ�������ı�
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // ÿ�η���ı�ʱ����������
                    Console.WriteLine($"��С������: {step:F6}mm");
                }

                // ���·����¼
                lastDirection = currentDirection;

                // ����Ƿ񳬳���������
                if (newW < 0.01)
                {
                    Console.WriteLine($"���棺�߿��Ѵﵽ��С����(0.01mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return 0.01;
                }
                if (newW > 10)
                {
                    Console.WriteLine($"���棺�߿��Ѵﵽ�������(10mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return 10;
                }

                // ��鲽���Ƿ�̫С
                if (step < 0.0001) // �������С��0.1΢��
                {
                    Console.WriteLine($"�����Ѿ���С��ֹͣ�����������߿�: {currentW:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return currentW;
                }

                // �����߿�
                currentW = newW;

                // ��ֹ����ѭ��
                if (directionChanges > 10)
                {
                    Console.WriteLine($"��������ı�������ֹ࣬ͣ�����������߿�: {currentW:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return currentW;
                }
            }
        }

        /// <summary>
        /// �ڲ㵥�߶Եط���
        /// </summary>
        /// <param name="Z">Ҫ���迹ֵ</param>
        /// <param name="H1">���ʺ��</param>
        /// <param name="Er1">���ʽ�糣��</param>
        /// <param name="H2">PP+ͭ��</param>
        /// <param name="Er2">PP��糣��</param>
        /// <param name="W1">�߿�</param>
        /// <param name="D1">�Եؾ���</param>
        /// <param name="T1">ͭ��</param>
        /// <returns>����õ����߿�ͶԵؾ���</returns>
        public (double Width, double Distance) �ڲ㵥�߶Ե�reverse(
            double Z, double H1, double Er1, double H2, double Er2, double W1, double D1, double T1)
        {
            double currentW = W1;
            double currentD = D1;
            double step = W1 * 0.1; // ��ʼ����Ϊ�߿��10%
            int lastDirection = 0; // ��¼��һ�ε�������: 1Ϊ���ӣ�-1Ϊ����
            int directionChanges = 0; // ��¼����ı����

            while (true)
            {
                // ���㵱ǰ�迹ֵ
                double currentZ = �ڲ㵥�߶Ե�(H1, Er1, H2, Er2, currentW, currentD, T1);

                // �������Ƿ�ʧ��
                if (currentZ == 0)
                {
                    Console.WriteLine("���棺�迹����ʧ�ܣ��������");
                    return (currentW, currentD);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // ����Ƿ�������Χ��
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"�ҵ����������Ĳ���: �߿� {currentW:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentD);
                }

                // ȷ����������
                int currentDirection;
                double newW, newD;
                if (currentZ < Z) // �迹ֵ��С����Ҫ��С�߿�
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    // �����µĶԵؾ��루�߿��С���Եؾ������ӣ�
                    double absChange = Math.Abs(newW - currentW);
                    newD = currentD + absChange / 2.0;
                }
                else // �迹ֵ������Ҫ�����߿�
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    // �����µĶԵؾ��루�߿����ӣ��Եؾ����С��
                    double absChange = Math.Abs(newW - currentW);
                    newD = currentD - absChange / 2.0;
                }

                // ����Ƿ�������ı�
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // ÿ�η���ı�ʱ����������
                    Console.WriteLine($"��С������: {step:F6}mm");
                }

                // ���·����¼
                lastDirection = currentDirection;

                // ����Ƿ񳬳���������
                if (newW < 0.01 || newD < 0.01)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ��С����(0.01mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentD);
                }
                if (newW > 10 || newD > 10)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ�������(10mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentD);
                }

                // ��鲽���Ƿ�̫С
                if (step < 0.0001) // �������С��0.1΢��
                {
                    Console.WriteLine($"�����Ѿ���С��ֹͣ���������ղ���: �߿� {currentW:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentD);
                }

                // ���²���
                currentW = newW;
                currentD = newD;

                // ��ֹ����ѭ��
                if (directionChanges > 10)
                {
                    Console.WriteLine($"��������ı�������ֹ࣬ͣ���������ղ���: �߿� {currentW:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentD);
                }
            }
        }

        /// <summary>
        /// ���˫�߲��Եط���
        /// </summary>
        /// <param name="Z">Ҫ���迹ֵ</param>
        /// <param name="H1">���ʺ��</param>
        /// <param name="Er1">���ʽ�糣��</param>
        /// <param name="W1">�߿�</param>
        /// <param name="S1">�߾�</param>
        /// <param name="T1">ͭ��</param>
        /// <param name="C1">������ī���</param>
        /// <param name="C2">ͭƤ��ī���</param>
        /// <param name="C3">������ī���</param>
        /// <param name="CEr">��ī��糣��</param>
        /// <returns>����õ����߿���߾�</returns>
        public (double Width, double Spacing) ���˫�߲��Ե�reverse(
            double Z, double H1, double Er1, double W1, double S1, double T1,
            double C1 = 0.04, double C2 = 0.012, double C3 = 0.04, double CEr = 3.5)
        {
            double currentW = W1;
            double currentS = S1;
            double step = W1 * 0.1; // ��ʼ����Ϊ�߿��10%
            int lastDirection = 0; // ��¼��һ�ε�������: 1Ϊ���ӣ�-1Ϊ����
            int directionChanges = 0; // ��¼����ı����

            while (true)
            {
                // ���㵱ǰ�迹ֵ
                double currentZ = ���˫�߲��Ե�(H1, Er1, currentW, currentS, T1, C1, C2, C3, CEr);

                // �������Ƿ�ʧ��
                if (currentZ == 0)
                {
                    Console.WriteLine("���棺�迹����ʧ�ܣ��������");
                    return (currentW, currentS);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // ����Ƿ�������Χ��
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"�ҵ����������Ĳ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS);
                }

                // ȷ����������
                int currentDirection;
                double newW, newS;
                if (currentZ < Z) // �迹ֵ��С����Ҫ��С�߿�
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    // �����µ��߾ࣨ�߿��С���߾����ӣ�
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS + absChange;
                }
                else // �迹ֵ������Ҫ�����߿�
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    // �����µ��߾ࣨ�߿����ӣ��߾��С��
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS - absChange;
                }

                // ����Ƿ�������ı�
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // ÿ�η���ı�ʱ����������
                    Console.WriteLine($"��С������: {step:F6}mm");
                }

                // ���·����¼
                lastDirection = currentDirection;

                // ����Ƿ񳬳���������
                if (newW < 0.01 || newS < 0.01)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ��С����(0.01mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS);
                }
                if (newW > 10 || newS > 10)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ�������(10mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS);
                }

                // ��鲽���Ƿ�̫С
                if (step < 0.0001) // �������С��0.1΢��
                {
                    Console.WriteLine($"�����Ѿ���С��ֹͣ���������ղ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS);
                }

                // ���²���
                currentW = newW;
                currentS = newS;

                // ��ֹ����ѭ��
                if (directionChanges > 10)
                {
                    Console.WriteLine($"��������ı�������ֹ࣬ͣ���������ղ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS);
                }
            }
        }

        /// <summary>
        /// �ڲ�˫�߲��Եط���
        /// </summary>
        /// <param name="Z">Ҫ���迹ֵ</param>
        /// <param name="H1">���ʺ��</param>
        /// <param name="Er1">���ʽ�糣��</param>
        /// <param name="H2">PP+ͭ��</param>
        /// <param name="Er2">PP��糣��</param>
        /// <param name="W1">�߿�</param>
        /// <param name="S1">�߾�</param>
        /// <param name="T1">ͭ��</param>
        /// <returns>����õ����߿���߾�</returns>
        public (double Width, double Spacing) �ڲ�˫�߲��Ե�reverse(
            double Z, double H1, double Er1, double H2, double Er2, double W1, double S1, double T1)
        {
            double currentW = W1;
            double currentS = S1;
            double step = W1 * 0.1; // ��ʼ����Ϊ�߿��10%
            int lastDirection = 0; // ��¼��һ�ε�������: 1Ϊ���ӣ�-1Ϊ����
            int directionChanges = 0; // ��¼����ı����

            while (true)
            {
                // ���㵱ǰ�迹ֵ
                double currentZ = �ڲ�˫�߲��Ե�(H1, Er1, H2, Er2, currentW, currentS, T1);

                // �������Ƿ�ʧ��
                if (currentZ == 0)
                {
                    Console.WriteLine("���棺�迹����ʧ�ܣ��������");
                    return (currentW, currentS);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // ����Ƿ�������Χ��
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"�ҵ����������Ĳ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS);
                }

                // ȷ����������
                int currentDirection;
                double newW, newS;
                if (currentZ < Z) // �迹ֵ��С����Ҫ��С�߿�
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    // �����µ��߾ࣨ�߿��С���߾����ӣ�
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS + absChange;
                }
                else // �迹ֵ������Ҫ�����߿�
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    // �����µ��߾ࣨ�߿����ӣ��߾��С��
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS - absChange;
                }

                // ����Ƿ�������ı�
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // ÿ�η���ı�ʱ����������
                    Console.WriteLine($"��С������: {step:F6}mm");
                }

                // ���·����¼
                lastDirection = currentDirection;

                // ����Ƿ񳬳���������
                if (newW < 0.01 || newS < 0.01)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ��С����(0.01mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS);
                }
                if (newW > 10 || newS > 10)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ�������(10mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS);
                }

                // ��鲽���Ƿ�̫С
                if (step < 0.0001) // �������С��0.1΢��
                {
                    Console.WriteLine($"�����Ѿ���С��ֹͣ���������ղ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS);
                }

                // ���²���
                currentW = newW;
                currentS = newS;

                // ��ֹ����ѭ��
                if (directionChanges > 10)
                {
                    Console.WriteLine($"��������ı�������ֹ࣬ͣ���������ղ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS);
                }
            }
        }

        /// <summary>
        /// ���˫�߶Եط���
        /// </summary>
        /// <param name="Z">Ҫ���迹ֵ</param>
        /// <param name="H1">���ʺ��</param>
        /// <param name="Er1">���ʽ�糣��</param>
        /// <param name="W1">�߿�</param>
        /// <param name="S1">�߾�</param>
        /// <param name="D1">�Եؾ���</param>
        /// <param name="T1">ͭ��</param>
        /// <param name="C1">������ī���</param>
        /// <param name="C2">ͭƤ��ī���</param>
        /// <param name="C3">������ī���</param>
        /// <param name="CEr">��ī��糣��</param>
        /// <returns>����õ����߿��߾�ͶԵؾ���</returns>
        public (double Width, double Spacing, double Distance) ���˫�߶Ե�reverse(
            double Z, double H1, double Er1, double W1, double S1, double D1, double T1,
            double C1 = 0.04, double C2 = 0.012, double C3 = 0.04, double CEr = 3.5)
        {
            double currentW = W1;
            double currentS = S1;
            double currentD = D1;
            double step = W1 * 0.1; // ��ʼ����Ϊ�߿��10%
            int lastDirection = 0; // ��¼��һ�ε�������: 1Ϊ���ӣ�-1Ϊ����
            int directionChanges = 0; // ��¼����ı����

            while (true)
            {
                // ���㵱ǰ�迹ֵ
                double currentZ = ���˫�߶Ե�(H1, Er1, currentW, currentS, currentD, T1, C1, C2, C3, CEr);

                // �������Ƿ�ʧ��
                if (currentZ == 0)
                {
                    Console.WriteLine("���棺�迹����ʧ�ܣ��������");
                    return (currentW, currentS, currentD);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // ����Ƿ�������Χ��
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"�ҵ����������Ĳ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS, currentD);
                }

                // ȷ����������
                int currentDirection;
                double newW, newS, newD;
                if (currentZ < Z) // �迹ֵ��С����Ҫ��С�߿�
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS + absChange; // �߾�����
                    newD = currentD + absChange / 2.0; // �Եؾ�������
                }
                else // �迹ֵ������Ҫ�����߿�
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS - absChange; // �߾��С
                    newD = currentD - absChange / 2.0; // �Եؾ����С
                }

                // ����Ƿ�������ı�
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // ÿ�η���ı�ʱ����������
                    Console.WriteLine($"��С������: {step:F6}mm");
                }

                // ���·����¼
                lastDirection = currentDirection;

                // ����Ƿ񳬳���������
                if (newW < 0.01 || newS < 0.01 || newD < 0.01)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ��С����(0.01mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS, currentD);
                }
                if (newW > 10 || newS > 10 || newD > 10)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ�������(10mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS, currentD);
                }

                // ��鲽���Ƿ�̫С
                if (step < 0.0001) // �������С��0.1΢��
                {
                    Console.WriteLine($"�����Ѿ���С��ֹͣ���������ղ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS, currentD);
                }

                // ���²���
                currentW = newW;
                currentS = newS;
                currentD = newD;

                // ��ֹ����ѭ��
                if (directionChanges > 10)
                {
                    Console.WriteLine($"��������ı�������ֹ࣬ͣ���������ղ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS, currentD);
                }
            }
        }

        /// <summary>
        /// �ڲ�˫�߶Եط���
        /// </summary>
        /// <param name="Z">Ҫ���迹ֵ</param>
        /// <param name="H1">���ʺ��</param>
        /// <param name="Er1">���ʽ�糣��</param>
        /// <param name="H2">PP+ͭ��</param>
        /// <param name="Er2">PP��糣��</param>
        /// <param name="W1">�߿�</param>
        /// <param name="S1">�߾�</param>
        /// <param name="D1">�Եؾ���</param>
        /// <param name="T1">ͭ��</param>
        /// <returns>����õ����߿��߾�ͶԵؾ���</returns>
        public (double Width, double Spacing, double Distance) �ڲ�˫�߶Ե�reverse(
            double Z, double H1, double Er1, double H2, double Er2,
            double W1, double S1, double D1, double T1)
        {
            double currentW = W1;
            double currentS = S1;
            double currentD = D1;
            double step = W1 * 0.1; // ��ʼ����Ϊ�߿��10%
            int lastDirection = 0; // ��¼��һ�ε�������: 1Ϊ���ӣ�-1Ϊ����
            int directionChanges = 0; // ��¼����ı����

            while (true)
            {
                // ���㵱ǰ�迹ֵ
                double currentZ = �ڲ�˫�߶Ե�(H1, Er1, H2, Er2, currentW, currentS, currentD, T1);

                // �������Ƿ�ʧ��
                if (currentZ == 0)
                {
                    Console.WriteLine("���棺�迹����ʧ�ܣ��������");
                    return (currentW, currentS, currentD);
                }

                WaitCalcFinished();
                BEMCalcResultStructure results = new BEMCalcResultStructure();
                Dll.CalcEngineBEMDLLQueryCalculationResult(ref results);
                currentZ = results.dImpedance;

                // ����Ƿ�������Χ��
                if (Math.Abs(currentZ - Z) <= 0.3)
                {
                    Console.WriteLine($"�ҵ����������Ĳ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS, currentD);
                }

                // ȷ����������
                int currentDirection;
                double newW, newS, newD;
                if (currentZ < Z) // �迹ֵ��С����Ҫ��С�߿�
                {
                    currentDirection = -1;
                    newW = currentW - step;
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS + absChange; // �߾�����
                    newD = currentD + absChange / 2.0; // �Եؾ�������
                }
                else // �迹ֵ������Ҫ�����߿�
                {
                    currentDirection = 1;
                    newW = currentW + step;
                    double absChange = Math.Abs(newW - currentW);
                    newS = currentS - absChange; // �߾��С
                    newD = currentD - absChange / 2.0; // �Եؾ����С
                }

                // ����Ƿ�������ı�
                if (lastDirection != 0 && currentDirection != lastDirection)
                {
                    directionChanges++;
                    step *= 0.5; // ÿ�η���ı�ʱ����������
                    Console.WriteLine($"��С������: {step:F6}mm");
                }

                // ���·����¼
                lastDirection = currentDirection;

                // ����Ƿ񳬳���������
                if (newW < 0.01 || newS < 0.01 || newD < 0.01)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ��С����(0.01mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS, currentD);
                }
                if (newW > 10 || newS > 10 || newD > 10)
                {
                    Console.WriteLine($"���棺�ߴ��Ѵﵽ�������(10mm)����ǰ�迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS, currentD);
                }

                // ��鲽���Ƿ�̫С
                if (step < 0.0001) // �������С��0.1΢��
                {
                    Console.WriteLine($"�����Ѿ���С��ֹͣ���������ղ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS, currentD);
                }

                // ���²���
                currentW = newW;
                currentS = newS;
                currentD = newD;

                // ��ֹ����ѭ��
                if (directionChanges > 10)
                {
                    Console.WriteLine($"��������ı�������ֹ࣬ͣ���������ղ���: �߿� {currentW:F4}mm, �߾� {currentS:F4}mm, �Եؾ��� {currentD:F4}mm, �迹ֵ: {currentZ:F2}��");
                    return (currentW, currentS, currentD);
                }
            }
        }

        #endregion �������ģ��
    }
}