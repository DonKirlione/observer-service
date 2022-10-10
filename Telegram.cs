using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{
    //Взаимодействие с Телеграм
    class TG
    {
        //оффсет нужен для однократной обработки команды
        private long offset = 100000;
        public string chatID = ""; //default id ME
        private static string message = "Сообщение"; //default message
        private string method = "sendMessage?"; //getUpdates    getMe   sendMessage?
        public string token = "";
        private string url = "https://api.telegram.org";
        private string responseBody = "Answer";
        public List<Tuple<string, string, string>> tgcomlist = new List<Tuple<string, string, string>>();
        object tgjson = new
        {
            text = message,
        };

        //Пустой конструктор
        public TG()
        {

        }

        //Конструктор с вводом id чата
        public TG(string id)
        {
            chatID = id;
        }

        //Отправка сообщения msg JSONом
        public async void SendJsonMessage(string msg)
        {
            HttpClient client = new HttpClient();
            tgjson = new
            {
                text = msg
            };

            client.PostAsync("https://api.telegram.org/" + token + "/sendMessage?chat_id=" + chatID, new StringContent(JsonConvert.SerializeObject(tgjson), Encoding.UTF8, "application/json"));

            Console.WriteLine("Сообщение отправлено");
        }

        //Отправка сообщения msg курл
        public async void SendMessage(string msg)
        {
            message = msg;

            HttpClient client = new HttpClient();

            string additional = "chat_id=" + chatID + "&text=" + message;
            string requestUrl = url + "/" + token + "/" + method + additional;

            HttpResponseMessage response = await client.GetAsync(requestUrl);

            string responseBody = await response.Content.ReadAsStringAsync();

            //Console.WriteLine(response.StatusCode.ToString() + "\n");

            if (response.IsSuccessStatusCode)
            {
                //Console.WriteLine(responseBody);
            }
            else
            {
                // problems handling here
                Console.WriteLine(
                    "Ошибка, StatusCode: {0}",
                    response.StatusCode
                );
            }
            Console.WriteLine("Сообщение отправлено");
            //Console.ReadLine();
        }

        //Обновление бота
        public async void GetUpdate()
        {
            string com = "";
            int comcount = 0;

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url + "/" + token + "/" + "getUpdates?offset=" + offset);

            responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine(response.StatusCode.ToString() + "\n");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(responseBody);
                comcount = CountWords(responseBody, "\"text\"");

                for (int i = 0; i < comcount; i++)
                {
                    // Console.WriteLine("Количество команд: " + CountWords(responseBody, "\"text\""));
                    tgcomlist.Add(WhatACom(GetCom(responseBody)));
                    GetOffset(responseBody);
                    responseBody = responseBody.Substring(responseBody.IndexOf("\"text\":") + 6);
                }
            }
            else
            {
                // problems handling here
                Console.WriteLine(
                    "Ошибка, StatusCode: {0}",
                    response.StatusCode
                );
            }
            //Console.ReadLine();
        }
        
        //Получить последний offset
        public void GetOffset(string body)
        {
            int ib = 0;
            int ie = 0;
            string offsetstr = "";
            ib = body.IndexOf("\"update_id\":") + 12;
            int k = ib;
            int lengh = 0;
            while ((body[k].ToString() != ","))
            {
                k++;
                lengh++;
            }
            offsetstr = body.Substring(ib, lengh);
            Console.WriteLine("get offset: " + offsetstr);
            offset = Convert.ToInt64(offsetstr) + 1;
        }
        
        //Получить команду
        public string GetCom(string body)
        {
            int ib = 0;
            int ie = 0;
            string com = "";
            ib = body.IndexOf("\"text\":") + 8;
            int k = ib;
            int lengh = 0;
            while ((body[k].ToString() != "\""))
            {
                if (body[k + 1].ToString() == "\"")
                {
                    if (body[k].ToString() == "\\")
                    {
                        k++;
                        lengh++;
                    }
                }
                k++;
                lengh++;
            }
            com = body.Substring(ib, lengh);
            //Console.WriteLine(com);
            return com;
        }

        //Количество в строке подстрок
        private int CountWords(string s, string s0)
        {
            int count = (s.Length - s.Replace(s0, "").Length) / s0.Length;
            return count;
        }

        //Разбор команды из ТГ
        private Tuple<string, string, string> WhatACom(string com)
        {
            string tgcom = "";
            string comname = "";
            string comtxt = "";
            string temp = "";
            if (com != "/list")
            {
                try
                {
                    tgcom = com.Substring(0, com.IndexOf(" "));
                    temp = com.Substring(tgcom.Length + 1);
                    if (tgcom == "/del")
                    {
                        comname = temp;
                    }
                    else
                    {
                        comname = temp.Substring(0, temp.IndexOf(" "));
                        temp = temp.Substring(comname.Length + 1);

                        comtxt = temp;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid command");
                }
            }
            else tgcom = com;
            Console.WriteLine("tgcom - " + tgcom + ".");
            Console.WriteLine("comname - " + comname + ".");
            Console.WriteLine("comtxt - " + comtxt + ".");
            var Com = Tuple.Create(tgcom, comname, comtxt);
            return Com;
        }
    }
}
