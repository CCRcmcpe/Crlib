using System;
using System.Linq;
using System.Reflection;
using REVUnit.Crlib.Extension;
using REVUnit.Crlib.Input;

namespace REVUnit.Crlib.ManualTest
{
    public class Program
    {
        public enum 圣经
        {
            我带你们打, 发把狙, A1高闪来一个
        }
        public static void Main()
        {
            Console.WriteLine(X.HappensProbabilityS(0.02f) ? "oh shit" : "kokodayo");
            return;
            var cin = new Cin();
            MethodInfo method = typeof(Cin).GetMethod("Get");
            while (true)
            {
                var type = cin.Get<string>("Enter type");
                object result = method.MakeGenericMethod(XType.GetType(type, true, true))
                    .Invoke(cin, new object[] {null, null});
                Console.WriteLine($"You entered: [{result.GetType().Name}] {result} | hash: {result.GetHashCode()}");
            }
        }
    }
}