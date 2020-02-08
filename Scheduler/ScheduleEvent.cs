using System;

namespace Scheduler
{
    //класс представляющий событие в планировщике
    class ScheduleEvent : ScheduleTask
    {
        public DateTime EndTime { get; set; }
        public ScheduleEvent(string description, DateTime beginTime, DateTime endTime)
            :base(description,beginTime)
        {
            EndTime = endTime;
        }
        //неявное преобразование информации о событии в строку
        public override string ToString(){ 
            string eventInfo = "";
            eventInfo
                += "| " + Description.PadRight(60, ' ')
                + " | " + BeginTime.ToString("g").PadRight(16, ' ')
                + " | " + EndTime.ToString("g").PadRight(16, ' ') + " |";
            return eventInfo;
        }
    }
}
