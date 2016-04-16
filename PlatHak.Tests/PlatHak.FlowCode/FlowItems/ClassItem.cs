
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatHak.FlowCode.FlowItems
{
    public class ClassItem : AccessFlowItem
    {
        public string Name { get; set; }
        public List<ClassItem> Inherits { get; set; }
        public override void AppendString(CodeBuilder stringBuilder)
        {
            stringBuilder.Append(AccessLevel.GetString());
            if (IsAbstract && !IsStatic) stringBuilder.Append("abstract");
            if (IsStatic && !IsAbstract) stringBuilder.Append("static");
            stringBuilder.Append("class");
            stringBuilder.Append(Name);
            if (!IsStatic && Inherits != null && Inherits.Count > 0)
            {
                stringBuilder.Append(":", addStartSpace: true);
                stringBuilder.Append(string.Join(", ", Inherits.Select(x => x.Name)));
            }
            stringBuilder.NewLine();

            stringBuilder.OpenBracket();

            AppendItems(stringBuilder);

            stringBuilder.CloseBracket();
        }

        public ClassItem(AccessLevel accessLevel, string name, params ClassItem[] inherits) : base(accessLevel)
        {
            Name = name;
            if(inherits != null) Inherits = inherits.ToList();
        }
    }
}