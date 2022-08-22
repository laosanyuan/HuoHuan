using Dapper;
using Dapper.Contrib.Extensions;
using HuoHuan.DataBase.Models;
using HuoHuan.Utils;

namespace HuoHuan.DataBase.Services
{
    public class GroupDB : BaseDB
    {
        public GroupDB()
        {
            base.FileName = $"group.db";
            base.FilePath = FolderUtil.DbPath;
            base.TableName = "groups";

            var command = $"CREATE TABLE {base.TableName} (Url varchar(200) not null, GroupName varchar(100), " +
                $"QRText varchar(100), InvalidateDate DateTime, LocalPath varchar(200), FileName varchar(200), PRIMARY KEY(Url))";
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
            await connection.InsertAsync(wechatGroup);
        }

        /// <summary>
        /// 查询当前有效微信群
        /// </summary>
        /// <returns></returns>
        public async Task<List<GroupImage>> QueryValidateGroup()
        {
            using var connection = base.DataBaseConnection();
            var sql = $"SELECT * FROM {base.TableName} WHERE InvalidateDate BETWEEN " +
                $"date('now') AND date('now','start of day','8 day')";
            var result = (await connection.QueryAsync<dynamic>(sql))?.Select(t => new GroupImage()
            {
                Url = t.Url,
                GroupName = t.GroupName,
                QRText = t.QRText,
                InvalidateDate = Convert.ToDateTime(t.InvalidateDate),
                LocalPath = t.LocalPath,
                FileName = t.FileName,
            }).ToList();
            return result!;
        }

        /// <summary>
        /// 判断当前链接所包含的二维码是否已存在
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsUrl(string url)
        {
            using var connection = base.DataBaseConnection();
            var sql = $"SELECT COUNT(*) FROM {base.TableName} WHERE Url = '{url}'";
            return (await connection.QueryFirstAsync<int>(sql)) > 0;
        }
        #endregion
    }
}
