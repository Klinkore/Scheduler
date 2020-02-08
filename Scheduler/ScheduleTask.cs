using System;

namespace Scheduler
{
    //класс представляющий задачу в планировщике
    class ScheduleTask
    {
        public string Description { get; set; }
        public DateTime BeginTime { get; set; }
        public ScheduleTask(string description, DateTime beginTime)
        {
            Description = description;
            BeginTime = beginTime;
        }
        //преобразование информации о задаче в строку
        public override string ToString()
        {
            string taskInfo = "";
            taskInfo
                += "| " + Description.PadRight(60, ' ')
                + " | " + BeginTime.ToString("g").PadRight(16, ' ')
                + " | " + new string("").PadRight(16, ' ') + " |";
            return taskInfo;
        }
    }
}
