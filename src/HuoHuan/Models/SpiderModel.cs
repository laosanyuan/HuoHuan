using CommunityToolkit.Mvvm.ComponentModel;
using HuoHuan.Plugin.Contracs;

#nullable disable

namespace HuoHuan.Models
{
    /// <summary>
    /// 爬取参数数据
    /// </summary>
    internal class SpiderData
    {
        /// <summary>
        /// 爬取关键词
        /// </summary>
        public string Key { get; set; } = null!;
        /// <summary>
        /// 爬取页数
        /// </summary>
        public int Page { get; set; }
    }

    [ObservableObject]
    public partial class SpiderInfo
    {
        public ISpider Spider { get; init; }

        public string Name { get; init; }

        [ObservableProperty]
        private int _count;

        [ObservableProperty]
        private SpiderStatus _status;

        [ObservableProperty]
        private double _progress;

        [ObservableProperty]
        private string _imageUrl;
    }
}
