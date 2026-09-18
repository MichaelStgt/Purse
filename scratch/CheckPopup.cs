using System;
using System.Linq;
using System.Reflection;

class Program
{
    static void Main()
    {
        try
        {
            var assembly = Assembly.LoadFrom(@"C:\Users\micha\.nuget\packages\communitytoolkit.maui\9.0.1\lib\net8.0-windows10.0.19041.0\CommunityToolkit.Maui.dll");
            var popupType = assembly.GetType("CommunityToolkit.Maui.Views.Popup");
            if (popupType == null)
            {
                Console.WriteLine("Popup type not found");
                return;
            }
            var methods = popupType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var m in methods.Where(x => x.Name.Contains("Close")))
            {
                var p = string.Join(", ", m.GetParameters().Select(x => x.ParameterType.Name + " " + x.Name));
                Console.WriteLine($"{m.ReturnType.Name} {m.Name}({p})");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
