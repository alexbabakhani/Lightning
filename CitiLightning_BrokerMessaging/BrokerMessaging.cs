using System;
using Apache.NMS;
using Apache.NMS.Util;
using System.Threading;
using System.Xml;
using CitiLightningDatabase;
using System.Data.SqlClient;
using System.Globalization;

namespace Strategies.BrokerMessaging
{
    public class ExecuteTradeEventArgs : EventArgs
    {

        public Message Message { get; set; }
        public ExecuteTradeEventArgs(Message cond)
        {
            Message = cond;
        }

    }
    public class Message
    {
        public bool b;
        public int id;
        public double price;
        public int size;
        public string symbol;
        public DateTime dateTime;
        public bool Open;
        public double closeprice;
        public string Position;
        public DateTime closedate;
        public override string ToString()
        {
            var s = String.Format("{0:yyyy-MM-ddThh:mm:ss.fff-ff:ff}", dateTime);
            return "<trade><buy>" + b
                + "</buy><id>" + id + "</id><price>" + price + "</price><size>"
                + size + "</size><stock>" + symbol + "</stock><whenAsDate>"
                + s
                + "</whenAsDate></trade>";
        }
        public static Message ConvertToMessage(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList listofnodes = doc.GetElementsByTagName("trade");
            XmlNodeList childlistofnodes = listofnodes[0].ChildNodes;
            Message message = new Message();
            message.b = Convert.ToBoolean(childlistofnodes[0].InnerText);
            message.id = Convert.ToInt32(childlistofnodes[1].InnerText);
            message.price = Convert.ToDouble(childlistofnodes[2].InnerText);
            message.size = Convert.ToInt32(childlistofnodes[4].InnerText);
            message.symbol = childlistofnodes[5].InnerText;
            message.dateTime = Convert.ToDateTime(childlistofnodes[6].InnerText);
            return message;

        }
        public static Transaction ConvertToTransaction(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList listofnodes = doc.GetElementsByTagName("trade");
            XmlNodeList childlistofnodes = listofnodes[0].ChildNodes;
            Console.WriteLine("Here is each element of the node:");
            //int count = 0;
            //foreach (XmlNode node in childlistofnodes)
            //{
            //    count += 1;
            //    Console.WriteLine(node.LastChild.InnerText);
            //}
            /*
            for (int i = 0; i < childlistofnodes.Count; i++)
            {
                if (childlistofnodes[i].InnerText.Length > 0)
                {
                    Console.WriteLine(childlistofnodes[i].InnerText);
                }
            }
            */
            //Console.WriteLine(childlistofnodes[0].InnerText);  //bool
            //Console.WriteLine(childlistofnodes[1].InnerText);  //id
            //Console.WriteLine(childlistofnodes[2].InnerText);  //price
            // Console.WriteLine(childlistofnodes[3].InnerText);  //status
            //Console.WriteLine(childlistofnodes[4].InnerText);  //size
            //Console.WriteLine(childlistofnodes[5].InnerText); // symbol
            //Console.WriteLine(childlistofnodes[6].InnerText);  //date
            //AddTransaction(decimal price, string strategy, string symbol, DateTime timestamp, bool boolean, int size, string status = "N/A")
            /*
            CLDB c = new CLDB();
            Console.WriteLine("Adding transaction...");
            c.AddTransaction(Convert.ToDecimal(childlistofnodes[2].InnerText), "strategy", 
                childlistofnodes[5].InnerText, Convert.ToDateTime(childlistofnodes[6].InnerText), 
                Convert.ToBoolean(childlistofnodes[0].InnerText), 
                Convert.ToInt32(childlistofnodes[4].InnerText), childlistofnodes[3].InnerText);
            Console.WriteLine("Done");*/
            /*
            if (childlistofnodes[3].InnerText == "REJECTED")
            {
                Console.WriteLine("The line says REJECTED will now resend...");
                BrokerMessaging m = new BrokerMessaging();
                m.SendOrderToBroker();
            }*/
            var transaction = new Transaction();
            transaction.Buy = Convert.ToBoolean(childlistofnodes[0].InnerText);
            transaction.Id = Convert.ToInt32(childlistofnodes[1].InnerText);
            transaction.Price = Convert.ToDecimal(childlistofnodes[2].InnerText);
            transaction.Size = Convert.ToInt32(childlistofnodes[4].InnerText);
            transaction.Symbol = childlistofnodes[5].InnerText;
            transaction.Status = "N/A";
            transaction.Timestamp = Convert.ToDateTime(childlistofnodes[6].InnerText);
            return transaction;
        }
    }

    public class BrokerMessaging
    {
        CLDB Database = new CLDB();
        public event EventHandler ThresholdReached;

        protected virtual void OnThresholdReached(EventArgs e)
        {
            EventHandler handler = ThresholdReached;
            if (handler != null)
            {
                handler(this, e);
            }

        }
        protected static ITextMessage message = null;
        /*
        public static void Main(string[] args)
        {

            //SendOrderToBroker();
            //ListenForBrokerResponse();

            BrokerMessaging m = new BrokerMessaging();
            m.SendOrderToBroker();
            m.ListenForBrokerResponse();

            //var dt = DateTime.ParseExact("2014-07-31T22:33:22.801-04:00", "yyyy-MM-ddThh:mm:ss.fff-ff:ff", new CultureInfo("en-US"));
            //Console.WriteLine(dt);

            //CLDB c = new CLDB();
            //Console.WriteLine("Adding transaction...");
            //c.AddTransaction(2.5M, "strategy", "ll", DateTime.Now, true, 52, "hello");

            Console.ReadKey();
        }*/
        public void ListenForBrokerResponse()
        {
            Uri connecturi = new Uri("activemq:tcp://localhost:61616");
            Console.WriteLine("Listening For Broker Response");

            IConnectionFactory factory = new NMSConnectionFactory(connecturi);
            IConnection connection = factory.CreateConnection();
            ISession session = connection.CreateSession();
            IDestination destination = SessionUtil.GetDestination(session, "queue://OrderBroker_Reply");

            // Create a consumer and producer
            connection.Start();
            IMessageConsumer consumer = session.CreateConsumer(destination);
            consumer.Listener += new MessageListener(OnMessage);
            Console.WriteLine("Listening...");
        }
        public void OnMessage(IMessage receivedMsg)
        {
            var message = receivedMsg as ITextMessage;
            if (message == null)
            {
                Console.WriteLine("No message received!");
            }
            else
            {
                Console.WriteLine("Received message with text: " + message.Text);
            }
            Console.WriteLine(message.Text);
            Console.WriteLine("Converting to transaction");
            Message m = new Message();
            OnThresholdReached(new ExecuteTradeEventArgs(Message.ConvertToMessage(message.Text)));
            Transaction transaction = Message.ConvertToTransaction(message.Text);

            Database.AddTransaction(transaction);
        }

        public void SendOrderToBroker(Message m)
        //public void SendOrderToBroker()
        {
            Uri connecturi = new Uri("activemq:tcp://localhost:61616");
            Console.WriteLine("Connecting to " + connecturi + "For Sending Orders to Broker");

            IConnectionFactory factory = new NMSConnectionFactory(connecturi);

            using (IConnection connection = factory.CreateConnection())
            using (ISession session = connection.CreateSession())
            {
                IDestination destination = SessionUtil.GetDestination(session, "queue://OrderBroker");
                Console.WriteLine("Using destination: " + destination);

                // Create a consumer and producer
                //using (IMessageConsumer consumer = session.CreateConsumer(destination))
                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    connection.Start();
                    //producer.Persistent = true;

                    //working hardcoded request
                    //ITextMessage request = session.CreateTextMessage("<trade><buy>true</buy><id>1</id><price>88.0</price><size>2000</size><stock>HON</stock><whenAsDate>2014-07-31T22:33:22.801-04:00</whenAsDate></trade>");
                    ITextMessage request = session.CreateTextMessage(m.ToString());
                    request.NMSCorrelationID = "Testing Now";

                    producer.Send(request);
                    Console.WriteLine("Request Sent");
                }
            }
        }
        //public string CreateMessage(Message m)
        //{
        //    return "<trade><buy>" + b
        //        + "</buy></id>" + id + "</id><price>" + price + "</price><size>" 
        //        + size + "</size><stock>" + symbol + "</stock><whenAsDate>"
        //        + String.Format("yyyy-MM-ddThh:mm:ss.fff-ff:ff",dateTime) 
        //        + "</whenAsDate></trade>";

        //    //String.Format("YYYY-mm-dd ffffff", DateTime.Now);
        //}
    }
}