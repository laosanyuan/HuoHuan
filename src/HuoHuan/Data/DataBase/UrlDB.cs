using Dapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HuoHuan.Data.DataBase
{
    internal class UrlDB : BaseDB
    {
        public static UrlDB Instance { get; } = new UrlDB();
        private UrlDB()
        {
            base.FileName = "url.db";
            base.FilePath = Environment.CurrentDirectory + @"\DataBase";
            base.TableName = "urls";

            var command = $"CREATE TABLE {base.TableName} (url varchar(200),tags varchar(200))";
            base.InitDataBase(command);
        }

        #region [Methods]
        /// <summary>
        /// 更新链接使用标记
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tag"></param>
        public async void UpdateUsedUrl(string url, string tag)
        {
            using (var connection = base.DataBaseConnection())
            {
                var sql = $"SELECT COUNT(*) FROM {base.TableName} WHERE url = '{url}'";
                var count = connection.QueryFirst<int>(sql);
                if (count > 0)
                {
                    var result = (await connection.QueryAsync<string>($"SELECT tags FROM {base.TableName} WEHRE url = '{url}'")).AsList().First();
                    result += "#" + tag;
                    sql = $"UPDATE {base.TableName} SET tags = '{result}' WHERE url = '{url}'";
                }
                else
                {
                    sql = $"INSERT INTO {base.TableName} (url,tags) values ('{url}','{tag}')";
                }
                await connection.ExecuteAsync(sql);
            }
        }

        /// <summary>
        /// 判断当前链接是否被tag使用过
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task<bool> IsUsedUrl(string url, string tag)
        {
            using (var connection = base.DataBaseConnection())
            {
                var sql = $"SELECT COUNT(*) FROM {base.TableName} WHERE url = '{url}'";
                var count = await connection.QueryFirstAsync<int>(sql);
                if (count > 0)
                {
                    var result = (await connection.QueryAsync<string>($"SELECT tags FROM {base.TableName} WHERE url = '{url}'")).AsList().First();
                    return result.Contains(tag);
                }
                return false;
            }
        }
        #endregion
    }
}
