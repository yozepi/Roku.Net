using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using yozepi.Roku;

namespace Roku.Net.ConsoleTests.NetFramework
{
    class Program
    {
        static RokuDiscovery discovery;

        static void Main(string[] args)
        {
            Console.WriteLine("Searching for Rokus...");
            discovery = new RokuDiscovery();
            var rokus = discovery.DiscoverAsync().Result;
            Console.WriteLine($"{rokus.Count} Roku(s) found.");

            while (true)
            {
                IRokuRemote selected = null;

                Console.WriteLine("0) Manually set up a ROKU");
                for (int i = 0; i < rokus.Count; i++)
                {
                    var roku = rokus[i];
                    Console.WriteLine($"{i + 1}) {roku.Info.UserDeviceName} model {roku.Info.ModelNumber} (id {roku.Info.Id})");
                }
                var choice = ParseChoice("Choose a device: ", 0, rokus.Count);
                if (choice == null)
                    break;
                if (choice < 0)
                    continue;
                if (choice == 0)
                {
                    selected = SelectByIp();
                    if (selected == null)
                        continue;
                }
                else
                    selected = rokus[choice.Value - 1];

                DeviceOperations(selected);

                Console.WriteLine();
            }
        }

        static IRokuRemote SelectByIp()
        {
            while (true)
            {
                Console.Write("Enter the ROKU Address: ");
                var ipString = Console.ReadLine();
                if (string.IsNullOrEmpty(ipString))
                    return null;

                IPAddress rokuIp;
                if (!IPAddress.TryParse(ipString, out rokuIp))
                {
                    Console.WriteLine("The address you entered isn't valid");
                    continue;
                }
                var roku = discovery.DiscoverAsync(rokuIp).Result;
                if (roku == null)
                {
                    Console.WriteLine("Could not find a ROKU at this address.");
                    continue;
                }

                return roku;
            }
        }

        static void DeviceOperations(IRokuRemote roku)
        {
            Console.WriteLine($"You chose device id {roku.Info.Id}");


            while (true)
            {
                Console.WriteLine("1) Go to the home screen");
                Console.WriteLine("2) Launch an application");
                Console.WriteLine("3) Show the current active application");
                Console.WriteLine("4) Keypress");
                Console.WriteLine("5) Search");
                Console.WriteLine("6) Save App Icons");
                var choice = ParseChoice("Make a selection: ", 1, 6);
                if (choice == null)
                {
                    Console.WriteLine($"leaving {roku.Info.Id}");
                    break;
                }
                if (choice < 0)
                    continue;

                switch (choice.Value)
                {
                    case 1:
                        Console.WriteLine($"Sending {roku.Info.Id} to the home screen.");
                        roku.KeypressAsync(CommandKeys.Home).Wait();
                        break;

                    case 2:
                        LaunchApp(roku);
                        break;

                    case 3:
                        ShowActiveApp(roku);
                        break;

                    case 4:
                        HandleKeypress(roku);
                        break;

                    case 5:
                        Search(roku);
                        break;

                    case 6:
                        SaveAppIcons(roku);
                        break;

                    default:
                        break;
                }
                Console.WriteLine();
            }
        }

        static void HandleKeypress(IRokuRemote roku)
        {
            var controlKeys = new Dictionary<ConsoleKey, CommandKeys>
            {
                { ConsoleKey.B, CommandKeys.Back },
                { ConsoleKey.I, CommandKeys.Info },
                { ConsoleKey.Spacebar, CommandKeys.Select },

                { ConsoleKey.P, CommandKeys.Play },
                { ConsoleKey.D, CommandKeys.Fwd },
                { ConsoleKey.R, CommandKeys.Rev },
            };

            var altKeys = new Dictionary<ConsoleKey, CommandKeys>
            {
                { ConsoleKey.S, CommandKeys.Search },
                { ConsoleKey.R, CommandKeys.InstantReplay },
            };

            var commandKeys = new Dictionary<ConsoleKey, CommandKeys>
            {
                { ConsoleKey.Enter, CommandKeys.Select },
                { ConsoleKey.Home, CommandKeys.Home },
                { ConsoleKey.RightArrow, CommandKeys.Right },
                { ConsoleKey.DownArrow, CommandKeys.Down },
                { ConsoleKey.LeftArrow, CommandKeys.Left },
                { ConsoleKey.UpArrow, CommandKeys.Up },
                { ConsoleKey.Backspace, CommandKeys.Backspace },
                { ConsoleKey.MediaPlay, CommandKeys.Play },
                { ConsoleKey.MediaNext, CommandKeys.Fwd },
                { ConsoleKey.MediaPrevious, CommandKeys.Rev },
            };


            Console.Write("Enter keys: ");
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }

                CommandKeys? commandKey = null;

                if (keyInfo.Modifiers == ConsoleModifiers.Control)
                {
                    if (controlKeys.ContainsKey(keyInfo.Key))
                    {
                        commandKey = controlKeys[keyInfo.Key];
                    }
                }
                else if (keyInfo.Modifiers == ConsoleModifiers.Alt)
                {
                    if (altKeys.ContainsKey(keyInfo.Key))
                    {
                        commandKey = altKeys[keyInfo.Key];
                    }
                }
                else if (commandKeys.ContainsKey(keyInfo.Key))
                {
                    commandKey = commandKeys[keyInfo.Key];
                }

                Func<ICommandResponse> command;
                if (commandKey != null)
                    command = () => roku.KeypressAsync(commandKey.Value).Result;
                else
                    command = () => roku.KeypressAsync(keyInfo.KeyChar).Result;

                if (!command().IsSuccess)
                    Console.Write("-DEVICE ERROR");
            }
        }

        static int? ParseChoice(string text, int low, int high)
        {
            int choice = 0;
            Console.Write(text);
            var n = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(n))
                return null;

            if (!int.TryParse(n, out choice))
            {
                Console.WriteLine("Invalid choice.");
                return -1;
            }
            if (choice < low || choice > high)
            {
                Console.WriteLine("Invalid choice.");
                return -1;
            }
            return choice;
        }

        static void LaunchApp(IRokuRemote roku)
        {
            var selected = SelectApp(roku, "Choose an App: ");
            if (selected != null)
            {
                Console.WriteLine($"Launching {selected.Text}.");
                roku.LaunchAppAsync(selected.Id).Wait();
            }
        }

        static void ShowActiveApp(IRokuRemote roku)
        {
            var appInfo = roku.GetActiveAppAsync().Result;
            if (appInfo.App.Id != 0)
            {
                Console.WriteLine(appInfo.App.Text);
            }
            else if (appInfo.ScreenSaver == null)
            {
                Console.WriteLine("Home Screen");
            }
            else
            {
                Console.WriteLine($"Screen Saver: {appInfo.ScreenSaver.Text}");
            }
            Console.WriteLine();
        }


        static RokuApp SelectApp(IRokuRemote roku, string text)
        {
            var appList = roku.Apps
                 .Where(app => app.AppType.Equals("appl", StringComparison.OrdinalIgnoreCase))
                 .OrderBy(app => app.Text).ToArray();

            while (true)
            {
                int i = 0;
                foreach (var app in appList)
                {
                    i++;
                    Console.WriteLine($"{i}) {app.Text}");
                };
                var choice = ParseChoice(text, 1, appList.Length);
                if (choice == null)
                {
                    Console.WriteLine("Cancelled.");
                    return null;
                }
                if (choice < 0)
                    continue;

                var selected = appList[choice.Value - 1];
                return selected;
            }
        }

        static void Search(IRokuRemote roku)
        {
            RokuApp app = null;
            int? appId = null;
            SearchType? type = null;
            int? season = null;
            bool launch = false;

            Console.Write($"Search for: ");
            var keyword = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            type = SelectSearchType("(optional) search type: ");
            if (type != null && type.Value == SearchType.TVShow)
            {
                season = ParseChoice("(optional) season: ", 1, 99);
            }

            app = SelectApp(roku, "(optional) app to search in: ");


            Console.Write($"Searching for \"{keyword}\"");
            if (type != null)
            {
                Console.Write($" of type {type}");
                if (type.Value == SearchType.TVShow && season != null)
                    Console.Write($" season {season}");
            }

            if (app != null)
            {
                appId = app.Id;
                launch = true;
                Console.Write($" in {app.Text}");
            }
            roku.SearchAsync(keyword, type, season, appId, launch).Wait();
            Console.WriteLine();

        }

        static SearchType? SelectSearchType(string text)
        {
            var searchTypes = Enum.GetValues(typeof(SearchType)).Cast<SearchType>().ToArray();
            int i = 0;
            foreach (var item in searchTypes)
            {
                i++;
                Console.WriteLine($"{i}) {item}");
            }
            var choice = ParseChoice(text, 1, searchTypes.Length);
            if (choice == null)
                return null;

            return searchTypes[choice.Value - 1];
        }

        static void SaveAppIcons(IRokuRemote roku)
        {

            foreach (var app in roku.Apps)
            {
                var icon = roku.GetAppIconAsync(app.Id).Result;
                if (icon == null)
                {
                    Console.WriteLine("ERROR SAVING ICON");
                    return;
                }

                var fileName = app.Text + (icon.ContentType.Contains("jpeg") ? ".jpg" : ".png");
                using (var fs = File.Create(fileName))
                {
                    fs.Write(icon.Image, 0, icon.Image.Length);
                }
            }
            Console.WriteLine($"Saved {roku.Apps.Count} icons.");

        }
    }
}
