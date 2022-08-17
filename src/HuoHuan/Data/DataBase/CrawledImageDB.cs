using Dapper;
using HuoHuan.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HuoHuan.Data.DataBase
{
    internal class CrawledImageDB : BaseDB
    {
        public static CrawledImageDB Instance { get; private set; } = new CrawledImageDB();

        private CrawledImageDB()
        {
            base.FileName = "crawled_url.db";
            base.FilePath = FolderUtil.DbPath + @"\DataBase";
            base.TableName = "urls";

            var command = $"CREATE TABLE {base.TableName} (url varchar(200),timestamp varchar(200))";
            base.InitDataBase(command);
        }

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
        /// 判断是否已存在过
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> IsExists(string url)
        {
            using var connection = base.DataBaseConnection();
            var sql = $"SELECT COUNT(*) FROM {base.TableName} WHERE url = '{url}'";
            var result = (await connection.QueryAsync<dynamic>(sql))
                .Select<dynamic, (string, long)>(t => ((string)t.url, Convert.ToInt64(t.timestamp)))?.FirstOrDefault();

            if (result is not null)
            {
                if ((DateTime.Now - new DateTime(result.Value.Item2)).Days > 3)
                {
                    sql = $"UPDATE {base.TableName} Set timestamp = '{DateTime.Now.Ticks}' WHERE url = '{url}'";
                    await connection.ExecuteAsync(sql);
                }
                return true;
            }
            return false;
        }

        public async Task Insert(string url)
        {
            using var connection = base.DataBaseConnection();
            var sql = $"INSERT INTO {base.TableName} (url,timestamp) values ('{url}','{DateTime.Now.Ticks}')";
            await connection.ExecuteAsync(sql);
        }

        /// <summary>
        /// 移除过于陈旧数据，节约空间
        /// </summary>
        /// <returns></returns>
        public async Task ClearStaleUrls()
        {
            //TODO 
            // 1. 获取最后插入时间
            // 2. 获取超出特定期限以外的时间，如3个月
            // 3. 删除所有超出期限的无效数据
        }
    }
}
