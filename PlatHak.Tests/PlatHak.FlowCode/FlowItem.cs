using System.Collections.Generic;
using System.Text;

namespace PlatHak.FlowCode
{
    public abstract class FlowItem
    {
        public List<FlowItem> Items { get; set; }

        protected FlowItem()
        {
            Items = new List<FlowItem>();
        }

        protected void AppendItems(CodeBuilder stringBuilder)
        {
            foreach (var flowItem in Items)
            {
                flowItem.AppendString(stringBuilder);
            }
        }
        public abstract void AppendString(CodeBuilder stringBuilder);
        public override string ToString()
        {
            var builder = new CodeBuilder();
            AppendString(builder);
            return builder.ToString();
        }
    }
}