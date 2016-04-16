using PlatHak.FlowCode.FlowItems;

namespace PlatHak.FlowCode.Data
{
    public static class FlowItems
    {
        public static ClassItem String = new ClassItem(AccessLevel.Public, "string");
        public static ClassItem Boolean = new ClassItem(AccessLevel.Public, "bool");
        public static ClassItem Integer = new ClassItem(AccessLevel.Public, "int");
        public static ClassItem Float = new ClassItem(AccessLevel.Public, "float");
        public static ClassItem Double = new ClassItem(AccessLevel.Public, "double");
        public static ClassItem UnsignedInteger = new ClassItem(AccessLevel.Public, "uint");
        public static ClassItem UnsignedLong = new ClassItem(AccessLevel.Public, "ulong");
        public static ClassItem Long = new ClassItem(AccessLevel.Public, "long");

        public static ClassItem DateTime = new ClassItem(AccessLevel.Public, "DateTime");
        public static ClassItem Guid = new ClassItem(AccessLevel.Public, "Guid");
    }
}