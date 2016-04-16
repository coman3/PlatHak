namespace PlatHak.FlowCode
{
    public abstract class AccessFlowItem : FlowItem
    {
        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public AccessLevel AccessLevel { get; set; }

        protected AccessFlowItem(AccessLevel accessLevel)
        {
            AccessLevel = accessLevel;
        }
    }
}