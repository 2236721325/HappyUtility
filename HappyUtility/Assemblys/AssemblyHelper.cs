using System.Reflection;

namespace HappyUtility.Assemblys
{
    public static class AssemblyHelper
    {
        public static Assembly[] GetAssemblies(string? startsWith = null)
        {
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                                .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));

            if (startsWith != null)
            {
                return assemblies.Where(e => e.FullName.StartsWith(value: startsWith)).ToArray();
            }
            return assemblies.ToArray();
        }



    }
}
