using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{
    //Работа с командами на ПК
    public class Comands
    {
        public List<string> namelist = new List<string>();
        public List<string> comlist = new List<string>();

        //Новый список команд
        public void CreateNewList()
        {
            namelist.Clear();
            comlist.Clear();
            Console.WriteLine("Новый список создан");
        }

        //Проверка названия команды
        public bool CheckCMName(string name)
        {
            if (namelist.Contains(name))
            {
                return false;
            }
            else return true;
        }

        //Вернуть списки названий и команд
        public Tuple<List<string>, List<string>> CMList()
        {
            var CMlist = Tuple.Create(namelist, comlist);
            Console.WriteLine("Список команд. [Id] <Название>: <Команда>");

            for (int i = 0; i < namelist.Count; i++)
            {
                Console.Write("[" + i + "]" + namelist[i] + ": ");
                Console.WriteLine(comlist[i]);
            }
            return CMlist;
        }

        //Добавление команды в список и в файл
        public void AddCM(string name, string com)
        {
            if (CheckCMName(name))
            {
                namelist.Add(name);
                comlist.Add(com);
                Console.WriteLine("Команда добавлена.");
            }
            else Console.WriteLine("Такая команда уже есть в списке.");
        }

        //Удаление команды из списка
        public void DeleteCM(int id)
        {
            namelist.RemoveAt(id);
            comlist.RemoveAt(id);
        }

        //Вернуть команду по id
        public string GetCM(int id)
        {
            return comlist[id];
        }

        //Отправить список команд
        public string CMListStr(string ID)
        {
            string msg = "id <" + ID + "> СПИСОК КОМАНД\n";

            for (int i = 0; i < namelist.Count; i++)
            {
                msg = msg + ("[" + i + "]" + namelist[i] + ": " + comlist[i] + "\n");
            }

            return msg;
        }
    }
}
