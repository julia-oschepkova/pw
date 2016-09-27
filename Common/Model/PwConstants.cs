using System;

namespace Common.Model
{
    public static class PwConstants
    {
        public const int InitialSum = 500;
        public static Guid UnlimitedAccountId => Guid.Parse("ef2d277d-37cd-4491-9f96-48a96638658a");
        public static Guid RootUserId => Guid.Parse("80e2be86-78ec-40c0-b549-53d1cb08c0d6");
    }
}