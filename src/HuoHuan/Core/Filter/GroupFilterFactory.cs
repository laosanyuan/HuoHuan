using HuoHuan.Enums;

namespace HuoHuan.Core.Filter
{
    internal class GroupFilterFactory
    {
        public static IGroupFilter CreatFilter(QRCodeType type)
        {
            return type switch
            {
                QRCodeType.WechatGroup => new WechatGroupFilter(),
                _ => null!,
            };
        }
    }
}
