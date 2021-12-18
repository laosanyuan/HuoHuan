using HuoHuan.Enum;

namespace HuoHuan.Core.Filter
{
    class GroupFilterFactory
    {
        public static IGroupFilter CreatFilter(QRCodeType type)
        {
            switch (type)
            {
                case QRCodeType.WechatGroup:
                    return new WechatGroupFilter();
                default:
                    return null;
            }
        }
    }
}
