using System;
using System.Diagnostics.Contracts;

namespace PlatHak.FlowCode.FlowItems
{
    public class PropertyItem : AccessFlowItem
    {
        public ClassItem Type { get; set; }
        public string Name { get; set; }

        public PropertyItem(AccessLevel accessLevel, ClassItem type, string name) : base(accessLevel)
        {
            if(type == null) throw new ArgumentException("Class definition cannot be null");
            if(type.IsStatic) throw new ArgumentException("Class definition cannot be static");
            Type = type;
            Name = name;
        }

        public override void AppendString(CodeBuilder stringBuilder)
        {
            stringBuilder.Append(AccessLevel.GetString());
            if (IsStatic && !IsAbstract) stringBuilder.Append("static");
            stringBuilder.Append(Type.Name);
            stringBuilder.Append(Name);
            stringBuilder.AppendLine("{ get; set; }");
        }
    }
}