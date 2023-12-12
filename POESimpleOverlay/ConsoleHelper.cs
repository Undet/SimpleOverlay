using POESimpleOverlay.DataModels;
using POESimpleOverlay.Overlay;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.FSharp.Core.ByRefKinds;

namespace POESimpleOverlay
{
    internal class ConsoleHelper
    {
        public static List<OverlayWrapper> overlayWrappers = new List<OverlayWrapper>();
        public static SimpleTextOverlay simpleTextOverlay;
        public static List<SimpleImageOverlay> simpleImageOverlays  = new List<SimpleImageOverlay>();

        public static async void Handle(string inputs)
        {

            if (string.IsNullOrEmpty(inputs)) return;

            if (File.Exists(inputs))
            {
                CreateOverlay(inputs);
            }

            HandleCommand(inputs);

        }
        private static void CreateOverlay(string path)
        {
            try
            {
                var wrapper = CreateOverlayWrapper(path);
                overlayWrappers.Add(wrapper);
                Console.WriteLine("Overlay was created: " + wrapper.ToString());
                new Thread(wrapper.Overlay.Run).Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Возникла ошибка при создании оверлея\n{e.Message}");
            }
        }

        private static void HandleCommand(string inputs)
        {
            var goodString = inputs.Trim().ToLower().Split(" ");
            try
            {
                switch (goodString[0])
                {
                    case "help":
                        Help(); break;
                    case "list":
                        IdList(); break;
                    case "clear":
                        Console.Clear(); break;
                    case "delall":
                        Dellall(); break;
                    case "del":
                        DeleteById(int.Parse(goodString[1])); break;
                    case "change":
                        Change(int.Parse(goodString[1])); break;
                    case "goto":
                        GotoCommand(int.Parse(goodString[1])); break;
                    default:
                        break;
                }
            }
            catch (Exception e )
            {
                Console.WriteLine("Ошибка ввода команды: " + e.Message);
            }
            
        }

        private static void GotoCommand(int v)
        {
            simpleTextOverlay.GotoPage(v);
        }

        private static void DeleteById(int id)
        {
            var overlay = overlayWrappers.Find((x) => {  return x.Id == id; });
            overlay.Overlay.Dispose();
            overlayWrappers.Remove(overlay);
        }
        private static void Change(int id) 
        { 
            var overlay = overlayWrappers.Find((x) => {  return x.Id == id; });
            overlay.Overlay.Dispose();
            CreateOverlay(overlay.Path);
        }
        private static void Dellall()
        {
            foreach (var ov in overlayWrappers)
            {
                ov.Overlay.Dispose();
            }
            Console.WriteLine("Список отчищен");
        }
        private static void IdList()
        {
            if (overlayWrappers.Count == 0)
            {
                Console.WriteLine("Вы не создали ни одного оверлея");
                return;
            }
            Console.WriteLine("Overlay list:");
            foreach (var overlay in overlayWrappers)
            {
                Console.WriteLine("\t" + overlay.ToString());
            }
        }
        private static OverlayWrapper CreateOverlayWrapper(string path)
        {
            if (!Path.Exists(path))
            {
                new Exception("Не верное расположение файла");
            }


            var format = Path.GetExtension(path);
            if (format == ".txt")
            {
                if (simpleTextOverlay != null)
                {
                    var toRemove = overlayWrappers.Find(x => x.Overlay == simpleTextOverlay);
                    overlayWrappers.Remove(toRemove);
                    simpleTextOverlay.Dispose();
                }
                var textData = CreateTextData(path);
                var textOverlay = new SimpleTextOverlay(textData);
                simpleTextOverlay = textOverlay;
                return new OverlayWrapper(textOverlay, OverlayWrapper.OverlayType.Text,Path.GetFileName(path), path);

            }
            else
            {
                var pos = ConsoleHelper.GetElementPosition();
                Console.Write("Введите размер (к примеру размер 0,5 уменьшит картинку в 2 раза): ");
                var sizeMult = float.Parse(Console.ReadLine());
                Console.Write("Введите прозрачность (к примеру значение 0,5 уменьшит прозрачность в 2 раза): ");
                var opacity = float.Parse(Console.ReadLine());
                var imageData = new ImageData(path, pos, sizeMult, opacity);
                
                var imageOverlay = new SimpleImageOverlay(imageData);
                simpleImageOverlays.Add(imageOverlay);
                return new OverlayWrapper(imageOverlay, OverlayWrapper.OverlayType.Image, Path.GetFileName(path), path);
            }
        }
        private static TextData CreateTextData(string filePath)
        {
            TextData textData;
            try
            {
                textData = new TextData(filePath);
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка разметки файла {filePath}\n{e.Message}");
            }

            return textData;
        }

        public static void PrintInfo() {
            Console.WriteLine("Done!\n" +
                "---------------------------------------------------------\n" +
                "Для того чтобы посмотреть следующую строку нажми 'z'\n" +
                "Для того чтобы посмотреть предыдущую строку нажми 'ctrl+z'\n" +
                "Для того чтобы скрыть оверлей нажми 'ctrl+r'\n" +
                "---------------------------------------------------------\n\n" +
                "Отобразить можно текст или картики.\n" +
                "Для загрузки перетащи файл в консоль\n" +
                "Заголовки можно выделять: [YOUR_TEXT]\n\n" +
                "---------------------------------------------------------\n\n" +
                "Ожидание загрузки файла...");
        }
        public static void Help()
        {            
            Console.WriteLine("\tdel {id} - Удалить элемент по id\n" +
                              "\tchange {id} - Изменить оверлей\n" +
                              "\tlist - Список зарегистрированных оверлеев\n" +
                              "\thelp - Вывести этот список\n" +
                              "\tdelall - Удалить все оверлеи\n" +
                              "\tclear - Отчистить консоль\n" +
                              "\tgoto {page} - Перелистнуть на страницу {page}");
        }
        public static PositionHelper.ItemPosition GetElementPosition()
        {

            Console.WriteLine(
                "Выбирите где расположить элемент \n" +
                "╔═══╦═══╦═══╗\n" +
                "║ 1 ║ 2 ║ 3 ║\n" +
                "╠═══╬═══╬═══╣\n" +
                "║ 4 ║ 5 ║ 6 ║\n" +
                "╠═══╬═══╬═══╣\n" +
                "║ 7 ║ 8 ║ 9 ║\n" +
                "╚═══╩═══╩═══╝\n"
                );
            var inputStrng = Console.ReadLine();
            var intPos = int.Parse(inputStrng);
            return (PositionHelper.ItemPosition)intPos;
        }

    }
}
