namespace Observer
{
    //Работа с XML
    class XMLs
    {
        public Comands ReadXML()
        {
            // Now we can read the serialized book ...  
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(Comands));
            StreamReader file = new StreamReader(
                "Coms.xml");
            Comands coms = (Comands)reader.Deserialize(file);
            file.Close();
            return coms;
        }
        public void WriteXML(Comands comands)
        {
            // First write something so that there is something to read ...  
            var b = comands;
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(Comands));
            var wfile = new System.IO.StreamWriter("Coms.xml");
            writer.Serialize(wfile, b);
            wfile.Close();
        }
    }
}
