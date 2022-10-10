using Observer;

new Core();

//Ядро программы инициализирующей цикл
public class Core
{
    TG tg = new TG();
    Files files = new Files();
    Comands comands = new Comands();
    XMLs xml = new XMLs();
    string ID = "";
    int delay = 60000;
    public Core()
    {
        var conf = files.GetConfig();
        ID = conf.Item1;
        delay = conf.Item2;
        tg.chatID = conf.Item3;
        tg.token = conf.Item4;
        comands.CreateNewList();
        try
        {
            comands = xml.ReadXML();
        }
        catch (Exception)
        {
            xml.WriteXML(comands);
        }

        //tg.SendMessage(comands.CMListStr());
        while (true)
        {
            tg.tgcomlist.Clear();
            tg.GetUpdate();
            Thread.Sleep(delay);
            for (int i = 0; i < tg.tgcomlist.Count; i++)
            {
                if (tg.tgcomlist[i].Item1 == "/add")
                {
                    comands.AddCM(tg.tgcomlist[i].Item2, tg.tgcomlist[i].Item3);
                    xml.WriteXML(comands);
                    tg.SendMessage("Команда добавлена");
                }
                if (tg.tgcomlist[i].Item1 == "/list")
                {
                    tg.SendMessage(comands.CMListStr(ID));
                }
                if (tg.tgcomlist[i].Item1 == "/del")
                {
                    try
                    {
                        comands.DeleteCM(Convert.ToInt32(tg.tgcomlist[i].Item2));
                        xml.WriteXML(comands);
                        tg.SendMessage("Команда удалена");

                    }
                    catch (Exception)
                    {
                        if (tg.tgcomlist[i].Item2 == "all")
                        {
                            comands.comlist.Clear();
                            comands.namelist.Clear();
                            xml.WriteXML(comands);
                        }
                        else
                            tg.SendMessage("Неверный формат команды");
                    }

                }
            }
            for (int i = 0; i < comands.comlist.Count; i++)
            {
                files.writeFile(comands.comlist[i], ID);
            }
            files.GetRes(tg);
            Thread.Sleep(delay);
        }
    }
}