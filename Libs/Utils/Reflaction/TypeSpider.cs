using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace BlackJack.Utils
{
	public sealed class TypeSpider
	{
		/// <summary>
		/// Finds all types match predicater in solution.
		/// </summary>
		/// <returns>The matched types in solution.</returns>
		/// <param name="predicater">Predicater.</param>
		public static IEnumerable<Type> FindTypesInSolution(Func<Type, bool> predicater, String nameInclude="")
		{
			// 现在怀疑Assembly::GetExportedTypes会返回两个相同的Type，基于这一点考虑，使用Set过滤返回Type. -- 周翀
			ISet<string> typesReturned = new HashSet<string>();

			ISet<string> assembliesVisited = new HashSet<string>();
			Queue<Assembly> assembliesToVisit = new Queue<Assembly>();

			// Start with the .exe assembly.
			Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) yield break;
			assembliesToVisit.Enqueue(entryAssembly);

			// BFS (Breadth-first search)
			while (assembliesToVisit.Count > 0)
			{
				Assembly assemblyVisiting = assembliesToVisit.Dequeue();
				assembliesVisited.Add(assemblyVisiting.FullName);

				// Enqueue referenced assemblies.
				foreach (AssemblyName an in assemblyVisiting.GetReferencedAssemblies())
				{
                    //排除掉名字不符合要求的AssemblyName
                    if (!String.IsNullOrEmpty(nameInclude) && nameInclude.IndexOf(an.Name, 0) == -1) continue;
					
					//Debug.WriteLine("Visiting={0}", a.FullName);
                    if (!assembliesVisited.Contains(an.FullName))
                    {
                        Assembly a = Assembly.Load(an);
                        assembliesToVisit.Enqueue(a);
                    }
				}

				foreach (Type type in assemblyVisiting.GetExportedTypes())
				{
					if (predicater(type) && typesReturned.Add(type.FullName))
						yield return type;
				}
			}
		}
	}
}

