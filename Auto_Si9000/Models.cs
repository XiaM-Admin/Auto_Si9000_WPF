using Auto_Si900_Calc;

namespace Auto_Si9000
{
    public class 内层单线不对地_Model
    {
        public double H1 { get; set; }
        public double Er1 { get; set; }
        public double H2 { get; set; }
        public double Er2 { get; set; }
        public double W1 { get; set; }
        public double T1 { get; set; }
        public double Zo { get; set; } = 0;
        public double TargetZo { get; set; } = 0;

        /// <summary>
        /// 计算阻抗
        /// </summary>
        /// <returns></returns>
        public void Calculate_Zo()
        {
            Zo = new Calc().Do(Calc.PcbModel.内层单线不对地, ModelConfig.内层单线不对地(H1, Er1, H2, Er2, W1, T1));
        }

        /// <summary>
        /// 检查TargetZo是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_TargetZo()
        {
            if (TargetZo == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 检查所有参数是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_Valid()
        {
            if (H1 == 0 || Er1 == 0 || H2 == 0 || Er2 == 0 || W1 == 0 || T1 == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 反算阻抗
        /// </summary>
        public void ResCalculate_Zo()
        {
            (W1, _, _) = new Calc().Res_Do(Calc.PcbModel.内层单线不对地, ModelConfig.内层单线不对地(H1, Er1, H2, Er2, W1, T1), TargetZo);
        }
    }

    public class 内层单线对地_Model
    {
        public double H1 { get; set; }
        public double Er1 { get; set; }
        public double H2 { get; set; }
        public double Er2 { get; set; }
        public double W1 { get; set; }
        public double D1 { get; set; }
        public double T1 { get; set; }
        public double Zo { get; set; } = 0;
        public double TargetZo { get; set; } = 0;

        /// <summary>
        /// 计算阻抗
        /// </summary>
        /// <returns></returns>
        public void Calculate_Zo()
        {
            Zo = new Calc().Do(Calc.PcbModel.内层单线对地, ModelConfig.内层单线对地(H1, Er1, H2, Er2, W1, D1, T1));
        }

        /// <summary>
        /// 检查TargetZo是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_TargetZo()
        {
            if (TargetZo == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 检查所有参数是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_Valid()
        {
            if (H1 == 0 || Er1 == 0 || H2 == 0 || Er2 == 0 || D1 == 0 || W1 == 0 || T1 == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 反算阻抗
        /// </summary>
        public void ResCalculate_Zo()
        {
            (W1, _, D1) = new Calc().Res_Do(Calc.PcbModel.内层单线对地, ModelConfig.内层单线对地(H1, Er1, H2, Er2, W1, D1, T1), TargetZo);
        }
    }

    public class 内层双线不对地_Model
    {
        public double H1 { get; set; }
        public double Er1 { get; set; }
        public double H2 { get; set; }
        public double Er2 { get; set; }
        public double W1 { get; set; }
        public double S1 { get; set; }
        public double T1 { get; set; }
        public double Zo { get; set; } = 0;
        public double TargetZo { get; set; } = 0;

        /// <summary>
        /// 计算阻抗
        /// </summary>
        /// <returns></returns>
        public void Calculate_Zo()
        {
            Zo = new Calc().Do(Calc.PcbModel.内层双线不对地, ModelConfig.内层双线不对地(H1, Er1, H2, Er2, W1, S1, T1));
        }

        /// <summary>
        /// 检查TargetZo是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_TargetZo()
        {
            if (TargetZo == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 检查所有参数是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_Valid()
        {
            if (H1 == 0 || Er1 == 0 || H2 == 0 || Er2 == 0 || W1 == 0 || S1 == 0 || T1 == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 反算阻抗
        /// </summary>
        public void ResCalculate_Zo()
        {
            (W1, S1, _) = new Calc().Res_Do(Calc.PcbModel.内层双线不对地, ModelConfig.内层双线不对地(H1, Er1, H2, Er2, W1, S1, T1), TargetZo);
        }
    }

    public class 内层双线对地_Model
    {
        public double H1 { get; set; }
        public double Er1 { get; set; }
        public double H2 { get; set; }
        public double Er2 { get; set; }
        public double W1 { get; set; }
        public double S1 { get; set; }
        public double D1 { get; set; }
        public double T1 { get; set; }
        public double Zo { get; set; } = 0;

        public double TargetZo { get; set; } = 0;

        /// <summary>
        /// 计算阻抗
        /// </summary>
        /// <returns></returns>
        public void Calculate_Zo()
        {
            Zo = new Calc().Do(Calc.PcbModel.内层双线对地, ModelConfig.内层双线对地(H1, Er1, H2, Er2, W1, S1, D1, T1));
        }

        /// <summary>
        /// 检查TargetZo是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_TargetZo()
        {
            if (TargetZo == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 检查所有参数是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_Valid()
        {
            if (H1 == 0 || Er1 == 0 || H2 == 0 || Er2 == 0 || W1 == 0 || S1 == 0 || D1 == 0 || T1 == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 反算阻抗
        /// </summary>
        public void ResCalculate_Zo()
        {
            (W1, S1, D1) = new Calc().Res_Do(Calc.PcbModel.内层双线对地, ModelConfig.内层双线对地(H1, Er1, H2, Er2, W1, S1, D1, T1), TargetZo);
        }
    }

    public class 外层单线不对地_Model
    {
        public double H1 { get; set; }
        public double Er1 { get; set; }
        public double W1 { get; set; }
        public double T1 { get; set; }
        public double C1 { get; set; } = 0.04;
        public double C2 { get; set; } = 0.012;
        public double CEr { get; set; } = 3.5;
        public double Zo { get; set; } = 0;
        public double TargetZo { get; set; } = 0;

        /// <summary>
        /// 计算阻抗
        /// </summary>
        /// <returns></returns>
        public void Calculate_Zo()
        {
            Zo = new Calc().Do(Calc.PcbModel.外层单线不对地, ModelConfig.外层单线不对地(H1, Er1, W1, T1, C1, C2, CEr));
        }

        /// <summary>
        /// 检查TargetZo是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_TargetZo()
        {
            if (TargetZo == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 检查所有参数是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_Valid()
        {
            if (H1 == 0 || Er1 == 0 || W1 == 0 || T1 == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 反算阻抗
        /// </summary>
        public void ResCalculate_Zo()
        {
            (W1, _, _) = new Calc().Res_Do(Calc.PcbModel.外层单线不对地, ModelConfig.外层单线不对地(H1, Er1, W1, T1, C1, C2, CEr), TargetZo);
        }
    }

    public class 外层单线对地_Model
    {
        public double H1 { get; set; }
        public double Er1 { get; set; }
        public double W1 { get; set; }
        public double D1 { get; set; }
        public double T1 { get; set; }
        public double C1 { get; set; } = 0.04;
        public double C2 { get; set; } = 0.012;
        public double CEr { get; set; } = 3.5;
        public double Zo { get; set; } = 0;
        public double TargetZo { get; set; } = 0;

        /// <summary>
        /// 计算阻抗
        /// </summary>
        /// <returns></returns>
        public void Calculate_Zo()
        {
            Zo = new Calc().Do(Calc.PcbModel.外层单线对地, ModelConfig.外层单线对地(H1, Er1, W1, D1, T1, C1, C2, CEr));
        }

        /// <summary>
        /// 检查TargetZo是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_TargetZo()
        {
            if (TargetZo == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 检查所有参数是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_Valid()
        {
            if (H1 == 0 || Er1 == 0 || D1 == 0 || W1 == 0 || T1 == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 反算阻抗
        /// </summary>
        public void ResCalculate_Zo()
        {
            (W1, _, D1) = new Calc().Res_Do(Calc.PcbModel.外层单线对地, ModelConfig.外层单线对地(H1, Er1, W1, D1, T1, C1, C2, CEr), TargetZo);
        }
    }

    public class 外层双线不对地_Model
    {
        public double H1 { get; set; }
        public double Er1 { get; set; }
        public double W1 { get; set; }
        public double S1 { get; set; }
        public double T1 { get; set; }
        public double C1 { get; set; } = 0.04;
        public double C2 { get; set; } = 0.012;
        public double C3 { get; set; } = 0.04;
        public double CEr { get; set; } = 3.5;
        public double Zo { get; set; } = 0;
        public double TargetZo { get; set; } = 0;

        /// <summary>
        /// 计算阻抗
        /// </summary>
        /// <returns></returns>
        public void Calculate_Zo()
        {
            Zo = new Calc().Do(Calc.PcbModel.外层双线不对地, ModelConfig.外层双线不对地(H1, Er1, W1, S1, T1, C1, C2, C3, CEr));
        }

        /// <summary>
        /// 检查TargetZo是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_TargetZo()
        {
            if (TargetZo == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 检查所有参数是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_Valid()
        {
            if (H1 == 0 || Er1 == 0 || W1 == 0 || S1 == 0 || T1 == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 反算阻抗
        /// </summary>
        public void ResCalculate_Zo()
        {
            (W1, S1, _) = new Calc().Res_Do(Calc.PcbModel.外层双线不对地, ModelConfig.外层双线不对地(H1, Er1, W1, S1, T1, C1, C2, C3, CEr), TargetZo);
        }
    }

    public class 外层双线对地_Model
    {
        public double H1 { get; set; }
        public double Er1 { get; set; }
        public double W1 { get; set; }
        public double S1 { get; set; }
        public double D1 { get; set; }
        public double T1 { get; set; }
        public double C1 { get; set; } = 0.04;
        public double C2 { get; set; } = 0.012;
        public double C3 { get; set; } = 0.04;
        public double CEr { get; set; } = 3.5;
        public double Zo { get; set; } = 0;
        public double TargetZo { get; set; } = 0;

        /// <summary>
        /// 计算阻抗
        /// </summary>
        /// <returns></returns>
        public void Calculate_Zo()
        {
            Zo = new Calc().Do(Calc.PcbModel.外层双线对地, ModelConfig.外层双线对地(H1, Er1, W1, S1, D1, T1, C1, C2, C3, CEr));
        }

        /// <summary>
        /// 检查TargetZo是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_TargetZo()
        {
            if (TargetZo == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 检查所有参数是否有效
        /// </summary>
        /// <returns></returns>
        public bool Check_Valid()
        {
            if (H1 == 0 || Er1 == 0 || W1 == 0 || S1 == 0 || D1 == 0 || T1 == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 反算阻抗
        /// </summary>
        public void ResCalculate_Zo()
        {
            (W1, S1, D1) = new Calc().Res_Do(Calc.PcbModel.外层双线对地, ModelConfig.外层双线对地(H1, Er1, W1, S1, D1, T1, C1, C2, C3, CEr), TargetZo);
        }
    }
}