using Dapper;
using HuoHuan.DataBase.Models;
using HuoHuan.Utils;

namespace HuoHuan.DataBase.Services
{
    internal class GroupDB : BaseDB
    {
        public GroupDB()
        {
            base.FileName = $"group.db";
            base.FilePath = FolderUtil.DbPath + @"\DataBase";
            base.TableName = "groups";

            var command = $"CREATE TABLE {base.TableName} (source_url varchar(200),name varchar(100)," +
                $"qr_text varchar(100),invalidate_date varchar(100),local_path varchar(200),UNIQUE(source_url))";
            base.InitDataBase(command);
        }

        #region [Methods]
        /// <summary>
        /// 插入群数据
        /// </summary>
        /// <param name="wechatGroup"></param>
        public async Task InsertGroup(GroupImage wechatGroup)
        {
            using var connection = base.DataBaseConnection();
            var sql = $"INSERT INTO {base.TableName} (source_url,name,qr_text,invalidate_date,local_path) " +
                $"values (@SourceUrl,@Name,@QRText,'{wechatGroup.InvalidateDate.ToString("yyyyy-MM-dd")}',@LocalPath)";
            await connection.ExecuteAsync(sql, wechatGroup);
        }

        /// <summary>
        /// 查询当前有效微信群
        /// </summary>
        /// <returns></returns>
        public async Task<List<GroupImage>> QueryInvalidateGroup()
        {
            using var connection = base.DataBaseConnection();
            var sql = $"SELECT * FROM {base.TableName} WHERE invalidate_date BETWEEN date('now') AND date('now','start of day','8 day')";
            var result = (await connection.QueryAsync<dynamic>(sql)).Select(t => new GroupImage()
            {
                Url = t.source_url,
                Name = t.name,
                QRText = t.qr_text,
                InvalidateDate = Convert.ToDateTime(t.invalidate_date),
                LocalFile = t.local_path
            }).ToList();
            return result;
        }

        /// <summary>
        /// 判断当前链接所包含的二维码是否已存在
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsUrl(string url)
        {
            using var connection = base.DataBaseConnection();
            var sql = $"SELECT COUNT(*) FROM {base.TableName} WHERE source_url = '{url}'";
            return (await connection.QueryFirstAsync<int>(sql)) > 0;
        }
        #endregion
    }
}
