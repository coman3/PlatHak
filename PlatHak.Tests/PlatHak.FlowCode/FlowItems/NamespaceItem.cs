using System.Text;

namespace PlatHak.FlowCode.FlowItems
{
    public class NamespaceItem : FlowItem
    {
        public string Name { get; set; }

        public NamespaceItem(string name)
        {
            Name = name;
        }
        public override void AppendString(CodeBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"namespace {Name}");
            stringBuilder.OpenBracket();
            AppendItems(stringBuilder);
            stringBuilder.CloseBracket();

        }
    }
}