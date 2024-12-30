using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Si900_Calc
{
    public static class Extensions
    {
        /// <summary>
        /// 扩展方法，打印字典中的所有键值对到控制台
        /// </summary>
        /// <param name="dictionary">字典对象</param>
        public static string Show(this Dictionary<string, double> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                //Console.WriteLine("字典为空或未初始化。");
                //Debug.WriteLine("字典为空或未初始化。");
                return "";
            }
            string str = "";
            foreach (var kvp in dictionary)
            {
                //Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value:F4}");
                //Debug.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value:F4}");
                str += $"Key: {kvp.Key}, Value: {kvp.Value:F4}\n";
            }
            return str;
        }

        /// <summary>
        /// 获取枚举值的名称
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns>枚举的名称</returns>
        public static string GetName(this Enum enumValue)
        {
            return enumValue.ToString();
        }
    }
}