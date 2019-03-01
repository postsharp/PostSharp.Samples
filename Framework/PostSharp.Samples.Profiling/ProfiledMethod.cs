using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PostSharp.Samples.Profiling
{
    internal class MetricMetadata
    {
        public MetricMetadata(MethodBase method, int index)
        {
            this.Index = index;
            this.Name = method.DeclaringType.FullName+ "." + method.Name + "(" + string.Join(",", method.GetParameters().Select(p => p.ParameterType.FullName)) + ")";

        }

        internal int Index { get;}
        public string Name { get; }
    }
}
