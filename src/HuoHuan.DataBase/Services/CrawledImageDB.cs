using Dapper;
using Dapper.Contrib.Extensions;
using HuoHuan.DataBase.Models;
using HuoHuan.Utils;
using System.Data;

namespace HuoHuan.DataBase.Services
{
    public class CrawledImageDB : BaseDB
    {
        public CrawledImageDB()
        {
            base.FileName = "crawled_url.db";
            base.FilePath = FolderUtil.DbPath + @"\DataBase";
            base.TableName = "urls";

            var command = $"CREATE TABLE {base.TableName} (Url varchar(200) not null, DateTime DateTime, PRIMARY KEY(Url))";
            base.InitDataBase(command);
        }

        #region [Methods]
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task Insert(string url, DateTime? time = null)
        {
            using var connection = base.DataBaseConnection();
            await connection.InsertAsync(new PreviewImage() { Url = url, DateTime = time ?? DateTime.Now });
        }

        /// <summary>
        /// 判断是否已存在过
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> IsExists(string url, bool needUpdate = true)
        {
            using var connection = base.DataBaseConnection();
            var sql = $"SELECT * FROM {base.TableName} WHERE Url = '{url}'";
            var result = (await connection.QueryAsync<dynamic>(sql))
                ?.Select<dynamic, (string url, DateTime time)>(t => ((string)t.Url, Convert.ToDateTime(t.DateTime)))
                ?.FirstOrDefault();

            if (result is not null && result.Value.url?.Equals(url) == true)
            {
                if (needUpdate && (DateTime.Now - result.Value.time).Days > 3)
                {
                    await connection.UpdateAsync(new PreviewImage() { Url = url, DateTime = DateTime.Now });
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否存在并插入数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAndInsert(string url)
        {
            var result = await IsExists(url);
            if (!result)
            {
                await Insert(url);
            }
            return result;
        }

        /// <summary>
        /// 移除过于陈旧数据，节约数据库空间
        /// </summary>
        /// <returns></returns>
        public async Task ClearStaleUrls(int days = 30)
        {
            using var connection = base.DataBaseConnection();
            var sql = $"DELETE FROM {base.TableName} WHERE date('now','-{days} day') >= date(DateTime)";
            await connection.ExecuteAsync(sql);
        }
        #endregion
    }
}
