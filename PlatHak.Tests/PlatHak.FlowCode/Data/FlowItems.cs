using PlatHak.FlowCode.FlowItems;

namespace PlatHak.FlowCode.Data
{
    public static class FlowItems
    {
        public static readonly ClassItem String = new ClassItem(AccessLevel.Public, "string");
        public static readonly ClassItem Boolean = new ClassItem(AccessLevel.Public, "bool");
        public static readonly ClassItem Integer = new ClassItem(AccessLevel.Public, "int");
        public static readonly ClassItem Float = new ClassItem(AccessLevel.Public, "float");
        public static readonly ClassItem Double = new ClassItem(AccessLevel.Public, "double");
        public static readonly ClassItem UnsignedInteger = new ClassItem(AccessLevel.Public, "uint");
        public static readonly ClassItem UnsignedLong = new ClassItem(AccessLevel.Public, "ulong");
        public static readonly ClassItem Long = new ClassItem(AccessLevel.Public, "long");

        public static readonly ClassItem DateTime = new ClassItem(AccessLevel.Public, "DateTime");
        public static readonly ClassItem Guid = new ClassItem(AccessLevel.Public, "Guid");
    }
}