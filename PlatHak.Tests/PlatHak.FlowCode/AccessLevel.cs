using System;

namespace PlatHak.FlowCode
{
    public enum AccessLevel
    {
        Public,
        Private,
        Protected,
        Internal
    }

    public static class AccessLevelExtentions
    {
        public static string GetString(this AccessLevel level)
        {
            var name = Enum.GetName(typeof (AccessLevel), level);
            return name?.ToLower() ?? "";
        }
    }
}