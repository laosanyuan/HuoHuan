using CommunityToolkit.Mvvm.ComponentModel;
using HuoHuan.Glue;

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

    public class SpiderInfo : ObservableObject
    {
        public ISpider Spider { get; init; }

        public string Name { get; init; }

        private int _count;
        public int Count
        {
            get => this._count;
            set => SetProperty(ref this._count, value);
        }

        private SpiderStatus _status;
        public SpiderStatus Status
        {
            get => this._status;
            set => SetProperty(ref this._status, value);
        }

        private double _progress;
        public double Progress
        {
            get => this._progress;
            set => SetProperty(ref _progress, value);
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => this._imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }
    }
}
