using System;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

namespace Utils {
  public class Helpers {
    public static object InstantiateClass(string className) {
      var assembly = Assembly.GetExecutingAssembly();
      var type = assembly.GetTypes().First(t => t.Name == className);
      return Activator.CreateInstance(type);
    }
  }
}