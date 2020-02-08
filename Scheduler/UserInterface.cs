using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scheduler
{
    //Класс для отображения пользовательского интерфейса
    static class UserInterface
    {
        public enum Action
        {
            ViewShedule,
            AddNote,
            LoadFile,
            SaveFile,
            Exit,
            BackToMenu,
            EditNote,
            DeleteNote
        }
        //
        public static Action printMenu()
        {
            string[] menuItem = {
                " Просмотр и редактирование событий ",
                "         Добавление событий        ",
                "         Загрузить из файла        ",
                "          Сохранить в файл         ",
                "               Выход               "
            };
            ConsoleKeyInfo key;
            int choice = 0;
            do
            {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < menuItem.Length; i++)
                {
                    if (i == choice)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(menuItem[i]);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else Console.WriteLine(menuItem[i]);
                }
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                    if (choice != 0) choice--;

                if (key.Key == ConsoleKey.DownArrow)
                    if (choice != menuItem.Length - 1) choice++;

            } while (key.Key != ConsoleKey.Enter);

            return (UserInterface.Action)choice;
        }
        //Отображение и работа со списком записей
        public static (int itemNumber, Action action) printNotes(string[] notes)
        {
            Console.Clear();
            ConsoleKeyInfo key;
            if (notes != null)
            {
                int itemNumber = 0;
                Console.WriteLine("             Esc - вернуться в меню            Enter - редактировать            Del - удалить         ");
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                Console.WriteLine("|                           Описание                           |    Дата начала   |  Дата окончания  |");
                while (true)
                {
                    Console.SetCursorPosition(0, 3);
                    for (int i = 0; i < notes.Length; i++)
                    {
                        if (i == itemNumber)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine(notes[i]);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else Console.WriteLine(notes[i]);
                    }
                    key = Console.ReadKey();
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            {
                                if (itemNumber != 0) itemNumber--;
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                if (itemNumber != notes.Length - 1) itemNumber++;
                                break;
                            }
                        case ConsoleKey.Enter:
                            {
                                return (itemNumber, Action.EditNote);
                            }
                        case ConsoleKey.Escape:
                            {
                                return (0, Action.BackToMenu);
                            }
                        case ConsoleKey.Delete:
                            {
                                return (itemNumber, Action.DeleteNote);
                            }
                    }
                };
            }
            else
            {
                Console.WriteLine("Список пуст");
                Console.ReadKey();
                return (0, Action.BackToMenu);
            }
        } 
        //Интерфейс добавления и редактирования события
        public static (string description, DateTime beginTime, DateTime endTime) createNote(bool edit, bool isEvent=false)
        {
            if (!edit)
            {
                ConsoleKeyInfo key;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Добавить задачу(t) или событие(e)?");
                    key = Console.ReadKey();
                } while (key.Key != ConsoleKey.T && key.Key != ConsoleKey.E);
                if (key.Key == ConsoleKey.E) isEvent = true;
            }
            string description="";
            DateTime beginTime = new DateTime();
            DateTime endTime = new DateTime();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Описание должно быть меньше 60 символов, время окончания должно быть позже чем время начала события");
                Console.WriteLine("---------------------------------------------------------------------------------------------------");
                Console.WriteLine("Введите описание: ");
                if (description == "") 
                {
                    description = Console.ReadLine();

                    if (description.Length > 60)
                    {
                        Console.WriteLine("Описание должно быть не больше 60 символов");
                        Console.ReadKey();
                        description = "";
                        continue;
                    }
                    if (description == "")
                    {
                        Console.WriteLine("Описание не может быть пустым");
                        Console.ReadKey();
                        continue;
                    }
                }
                else
                    Console.WriteLine(description);

                try
                {
                    Console.WriteLine("Введите время начала в формате 'dd.mm.yyyy hh:mm' или 'hh:mm' если событие сегодня");
                    beginTime = DateTime.Parse(Console.ReadLine());
                    if (isEvent)
                    {
                        Console.WriteLine("Введите время окончания в формате 'dd.mm.yyyy hh:mm' или 'hh:mm' если событие сегодня");
                        endTime = DateTime.Parse(Console.ReadLine());
                    }
                }
                catch
                {
                    Console.WriteLine("Ошибка, дата или время введено неверно");
                    Console.ReadKey();
                    continue;
                }
                if (beginTime>endTime && isEvent)
                {
                    Console.WriteLine("Время окончания должно быть позже чем время начала события");
                    Console.ReadKey();
                    continue;
                }
                break;
            };
            return (description, beginTime, endTime);
        }
        //Интерфейс загрузки из файлы
        public static string LoadFile()
        {
            string filename;
            do
            {
                Console.Clear();
                Console.WriteLine("Введите название файла: ");
                filename = Console.ReadLine();
                if (filename == "" || filename.Length>255)
                {
                    Console.WriteLine("Недопустимое название файла");
                    Console.ReadKey();
                    continue;
                }
            } while (false);
            FileInfo fileInfo = new FileInfo(filename + ".json");
            if (fileInfo.Exists)
                return filename;
            else
                return "";
        }
        //Интерфейс сохранения в файл
        public static string SaveFile()
        {
            string filename;
            do
            {
                Console.Clear();
                Console.WriteLine("Введите название файла: ");
                filename = Console.ReadLine();
                if (filename == "" || filename.Length > 255)
                {
                    Console.WriteLine("Недопустимое название файла");
                    Console.ReadKey();
                    continue;
                }
                FileInfo fileInfo = new FileInfo(filename + ".json");
                if (fileInfo.Exists)
                {
                    Console.WriteLine("Файл с этим именем уже существует, заменить? (y)");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                        return filename;
                    else
                        continue;
                }
                else
                    return filename;
            } while (false);
            return filename;
        }
        //Функция для вывода информационного сообщения
        public static void PrintMessage(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }
}
