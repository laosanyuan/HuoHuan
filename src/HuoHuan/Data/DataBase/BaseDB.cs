using Dapper;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace HuoHuan.Data.DataBase
{
    abstract class BaseDB
    {
        public string FilePath { get; protected set; } = null!;
        public string FileName { get; protected set; } = null!;
        public string TableName { get; protected set; } = null!;

        #region [Functions]
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="command">创建表</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        protected bool InitDataBase(string command, string tableName = null)
        {
            if (String.IsNullOrEmpty(this.FileName))
            {
                return false;
            }

            var p = Path.Combine(FilePath, FileName);
            if (!File.Exists(p))
            {
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }
                File.Create(p).Close();
            }

            if (!this.QueryTableExist(tableName))
            {
                if (!this.ExecuteNonQuery(command))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 查询数据表是否已经存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private bool QueryTableExist(string tableName)
        {
            using (SQLiteConnection connection = DataBaseConnection())
            {
                //判断table是否已经存在
                if (tableName == null)
                {
                    tableName = this.TableName;
                }
                var count = connection.QueryFirst<int>($"SELECT COUNT(*) FROM sqlite_master WHERE TYPE = 'table' AND NAME = '{tableName}'");
                return count != 0;
            }
        }

        /// <summary>
        /// 获取数据库连接实例
        /// </summary>
        /// <returns></returns>
        protected SQLiteConnection DataBaseConnection()
        {
            return new SQLiteConnection("data source = " + Path.Combine(FilePath, FileName));
        }

        /// <summary>
        /// 执行无查询操作
        /// </summary>
        /// <param name="commandStr"></param>
        /// <returns></returns>
        protected bool ExecuteNonQuery(string commandStr)
        {
            using (IDbConnection connection = this.DataBaseConnection())
            {
                connection.ExecuteAsync(commandStr);
            }
            return true;
        }
        #endregion
    }
}
