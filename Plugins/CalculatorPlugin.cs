using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIChatApplication.Plugins;

public class CalculatorPlugin
{

    [KernelFunction("add")]
    [Description("İki sayısal değer üzerinde toplama işlemi gerçekleştirir.")]
    [return:Description("Toplam Değeri Döndürür.")]
    public int Add(int num1, int num2)
        => num1 + num2;

}
