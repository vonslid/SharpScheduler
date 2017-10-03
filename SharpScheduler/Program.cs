using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharpScheduler
{
    class Program
    {
        static ushort tasksCount = 21;
        static ushort taskLength = 140;
        static List<string> Tasks = new List<string>(tasksCount);

        static StringBuilder Messages = new StringBuilder(79);

        static bool saved = true;

        static string versionInfo = "ss0.1";

        static void Main(string[] args)
        {
            Console.Title = "SharpScheduler v0.1";
            Console.WindowHeight = 45;
            Console.WindowWidth = 80;
            Console.BufferHeight = 45;
            Console.BufferWidth = 80;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Clear();

            SplashScreen();

            SetMessage("Готово.");

            ParseFile();

            StartLoop();
        }

        static void StartLoop()
        {
            ConsoleKeyInfo key;
            bool loop = true;
            bool loopRead;
            ushort selected = 0;
            do
            {
                Header(true);
                if (Tasks.Count != 0)
                    PrintTasks(selected);
                else
                    Console.WriteLine("Тут пока ничего нет");

                loopRead = true;
                while (loopRead)
                {
                    key = Console.ReadKey(true); // true не отображает вводимую клавишу
                    switch (key.Key)
                    {
                        case ConsoleKey.F1:
                            ShowHelp();
                            loopRead = false;
                            break;
                        case ConsoleKey.F2:
                            AddTask();
                            loopRead = false;
                            break;
                        case ConsoleKey.F3:
                            if (Tasks.Count > 0)
                            {
                                RemoveTask(selected);
                            }
                            else
                            {
                                SetMessage("Нечего удалаять.");
                            }
                            loopRead = false;
                            break;
                        case ConsoleKey.F4:
                            if (Tasks.Count > 0)
                            {
                                EditTask(selected);
                            }
                            else
                            {
                                SetMessage("Нечего редактировать.");
                            }
                            loopRead = false;
                            break;
                        case ConsoleKey.F5:
                            if (Tasks.Count > 0)
                            {
                                DumpToFile();
                            }
                            else
                            {
                                SetMessage("Нечего сохранять.");
                            }
                            loopRead = false;
                            break;
                        case ConsoleKey.DownArrow:
                            if (selected + 1 < Tasks.Count)
                            {
                                selected++;
                                loopRead = false;
                            }
                            SetMessage("Готово.");
                            break;
                        case ConsoleKey.UpArrow:
                            if (selected > 0 )
                            {
                                selected--;
                                loopRead = false;
                            }
                            SetMessage("Готово.");
                            break;
                        case ConsoleKey.Escape:
                            loop = false;
                            loopRead = false;
                            ExitCheck();
                            break;
                        default:
                            SetMessage("Готово.");
                            break;
                    }
                }
            }
            while (loop);
        }

        static void SplashScreen()
        {
            string welcome = "Добро пожаловать в SharpScheduler v0.1";
            string info = "vonslid 2017";
            string press = "Нажмите любую клавишу...";
            string spaces = "";
            for (int i = 0; i < 20; i++)
            {
                spaces += "\n";
            }
            Console.Write(spaces);

            spaces = "";
            for (int i = 0; i < 40 - welcome.Length / 2; i++)
            {
                spaces += " ";
            }
            Console.WriteLine(spaces + welcome);

            spaces = "";
            for (int i = 0; i < 40 - info.Length / 2; i++)
            {
                spaces += " ";
            }
            Console.WriteLine(spaces + info);

            spaces = "";
            for (int i = 0; i < 40 - press.Length / 2; i++)
            {
                spaces += " ";
            }
            Console.WriteLine(spaces + press);


            spaces = "";
            for (int i = 0; i < 21; i++)
            {
                spaces += "\n";
            }
            Console.Write(spaces);
            Console.ReadKey(true);
        }

        static void SetMessage(string messageText)
        {
            Messages.Clear();
            Messages.Append(messageText);
            if (!saved) Messages.Append(" Нажмите F5, чтобы сохранить изменения.");
        }

        static void Header(bool printFunctionKeys)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;

            StringBuilder lines = new StringBuilder(160);

            string FunctionKeys = "F1-Справка|F2-Новый|F3-Удалить|F4-Ред.|F5-Сохранить|F6-Загрузить|\u2191\u2193-Навигация";
            if (printFunctionKeys)
            {
                lines.Append(FunctionKeys);
                for (int i = 0; i < 79 - FunctionKeys.Length; i++) lines.Append(' ');
                lines.Append('\n');
            }

            lines.Append(Messages.ToString());
            for (int i = 0; i < 79 - Messages.Length; i++) lines.Append(' ');
            lines.Append('\n');

            Console.Write(lines.ToString());

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
        }

        static void ShowHelp()
        {
            Console.Clear();
            StringBuilder help = new StringBuilder();
            
            help.Append(@"   ______                 ____    __          __     __       " + "\n");
            help.Append(@"  / __/ /  ___ ________  / __/___/ /  ___ ___/ /_ __/ /__ ____" + "\n");
            help.Append(@" _\ \/ _ \/ _ `/ __/ _ \_\ \/ __/ _ \/ -_) _  / // / / -_) __/" + "\n");
            help.Append(@"/___/_//_/\_,_/_/ / .__/___/\__/_//_/\__/\_,_/\_,_/_/\__/_/   " + "\n");
            help.Append(@"                 /_/                                          " + "\n");
            help.Append("\n");
            help.Append("\nSharpScheduler v0.1 by vonslid 2017\n\n");
            help.Append("\n\n");
            help.Append("Приложение для ведения списка дел.\n");
            help.Append("Позволяет управлять списком из 21 (!) задачи длинной в 140 символов.\n");
            help.Append("Автоматически считывает данные из файла при запуске.\n");
            help.Append("F1 - Выводит это окно справки.\n");
            help.Append("F2 - добавляет новый задачу в список.\n");
            help.Append("F3 - Удаляет выделенную задачу.\n");
            help.Append("F4 - Позволяет изменить задачу.\n");
            help.Append("F5 - Сохраняет список в файл.\n");
            help.Append("F6 - Загружает список из файла. На случай, если что-то пошло не так. \n");
            help.Append("Да здравствует строка сотояния!\n");
            help.Append("Для выхода есть Escape.\n");

            Console.Write(help.ToString());

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
            } while (key.Key != ConsoleKey.Escape);
        }

        static void AddTask()
        {
            if(Tasks.Count < Tasks.Capacity)
            {
                string text;
                saved = true;
                SetMessage(String.Format("Веедите текст задачи до {0} символов. Enter для подтверждения.", taskLength));

                Header(false);

                text = Console.ReadLine();
                Tasks.Add(text.Substring(0, (text.Length >= taskLength) ? taskLength : text.Length));

                saved = false;
                SetMessage("Задача успешно добавлена.");
            }
            else
            {
                SetMessage(String.Format("Уже добавленно максимальное число задач({0}).", Tasks.Capacity));
            }
        }

        static void PrintTasks(ushort selected)
        {
            int bufSize = Tasks.Capacity * 80 * 2; // 2 строки по 80 на 1 задачу
            StringBuilder buf = new StringBuilder(bufSize);
            for (ushort i = 0; i < Tasks.Count; i++)
            {
                char sign = ' ';
                if (i == selected)
                    sign = '>';
                buf.AppendFormat("{0}{1}\t{2}\n", sign, (i + 1).ToString(), Tasks[i]);
            }
            Console.Write(buf.ToString());
        }

        static void RemoveTask(ushort selected)
        {
            bool s = saved;
            saved = true; 
            SetMessage("Удалить данную задачу? Enter - удалить, Esc - отмена.");
            Header(false);

            Console.WriteLine(Tasks[selected]);

            ConsoleKeyInfo key;
            bool loopRead = true;
            while (loopRead)
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        saved = s;
                        SetMessage("Готово.");
                        loopRead = false;
                        break;
                    case ConsoleKey.Enter:
                        Tasks.RemoveAt(selected);
                        saved = false;
                        SetMessage("Задача удалена.");
                        loopRead = false;
                        break;
                }
            }
        }

        static void EditTask(ushort selected)
        {
            saved = true;
            SetMessage(String.Format("Вместо старой задачи введите новую({0} символов). Enter - для подтверждения.", taskLength));
            Header(false);

            Console.WriteLine(Tasks[selected]);

            string text;

            text = Console.ReadLine();
            Tasks.RemoveAt(selected);
            Tasks.Insert(selected, text.Substring(0, (text.Length >= taskLength) ? taskLength : text.Length));

            saved = false;
            SetMessage("Задача успешно добавлена.");
        }

        static void DumpToFile()
        {
            StringBuilder data = new StringBuilder(8 + (taskLength * tasksCount) +((tasksCount  + 1 )* 2)); // 8 служебн +  21 задача по 140 + 21 раз по @@
            data.Append(versionInfo);

            for(int i = 0; i < Tasks.Count; i++)
            {
                data.Append("@@" + Tasks[i]);
            }
            data.Append("@@");

            try
            {
                File.WriteAllText(@"SSdata.txt", data.ToString());
                saved = true;
                SetMessage("Данные успешно записаны.");
            }
            catch (Exception e)
            {
                SetMessage("Неудалось записать файл.");
                saved = false;
            }
        }

        static void ParseFile()
        {
            int offset = versionInfo.Length + 2;
            int nextOffset;
            string text = "";

            Tasks.Clear();

            //StringBuilder data = new StringBuilder((taskLength * tasksCount) + ((tasksCount + 1) * 2)); // 21 задача по 140 + 21 раз по @@

            try
            {
                string data = File.ReadAllText(@"SSdata.txt");

                if (!data.StartsWith(versionInfo + "@@"))
                {
                    SetMessage("Проблема с файлом.");
                    saved = false;
                }
                else
                {
                    for (int i = 0; i < Tasks.Capacity; i++)
                    {
                        nextOffset = data.IndexOf("@@", offset);
                        if (nextOffset > offset)
                        {
                            text = data.Substring(offset, nextOffset - offset);
                            Tasks.Add(text.Substring(0, (text.Length >= taskLength) ? taskLength : text.Length));
                            offset = nextOffset + 2;
                        }
                        else break;
                    }
                    saved = true;
                    SetMessage("Данные из файла загружены.");
                }
            }
            catch (Exception e)
            {
                SetMessage("Проблема с чтением файла.");
                saved = false;
            }
        }

        static void ExitCheck()
        {
            if (!saved)
            {
                saved = true;

                SetMessage("Список дел не сохранен. Enter - сохранить. Esc - выйти без сохранения.");

                Header(false);

                ConsoleKeyInfo key;
                bool loopRead = true;
                while (loopRead)
                {
                    key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            loopRead = false;
                            break;
                        case ConsoleKey.Enter:
                            DumpToFile();
                            loopRead = false;
                            break;
                    }
                }
            }
        }
    }
}
