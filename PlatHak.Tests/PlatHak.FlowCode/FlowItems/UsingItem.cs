using System.Text;

namespace PlatHak.FlowCode.FlowItems
{
    public class UsingItem : FlowItem
    {
        public string Name { get; set; }

        public UsingItem(string name)
        {
            Name = name;
        }
        public override void AppendString(CodeBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"using {Name};");
        }
    }
}