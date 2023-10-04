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
            long contentLength = 0;
            long totalBytesRead = 0;
            try
            {
                using HttpClient client = new HttpClient();
                using HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                using HttpContent content = response.Content;
                contentLength = content.Headers.ContentLength ?? -1;

                using var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                using var stream = await content.ReadAsStreamAsync(cancellationToken);
                byte[] buffer = new byte[this._bufferSize];
                totalBytesRead = 0;

                int bytesRead;
                do
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        goto _End;
                    }
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    await fileStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);

                    totalBytesRead += bytesRead;

                    if (contentLength != -1)
                    {
                        this.DownloadProgressChanged?.Invoke(this, new DownloadProgressArgs()
                        {
                            AllSize = contentLength,
                            DownloadedSize = totalBytesRead,
                            IsEnded = contentLength == totalBytesRead,
                        });
                    }
                }
                while (bytesRead > 0);
            }
            catch (Exception)
            {
                goto _End;
            }
        _End:
            this.DownloadProgressChanged?.Invoke(this, new DownloadProgressArgs()
            {
                AllSize = contentLength,
                DownloadedSize = totalBytesRead,
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
