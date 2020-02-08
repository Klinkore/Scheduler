using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Scheduler
{
    class EventProcessor
    {
        List<ScheduleTask> notes = new List<ScheduleTask>();
        //возвращение записей из планировщика в виде строк, для удобного отображения
        public string[] GetEventList()
        {
            if (notes.Count != 0)
            {
                string[] eventlist = new string[notes.Count];
                for (int i = 0; i < notes.Count; i++)
                {
                    eventlist[i] = notes[i].ToString();
                }
                return eventlist;
            }
            else return null;
        }
        //Добавление события в планировщик, сортировка по начальному времени
        public void AddEvent(ScheduleTask note)
        {
            bool eventAdded = false;
            if (notes.Count > 0)
            {
                for (int i = 0; i < notes.Count; i++)
                {
                    if (notes[i].BeginTime > note.BeginTime)
                    {
                        notes.Insert(i, note);
                        eventAdded = true;
                        break;
                    }
                }
            }
            if (!eventAdded)
                notes.Add(note);
        }
        public void DeleteEvent(int position)
        {
            notes.Remove(notes[position]);
        }
        //обновление информации о записи
        public void UpdateEvent(int position, ScheduleTask scheduleTask)
        {
            notes.RemoveAt(position);
            AddEvent(scheduleTask);
        }
        //загрузка данных из json-файла
        public bool LoadFromFile(string filename)
        {
            List<ScheduleEvent> tempList = new List<ScheduleEvent>();
            try
            {
                //десериализация файла во временный список
                using (StreamReader file = File.OpenText(filename+".json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    tempList = (List<ScheduleEvent>)serializer.Deserialize(file, typeof(List<ScheduleEvent>));
                }
                notes.Clear();
                //сохранение данных в список в зависимости от их формата
                foreach(ScheduleEvent note in tempList)
                {
                    if (note.EndTime == DateTime.MinValue)
                        AddEvent(new ScheduleTask(
                            note.Description,
                            note.BeginTime));
                    else
                        AddEvent(new ScheduleEvent(
                            note.Description,
                            note.BeginTime,
                            note.EndTime
                            ));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Сериализация списка в json-файл
        public bool SaveToFile(string filename)
        {
            try 
            {
                File.WriteAllText(filename + ".json", JsonConvert.SerializeObject(notes));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int GetCountOfNotes()
        {
            return notes.Count;
        }
        public bool IsEvent(int position)
        {
            if (notes.Count > 0)
            {
                Type type = notes[position].GetType();
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    if (prop.Name == "EndTime") return true;
                }
            }
            return false;
        }
    }
}
