using HuoHuan.Enums;

namespace HuoHuan.Core.Filter
{
    internal class GroupFilterFactory
    {
        public static IGroupFilter CreateFilter(QRCodeType type)
        {
            return type switch
            {
                QRCodeType.WechatGroup => new WechatGroupFilter(),
                _ => null!,
            };
        }
    }
}
