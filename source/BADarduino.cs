using System;
using System.IO;
using System.Text;
using Console = Colorful.Console;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Reflection;

//Written by Josh Bruegger.  Github: https://github.com/josh0196/BADarduino
namespace BADARDUINO
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"input.txt");
            if (!File.Exists(input))
            {
                Console.WriteAscii("ERROR!", Color.Red);
                Console.WriteLine("No input.txt file found.", Color.Red);
                Console.WriteLine("Press <ENTER> to exit...");
                Console.ReadLine();
                return;
            }
            string output = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"output.ino");
            
            Console.WriteAscii("BADarduino");
            Console.WriteLine("BADencoder V 00.01");
            Console.WriteLine("");
            Console.WriteLine("WARNING: BADarduino doesn't support the following commands:", Color.Red);
            Console.WriteLine("REPLAY", Color.Red);
            Console.WriteLine("SCROLLLOCK", Color.Red);
            Console.WriteLine("PRINTSCREEN", Color.Red);
            Console.WriteLine("");

            FormatOutput("START", output);
            
            var FileStream = new FileStream(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"input.txt"), FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(FileStream,Encoding.UTF8))
            {
                string line;
                int linen = 1;
                while ((line = streamReader.ReadLine())!=null)
                {
                    ProcessLine(line, output);
                    Console.WriteLine("Line " + linen + " processed...");
                    linen++;
                }
            }
            
            FormatOutput("END", output);

            Console.WriteLine("Done.");
            Console.WriteLine("Press <ENTER> to exit...");
            Console.ReadLine();
        }

        private static void ProcessLine(string line, string output)
        {
            if (line == "" || line == " " || line == null) return;

            String[] words = line.Split(' ');

            //Console.WriteLine("Line: " + line + "   Command: " + words[0]);
            var rgx = new Regex(words[0] + " ");
            switch (words[0])
            {
                case "REM":
                    File.AppendAllText(output, "//" + rgx.Replace(line, "", 1) + Environment.NewLine);
                    break;
                case "DEFAULT_DELAY":
                case "DEFAULTDELAY":
                    File.AppendAllText(output, "DEFAULT_DELAY=" + words[1] + ";"+Environment.NewLine);
                    break;
                case "DELAY":
                    File.AppendAllText(output, "delay(" + words[1] + ");"+Environment.NewLine);
                    break;
                case "STRING":
                    File.AppendAllText(output, "Keyboard.print(\"" + rgx.Replace(line, "", 1) + "\");"+Environment.NewLine);
                    break;
                case "WINDOWS":
                case "GUI":
                    if (words.Length>1)
                        File.AppendAllText(output, "Keyboard.press(KEY_LEFT_GUI); Keyboard.press('" + words[1] + "');\ndelay(100); Keyboard.releaseAll();"+Environment.NewLine);
                    else
                        File.AppendAllText(output, "Keyboard.write(KEY_LEFT_GUI);"+Environment.NewLine);
                    break;
                case "MENU":
                case "APP":
                    File.AppendAllText(output, "Keyboard.press(KEY_LEFT_SHIFT); Keyboard.press(KEY_F10);\ndelay(100); Keyboard.releaseAll();"+Environment.NewLine);
                    break;
                case "SHIFT":
                    if (words[1] != null && words[1] != "")
                    {
                        File.AppendAllText(output, "Keyboard.press(KEY_LEFT_SHIFT); Keyboard.press(");
                        switch (words[1])
                        {
                            case "DELETE":
                                File.AppendAllText(output, "KEY_DELETE);"+Environment.NewLine);
                                break;
                            case "HOME":
                                File.AppendAllText(output, "KEY_HOME);"+Environment.NewLine);
                                break;
                            case "INSERT":
                                File.AppendAllText(output, "KEY_INSERT);"+Environment.NewLine);
                                break;
                            case "PAGEUP":
                                File.AppendAllText(output, "KEY_PAGE_UP);"+Environment.NewLine);
                                break;
                            case "PAGEDOWN":
                                File.AppendAllText(output, "KEY_PAGE_DOWN);"+Environment.NewLine);
                                break;
                            case "WINDOWS":
                                File.AppendAllText(output, "KEY_LEFT_GUI);"+Environment.NewLine);
                                break;
                            case "GUI":
                                File.AppendAllText(output, "KEY_LEFT_GUI);"+Environment.NewLine);
                                break;
                            case "UPARROW":
                                File.AppendAllText(output, "KEY_UP_ARROW);"+Environment.NewLine);
                                break;
                            case "DOWNARROW":
                                File.AppendAllText(output, "KEY_DOWN_ARROW);"+Environment.NewLine);
                                break;
                            case "LEFTARROW":
                                File.AppendAllText(output, "KEY_LEFT_ARROW);"+Environment.NewLine);
                                break;
                            case "RIGHTARROW":
                                File.AppendAllText(output, "KEY_RIGHT_ARROW);"+Environment.NewLine);
                                break;
                            case "TAB":
                                File.AppendAllText(output, "KEY_TAB);"+Environment.NewLine);
                                break;
                        }
                        File.AppendAllText(output, "Keyboard.releaseAll();"+Environment.NewLine);
                    }
                    else
                        File.AppendAllText(output, "Keyboard.write(KEY_LEFT_SHIFT);"+Environment.NewLine);
                    break;
                case "ALT":
                    if (words[1] != null && words[1] != "")
                    {
                        File.AppendAllText(output, "Keyboard.press(KEY_RIGHT_ALT); Keyboard.press(");
                        switch (words[1])
                        {
                            case "END":
                                File.AppendAllText(output, "KEY_END);"+Environment.NewLine);
                                break;
                            case "ESC":
                            case "ESCAPE":
                                File.AppendAllText(output, "KEY_ESC);"+Environment.NewLine);
                                break;
                            case "F1":
                                File.AppendAllText(output, "KEY_F1);"+Environment.NewLine);
                                break;
                            case "F2":
                                File.AppendAllText(output, "KEY_F2);"+Environment.NewLine);
                                break;
                            case "F3":
                                File.AppendAllText(output, "KEY_F3);"+Environment.NewLine);
                                break;
                            case "F4":
                                File.AppendAllText(output, "KEY_F4);"+Environment.NewLine);
                                break;
                            case "F5":
                                File.AppendAllText(output, "KEY_F5);"+Environment.NewLine);
                                break;
                            case "F6":
                                File.AppendAllText(output, "KEY_F6);"+Environment.NewLine);
                                break;
                            case "F7":
                                File.AppendAllText(output, "KEY_F7);"+Environment.NewLine);
                                break;
                            case "F8":
                                File.AppendAllText(output, "KEY_F8);"+Environment.NewLine);
                                break;
                            case "F9":
                                File.AppendAllText(output, "KEY_F9);"+Environment.NewLine);
                                break;
                            case "F10":
                                File.AppendAllText(output, "KEY_F10);"+Environment.NewLine);
                                break;
                            case "F11":
                                File.AppendAllText(output, "KEY_F11);"+Environment.NewLine);
                                break;
                            case "F12":
                                File.AppendAllText(output, "KEY_F12);"+Environment.NewLine);
                                break;
                            case "TAB":
                                File.AppendAllText(output, "KEY_TAB);"+Environment.NewLine);
                                break;
                            case "SPACE":
                                File.AppendAllText(output, "' ');"+Environment.NewLine);
                                break;
                            default:
                                File.AppendAllText(output, "'" + words[1] + "');"+Environment.NewLine);
                                break;
                        }
                        File.AppendAllText(output, "delay(100); Keyboard.releaseAll();"+Environment.NewLine);
                    }
                    else
                        File.AppendAllText(output, "Keyboard.write(KEY_RIGHT_ALT);"+Environment.NewLine);
                    break;
                case "CONTROL":
                case "CTRL":
                    if (words[1] != null && words[1] != "")
                    {
                        File.AppendAllText(output, "Keyboard.press(KEY_RIGHT_CTRL); Keyboard.press(");
                        switch (words[1])
                        {
                            case "BREAK":
                            case "PAUSE":
                                File.AppendAllText(output, "KEY_PAUSE);"+Environment.NewLine);
                                break;
                            case "ESC":
                            case "ESCAPE":
                                File.AppendAllText(output, "KEY_ESC);"+Environment.NewLine);
                                break;
                            case "F1":
                                File.AppendAllText(output, "KEY_F1);"+Environment.NewLine);
                                break;
                            case "F2":
                                File.AppendAllText(output, "KEY_F2);"+Environment.NewLine);
                                break;
                            case "F3":
                                File.AppendAllText(output, "KEY_F3);"+Environment.NewLine);
                                break;
                            case "F4":
                                File.AppendAllText(output, "KEY_F4);"+Environment.NewLine);
                                break;
                            case "F5":
                                File.AppendAllText(output, "KEY_F5);"+Environment.NewLine);
                                break;
                            case "F6":
                                File.AppendAllText(output, "KEY_F6);"+Environment.NewLine);
                                break;
                            case "F7":
                                File.AppendAllText(output, "KEY_F7);"+Environment.NewLine);
                                break;
                            case "F8":
                                File.AppendAllText(output, "KEY_F8);"+Environment.NewLine);
                                break;
                            case "F9":
                                File.AppendAllText(output, "KEY_F9);"+Environment.NewLine);
                                break;
                            case "F10":
                                File.AppendAllText(output, "KEY_F10);"+Environment.NewLine);
                                break;
                            case "F11":
                                File.AppendAllText(output, "KEY_F11);"+Environment.NewLine);
                                break;
                            case "F12":
                                File.AppendAllText(output, "KEY_F12);"+Environment.NewLine);
                                break;
                            default:
                                File.AppendAllText(output, "'" + words[1] + "');"+Environment.NewLine);
                                break;
                        }
                        File.AppendAllText(output, "delay(100); Keyboard.releaseAll();"+Environment.NewLine);
                    }
                    else
                        File.AppendAllText(output, "Keyboard.write(KEY_RIGHT_CTRL);"+Environment.NewLine);
                    break;
                case "UPARROW":
                case "UP":
                    File.AppendAllText(output, "Keyboard.write(KEY_UP_ARROW);"+Environment.NewLine);
                    break;
                case "DOWN":
                case "DOWNARROW":
                    File.AppendAllText(output, "Keyboard.write(KEY_DOWN_ARROW);"+Environment.NewLine);
                    break;
                case "LEFT":
                case "LEFTARROW":
                    File.AppendAllText(output, "Keyboard.write(KEY_LEFT_ARROW);"+Environment.NewLine);
                    break;
                case "RIGHT":
                case "RIGHTARROW":
                    File.AppendAllText(output, "Keyboard.write(KEY_RIGHT_ARROW);"+Environment.NewLine);
                    break;
                case "BREAK":
                case "PAUSE":
                    File.AppendAllText(output, "Keyboard.write(KEY_PAUSE);"+Environment.NewLine);
                    break;
                case "DELETE":
                    File.AppendAllText(output, "Keyboard.write(KEY_DELETE);"+Environment.NewLine);
                    break;
                case "END":
                    File.AppendAllText(output, "Keyboard.write(KEY_END);"+Environment.NewLine);
                    break;
                case "ESC":
                case "ESCAPE":
                    File.AppendAllText(output, "Keyboard.write(KEY_ESC);"+Environment.NewLine);
                    break;
                case "HOME":
                    File.AppendAllText(output, "Keyboard.write(KEY_HOME);"+Environment.NewLine);
                    break;
                case "INSERT":
                    File.AppendAllText(output, "Keyboard.write(KEY_INSERT);"+Environment.NewLine);
                    break;
                case "CAPSLOCK":
                    File.AppendAllText(output, "Keyboard.write(KEY_CAPS_LOCK);"+Environment.NewLine);
                    break;
                case "PAGEUP":
                    File.AppendAllText(output, "Keyboard.write(KEY_PAGE_UP);"+Environment.NewLine);
                    break;
                case "PAGEDOWN":
                    File.AppendAllText(output, "Keyboard.write(KEY_PAGE_DOWN);"+Environment.NewLine);
                    break;
                case "SPACE":
                    File.AppendAllText(output, "Keyboard.write(' ');"+Environment.NewLine);
                    break;
                case "TAB":
                    File.AppendAllText(output, "Keyboard.write(KEY_TAB);"+Environment.NewLine);
                    break;
                case "ENTER":
                    File.AppendAllText(output, "Keyboard.write(KEY_RETURN);" + Environment.NewLine);
                    break;
            }
        }

        private static void FormatOutput(string v, string output)
        {
            String[] startLines = { "//Script generated using BADarduino by JoshBruegger, GitHub: https://github.com/josh0196/BADarduino", "#include \"Keyboard.h\"", "#define KEY_PAUSE (76+136)", "void setup(){", "Keyboard.begin();", "delay(1000);", "}", "void loop(){", "int DEFAULT_DELAY=100;", "delay(1000);" };
            String[] endLines = { "while (true);", "}" };

            if (v == "START")
                File.WriteAllLines(output, startLines);
            else if (v == "END") 
                File.AppendAllLines(output, endLines);
        }
    }
}
