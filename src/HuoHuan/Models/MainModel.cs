namespace HuoHuan.Models
{
    /// <summary>
    /// 展示图片
    /// </summary>
    public record DisplayImageInfo
    {
        public string Url { get; set; } = null!;
        public bool IsValid { get; set; }
    }
}
