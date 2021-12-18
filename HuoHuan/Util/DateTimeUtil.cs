using System;

namespace HuoHuan.Util
{
    class DateTimeUtil
    {
        /// <summary>
        /// 比较日期是否在有效范围内
        /// </summary>
        /// <param name="start">开始日期</param>
        /// <param name="day">范围天数</param>
        /// <param name="targetTime">目标日期</param>
        /// <returns></returns>
        public static bool IsValidTime(DateTime start, DateTime targetTime, int day = 7)
        {
            var end = start.AddDays(day);
            if (DateTime.Compare(start.Date, targetTime.Date) < 0 && DateTime.Compare(targetTime.Date, end.Date) <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
