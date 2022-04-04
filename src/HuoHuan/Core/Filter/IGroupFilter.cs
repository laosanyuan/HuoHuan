using HuoHuan.Enums;
using HuoHuan.Models;
using System.Threading.Tasks;

namespace HuoHuan.Core.Filter
{
    /// <summary>
    /// 群图片过滤器
    /// </summary>
    interface IGroupFilter
    {
        /// <summary>
        /// 过滤器类型
        /// </summary>
        QRCodeType FilterType { get; }

        /// <summary>
        /// 是否为有效图片url
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        Task<bool> IsValidImage(string imageUrl);

        /// <summary>
        /// 根据链接获取群数据
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        GroupData GetGroupData(string imageUrl);

        /// <summary>
        /// 保存下载后数据
        /// </summary>
        /// <param name="data"></param>
        void SaveData(GroupData data);
    }
}
