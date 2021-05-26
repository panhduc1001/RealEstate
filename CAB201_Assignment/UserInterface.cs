using System;
using System.Collections.Generic;

namespace CAB201_Assignment
{

    // UserInterface class sourced from CAB201 assignment files. Some modifications have been done to fit my application better
    // - DisplayList method - added option to use custom error string if the list is empty. 
    // - GetInput method - ensured non-empty input
    public static class UserInterface
    {
        public static T ChooseFromList<T>(IList<T> list)
        {
            if (!(list.Count > 0)) { UserInterface.Error("No properties available at the current time"); return default(T); }
            DisplayList("Please choose one of the following:", list);
            var option = UserInterface.getOption(1, list.Count);
            return list[option];
        }

        public static void DisplayList<T>(string title, IList<T> list, string failMsg = "None")
        {
            Console.WriteLine(title);
            if (list.Count == 0)
                Console.WriteLine($"  {failMsg}");
            else
                for (int i = 0; i < list.Count; i++)
                    Console.WriteLine("  {0}) {1}", i + 1, list[i].ToString());

            Console.WriteLine();
        }

        public static int getOption(int min, int max)
        {
            while (true)
            {
                var key = Console.ReadKey(true);
                var option = key.KeyChar - '0';
                if (min <= option && option <= max)
                {
                    return option - 1;
                }
                else
                {
                    Message("Invalid option");
                }
            }
        }

        public static string GetInput(string prompt)
        {
            Console.Write("{0}: ", prompt);
            string input = Console.ReadLine();
            while (String.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine($"Please enter a valid value for {prompt}");
                input = Console.ReadLine();
            }
            return input;
        }

        public static int GetInteger(string prompt)
        {
            while (true)
            {
                var response = UserInterface.GetInput(prompt);
                int integer;
                if (int.TryParse(response, out integer))
                    return integer;
                else
                    Error("Invalid number");
            }
        }

        public static string GetPassword(string prompt)
        {
            Console.Write("{0}: ", prompt);
            var password = new System.Text.StringBuilder();
            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);
                var key = keyInfo.Key;

                if (key == ConsoleKey.Enter)
                    break;
                else if (key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        Console.Write("\b \b");
                        password.Remove(password.Length - 1, 1);
                    }
                }
                else
                {
                    Console.Write("*");
                    password.Append(keyInfo.KeyChar);
                }
            }
            Console.WriteLine();
            return password.ToString();
        }

        public static void Error(string msg)
        {
            Console.WriteLine($"{msg}, please try again");
            Console.WriteLine();
        }

        public static void Message(object msg)
        {
            Console.WriteLine(msg);
            Console.WriteLine();
        }
    }


    public class Menu
    {
        class MenuItem
        {
            private string item;
            private Action selected;

            public MenuItem(string item, Action eventHandler)
            {
                this.item = item;
                selected = eventHandler;
            }

            public void select()
            {
                selected();
            }

            public override string ToString()
            {
                return item;
            }
        }

        private List<MenuItem> items = new List<MenuItem>();

        public void Add(string menuItem, Action eventHandler)
        {
            items.Add(new MenuItem(menuItem, eventHandler));
        }

        public void Display()
        {
            UserInterface.DisplayList("Please select one of the following:", items);
            var option = UserInterface.getOption(1, items.Count);
            items[option].select();
        }
    }
}
