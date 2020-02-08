using System;

namespace Scheduler
{
    class Program
    {
        static EventProcessor eventProcessor = new EventProcessor();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                switch (UserInterface.printMenu())
                {
                    case UserInterface.Action.ViewShedule:
                        {
                            DisplaySchedule();
                            break;
                        }
                    case UserInterface.Action.AddNote:
                        {
                            AddNote();
                            break;
                        }
                    case UserInterface.Action.LoadFile:
                        {
                            LoadFile();
                            break;
                        }
                    case UserInterface.Action.SaveFile:
                        {
                            SaveFile();
                            break;
                        }
                    case UserInterface.Action.Exit:
                        {
                            return;
                        }
                }
            }
        }
        static void DisplaySchedule()
        {
            UserInterface.Action action;
            do
            {
                //выбор одного из элементов списка, а так же действия, которое над ним необходимо провести
                var eventInfo = UserInterface.printNotes(eventProcessor.GetEventList());
                action = eventInfo.action;
                //редактирование элемента списка в зависимости от его типа
                if (action == UserInterface.Action.EditNote)
                {
                    bool isEvent = eventProcessor.IsEvent(eventInfo.itemNumber);
                    var newEvent = UserInterface.createNote(edit: true, isEvent);
                    if (isEvent) 
                        eventProcessor.UpdateEvent(
                            eventInfo.itemNumber,
                            new ScheduleEvent(
                                newEvent.description,
                                newEvent.beginTime,
                                newEvent.endTime
                        ));
                    else
                        eventProcessor.UpdateEvent(
                            eventInfo.itemNumber,
                            new ScheduleTask(
                                newEvent.description,
                                newEvent.beginTime
                        ));
                }
                if (action == UserInterface.Action.DeleteNote)
                {
                    eventProcessor.DeleteEvent(eventInfo.itemNumber);
                }

            } while (action != UserInterface.Action.BackToMenu);
        }

        static void AddNote() 
        {
            //ввод данных для добалвения записи
            var newEvent = UserInterface.createNote(edit: false);
            if (newEvent.endTime == DateTime.MinValue)
                eventProcessor.AddEvent(
                    new ScheduleTask(
                        newEvent.description,
                        newEvent.beginTime
                     ));
            else
                eventProcessor.AddEvent(
                    new ScheduleEvent(
                        newEvent.description,
                        newEvent.beginTime,
                        newEvent.endTime
                    ));
        }
        
        static void LoadFile()
        {
            string filename = UserInterface.LoadFile();
            if (filename.Length > 0)
            {
                if (eventProcessor.LoadFromFile(filename))
                    UserInterface.PrintMessage("Файл успешно загружен");
                else
                    UserInterface.PrintMessage("Ошибка загрузки файла");
            }
            else
                UserInterface.PrintMessage("Файл не найден");
        }

        static void SaveFile()
        {
            if (eventProcessor.GetCountOfNotes() > 0)
            {
                string filename = UserInterface.SaveFile();
                if (eventProcessor.SaveToFile(filename))
                    UserInterface.PrintMessage("Файл успешно сохранен");
                else
                    UserInterface.PrintMessage("Ошибка сохранения файла");
            }
            else
                UserInterface.PrintMessage("Список пуст");
        }
    }

}
