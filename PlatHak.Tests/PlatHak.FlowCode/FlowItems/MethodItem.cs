using System;
using System.Collections.Generic;

namespace PlatHak.FlowCode.FlowItems
{
    public class MethodItem : AccessFlowItem
    {
        public string Name { get; set; }
        public ClassItem ReturnType { get; set; }
        public Dictionary<string, ClassItem> Parameters { get; set; }

        public MethodItem(AccessLevel accessLevel, string name, ClassItem returnType, Dictionary<string, ClassItem> parameters = null) : base(accessLevel)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
        }

        public override void AppendString(CodeBuilder stringBuilder)
        {
            
        }
    }
}