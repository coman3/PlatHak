using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatHak.FlowCode.FlowItems;

namespace PlatHak.FlowCode
{
    public class FlowCodeBuilder
    {
        public List<FlowItem> FlowItems { get; set; }

        public FlowCodeBuilder()
        {
            FlowItems = new List<FlowItem>();
        }

        public override string ToString()
        {
            var stringbuilder = new CodeBuilder();
            foreach (var usingItem in FlowItems.OfType<UsingItem>())
            {
                usingItem.AppendString(stringbuilder);
            }
            foreach (var usingItem in FlowItems.Where(x=> !(x is UsingItem)))
            {
                usingItem.AppendString(stringbuilder);
            }
            return stringbuilder.ToString();
        }
    }
}
