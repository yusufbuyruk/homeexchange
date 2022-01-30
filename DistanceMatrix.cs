using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;

namespace ConsoleApp3
{
    class DistanceMatrix
    {
        public string Destinations { get; set; }
        public string Origins { get; set; }
        public string APIKey { get; set; }


        public DistanceMatrix()
        {
        }

        public void Init()
        {
            List<string> destinations = new List<string>();
            StringBuilder sb = new StringBuilder();
            string line = string.Empty;

            using (StreamReader sr = new StreamReader("destinations_database.txt"))
            {
                while ((line = sr.ReadLine()) != null)
                    destinations.Add(line);
            }

            using (StreamReader sr = new StreamReader("origins_database.txt"))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                    WebClient client = new WebClient();
                    client.QueryString.Add("key", APIKey); // TODO: &key=YOUR_API_KEY
                    client.QueryString.Add("units", "metric");
                    client.QueryString.Add("language", "tr");
                    client.QueryString.Add("mode", "transit"); // transit_mode | transit_routing_preferences
                    client.QueryString.Add("traffic_model", "best_guess"); // traffic_model | best_guess, pessimistic, optimistic
                    client.QueryString.Add("destinations", String.Join("|", destinations));
                    client.QueryString.Add("origins", line);
                    // client.QueryString.Add("departure_time" || "arrival_time", UTC 1970)

                    Stream response = client.OpenRead("https://maps.googleapis.com/maps/api/distancematrix/xml");
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(response);
                    XmlNodeList durations = xmlDoc.SelectNodes("//duration/value");

                    foreach (XmlNode duration in durations)
                        sb.Append(String.Format("{0} ", duration.InnerText));
                    sb.AppendLine();
                    Console.WriteLine(line);
                }

            }

            using (StreamWriter sw = new StreamWriter("distance_matrix.txt", append: false))
                sw.WriteLine(sb.ToString());

            Console.ReadKey();
        }
    }
}
