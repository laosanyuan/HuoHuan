using System;
using System.Reflection;

namespace HuoHuan.Test.Utils
{
    internal static class TestUtil
    {
        internal static object InvokeNonPublicMethod(Type type, string methodName, object instance, object?[]? parameters)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            MethodInfo method = type.GetMethod(methodName, flags)!;
            if (method == null)
            {
                throw new Exception("反射函数不存在");
            }
            return method.Invoke(instance, parameters)!;
        }


        internal static object GetStaticInstance(Type type ,string name)
        {
            return "";
        }
    }
}
