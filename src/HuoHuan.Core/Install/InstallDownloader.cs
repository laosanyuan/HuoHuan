namespace HuoHuan.Core.Install
{
    public delegate void DownloadProgressEventHandler(object sender, DownloadProgressArgs args);

    public class InstallDownloader
    {
        #region [Fields]
        private HttpClient _client = new();
        private readonly int _bufferSize = 8192;
        #endregion

        #region [Events]
        public event DownloadProgressEventHandler DownloadProgressChanged = null!;
        #endregion

        public async Task DownloadAsync(string url, string fileName, CancellationToken cancellationToken)
        {
            long readLength = 0;
            long allLength = 0;

            try
            {
                var response = await _client.GetAsync(url, cancellationToken);
                using Stream stream = response.Content.ReadAsStream();
                using FileStream fileStream = new(fileName, FileMode.Create);
                byte[] buffer = new byte[this._bufferSize];
                int length;
                allLength = stream.Length;
                while ((length = await stream.ReadAsync(buffer, cancellationToken)) != 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        goto _End;
                    }
                    readLength += length;
                    // 写入到文件
                    fileStream.Write(buffer, 0, length);
                    this.DownloadProgressChanged?.Invoke(this, new DownloadProgressArgs()
                    {
                        AllSize = allLength,
                        DownloadedSize = readLength,
                        IsEnded = stream.Length == readLength,
                    });
                }
            }
            catch (Exception ex)
            {
                goto _End;
            }
        _End:
            this.DownloadProgressChanged?.Invoke(this, new DownloadProgressArgs()
            {
                AllSize = allLength,
                DownloadedSize = readLength,
                IsEnded = true,
            });
        }
    }

    public class DownloadProgressArgs : EventArgs
    {
        /// <summary>
        /// 全部文件大小
        /// </summary>
        public long AllSize { get; set; }
        /// <summary>
        /// 已下载部分大小
        /// </summary>
        public long DownloadedSize { get; set; }
        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsEnded { get; set; }
    }
}
