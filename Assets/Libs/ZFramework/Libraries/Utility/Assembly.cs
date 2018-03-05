﻿using System;
using System.Collections.Generic;

namespace ZFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// 程序集相关的实用函数。
        /// </summary>
        public static class Assembly
        {
            private static readonly Dictionary<string, Type> s_CachedTypes = new Dictionary<string, Type>();
            private static readonly List<string> s_LoadedAssemblyNames = new List<string>();
            private static readonly System.Reflection.Assembly[] s_Assemblies = null;

            static Assembly()
            {
                s_Assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (System.Reflection.Assembly assembly in s_Assemblies)
                {
                    s_LoadedAssemblyNames.Add(assembly.FullName);
                }
            }

            /// <summary>
            /// 获取已加载的程序集名称。
            /// </summary>
            /// <returns>已加载的程序集名称。</returns>
            public static string[] GetLoadedAssemblyNames()
            {
                return s_LoadedAssemblyNames.ToArray();
            }

            /// <summary>
            /// 获取已加载的程序集。
            /// </summary>
            /// <returns>已加载的程序集。</returns>
            public static System.Reflection.Assembly[] GetAssemblies()
            {
                return s_Assemblies;
            }

            /// <summary>
            /// 获取已加载的程序集中的所有类型。
            /// </summary>
            /// <returns>已加载的程序集中的所有类型。</returns>
            public static Type[] GetTypes()
            {
                List<Type> allTypes = new List<Type>();
                for (int i = 0; i < s_Assemblies.Length; i++)
                {
                    allTypes.AddRange(s_Assemblies[i].GetTypes());
                }

                return allTypes.ToArray();
            }

            /// <summary>
            /// 从已加载的程序集中获取类型。
            /// </summary>
            /// <param name="typeName">要获取的类型名。</param>
            /// <returns>获取的类型。</returns>
            public static Type GetTypeWithinLoadedAssemblies(string typeName)
            {
                if (string.IsNullOrEmpty(typeName))
                {
                    throw new Exception("Type name is invalid.");
                }

                Type type = null;
                if (s_CachedTypes.TryGetValue(typeName, out type))
                {
                    return type;
                }

                type = Type.GetType(typeName);
                if (type != null)
                {
                    s_CachedTypes.Add(typeName, type);
                    return type;
                }

                foreach (string assemblyName in s_LoadedAssemblyNames)
                {
                    type = Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
                    if (type != null)
                    {
                        s_CachedTypes.Add(typeName, type);
                        return type;
                    }
                }

                return null;
            }
        }
    }
}