using Dapper;
using HuoHuan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HuoHuan.Data.DataBase
{
    class GroupDB : BaseDB
    {
        public GroupDB(string fileName)
        {
            base.FileName = $"{fileName}.db";
            base.FilePath = Environment.CurrentDirectory + @"\DataBase";
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
        public void InsertGroup(GroupData wechatGroup)
        {
            using (var connection = base.DataBaseConnection())
            {
                var sql = $"INSERT INTO {base.TableName} (source_url,name,qr_text,invalidate_date,local_path) " +
                    $"values (@SourceUrl,@Name,@QRText,@InvalidateDate,'{wechatGroup.InvalidateDate.ToString("yyyyy-MM-dd")}')";
                connection.ExecuteAsync(sql, wechatGroup);
            }
        }

        /// <summary>
        /// 查询当前有效微信群
        /// </summary>
        /// <returns></returns>
        public List<GroupData> QueryInvalidateGroup()
        {
            using (var connection = base.DataBaseConnection())
            {
                var sql = $"SELECT * FROM {base.TableName} WHERE invalidate_date BETWEEN date('now') AND date('now','start of day','8 day')";
                var result = connection.Query<dynamic>(sql).Select(t => new GroupData()
                {
                    SourceUrl = t.source_url,
                    Name = t.name,
                    QRText = t.qr_text,
                    InvalidateDate = Convert.ToDateTime(t.invalidate_date),
                    LocalPath = t.local_path
                }).ToList();
                return result;
            }
        }

        /// <summary>
        /// 判断当前链接所包含的二维码是否已存在
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> IsExistUrl(string url)
        {
            using (var connection = base.DataBaseConnection())
            {
                var sql = $"SELECT COUNT(*) FROM {base.TableName} WHERE source_url = '{url}'";
                return (await connection.QueryFirstAsync<int>(sql)) > 0;
            }
        }
        #endregion
    }
}
