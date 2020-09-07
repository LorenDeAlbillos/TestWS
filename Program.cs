using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace testWS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**************************************");
            Console.WriteLine("SamPostAlarmas V.0005");
            Console.WriteLine("**************************************");

            Console.WriteLine("**************************************");
            Console.WriteLine("PETICION DE VERSION");
            Console.WriteLine("**************************************");
            //SamGetVersion();

            Console.WriteLine("**************************************");
            Console.WriteLine("ENVIO DE ALARMAS");
            Console.WriteLine("**************************************");
            SamPostAlarmas8088();


            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
 



        }

        private static string SerializeXml(mensaje theMensaje)
        {
            XmlSerializer ser = new XmlSerializer(typeof(mensaje));
            // Using a MemoryStream to store the serialized string as a byte array, 
            // which is "encoding-agnostic"
            using (MemoryStream ms = new MemoryStream())
            // Few options here, but remember to use a signature that allows you to 
            // specify the encoding  
            using (XmlTextWriter tw = new XmlTextWriter(ms, Encoding.UTF8))
            {
                tw.Formatting = System.Xml.Formatting.Indented;
                ser.Serialize(tw, theMensaje);
                // Now we get the serialized data as a string in the desired encoding

                String rpta= Encoding.UTF8.GetString(ms.ToArray());

                string path = @"d:\alertasSiam.txt";

                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    //string createText = "Hello and Welcome" + Environment.NewLine;
                    File.WriteAllText(path, rpta);
                }
                return rpta;

            }
        }

        public static String getXmlFile()
        {
            string path = @"c:\alertasSiam.txt";
            string readText = File.ReadAllText(path);
            return readText;

        }

        public static mensaje getMensaje()
        {
            Sis_informador theSis_informador = new Sis_informador("03107", "OK", "1", "2019_01_18 11_53_55");

            List<Sis_alerta> theListSis_alerta = new List<Sis_alerta>();

            Sis_alerta theSis_alerta = new Sis_alerta("BAAMONDE", "1");
            theSis_alerta.listAlertas.Add(new Alerta("51", "eth1 conexion eliminada", "2019_01_21 16_43_34"));
            theListSis_alerta.Add(theSis_alerta);
            mensaje theMensaje = new mensaje(theSis_informador);
            theMensaje.sis_alerta = theListSis_alerta;

            return theMensaje;
        }

      public static String getXmlPost()
        {
            Sis_informador theSis_informador = new Sis_informador("03107", "OK", "1", "2019_01_18 11_53_55");

            List<Sis_alerta> theListSis_alerta = new List<Sis_alerta>();

            Sis_alerta theSis_alerta = new Sis_alerta("BAAMONDE", "2");
            theSis_alerta.listAlertas.Add(new Alerta("51", "eth1 conexion eliminada", "2019_01_21 16_43_34"));
            //theSis_alerta.listAlertas.Add(new Alerta("52", "eth2 conexion eliminada", "2012_01_21 16_43_34"));
            theListSis_alerta.Add(theSis_alerta);

          /*
            Sis_alerta theSis_alerta1 = new Sis_alerta("BAAMONDE", "1");
            theSis_alerta1.listAlertas.Add(new Alerta("51", "eth1 conexion eliminada", "2019_01_21 16_43_34"));
            theSis_alerta1.listAlertas.Add(new Alerta("52", "eth2 conexion eliminada", "2012_01_21 16_43_34"));
            theListSis_alerta.Add(theSis_alerta1);
           */

            mensaje theMensaje = new mensaje(theSis_informador);
            theMensaje.sis_alerta=theListSis_alerta;



            XmlSerializer xmlSerializer = new XmlSerializer(theMensaje.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, theMensaje);
                return textWriter.ToString();
            }

        }

        public static void SamPostAlarmas8088()
        {
            WebResponse response = null;

            try
            {
                // Create a request using a URL that can receive a post. 
                //WebRequest request = (HttpWebRequest)WebRequest.Create("http://43.68.8.26:8088/main/system/webdev/xml/xml/");
                //WebRequest request = (HttpWebRequest)WebRequest.Create("https://postman-echo.com/post");
                WebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:1234/main/system/webdev/xml/xml/");

                //request.Headers.Add("Authorization: OAuth");
                // Set the Method property of the request to POST.  
                request.Method = "POST";
                ((HttpWebRequest)request).Accept = "*.*";

                // Set the ContentType property of the WebRequest.  
                //request.ContentType = "application/xml";
                request.ContentType = "text/plain";

                // Create POST data and convert it to a byte array.  
                //string postData = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?> <mensaje> <sis_informador> <id_sis_informador>03107</id_sis_informador> <estado>OK</estado><num_sistemas>1</num_sistemas><time_generacion>2019_01_18 11_53_55</time_generacion></sis_informador><sis_alerta><id_sis_alerta>BAAMONDE</id_sis_alerta><num_alertas>0</num_alertas></sis_alerta></mensaje>";
                //string postData = getXmlPost();
                //string postData = SerializeXml(getMensaje());
                string postData = getXmlFile();

                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentLength property of the WebRequest.  
                request.ContentLength = byteArray.Length;

                request.Headers.Add("TECNOLOGO", "ALSTOM");
                request.Headers.Add("ID-SIS-INFORMADOR", "03107");

                // Get the request stream.  
                Stream dataStreamWrite = request.GetRequestStream();
                // Write the data to the request stream.  
                dataStreamWrite.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.  
                dataStreamWrite.Close();

                //Response.ContentType = "application/xml";
                //((HttpWebRequest)request).ContentEncoding = System.Text.Encoding.UTF8;
                //Response.Output.Write(xml);

                // Get the response.  
                response = request.GetResponse();

                // Display the status.  
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                Stream dataStreamRead = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStreamRead);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);

                RespPost respPost = JsonConvert.DeserializeObject<RespPost>(responseFromServer);

                Console.WriteLine(respPost.messages +"  --   "+respPost.status);
                // Clean up the streams.  
                reader.Close();
                dataStreamRead.Close();
                response.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine("ERROR:" + e.Status.ToString()+"--"+ e.Message);
                if (e.Response != null)
                {
                    Console.WriteLine(((HttpWebResponse)e.Response).StatusCode.ToString());
                    Console.WriteLine(((HttpWebResponse)e.Response).StatusDescription.ToString());
                }
            }
        }




        public static void SamPostAlarmas8000()
        {
            WebResponse response = null;

            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = (HttpWebRequest)WebRequest.Create("http://43.68.8.26:8000/Siam/api/xml/");

                //request.Headers.Add("Authorization: OAuth");
                // Set the Method property of the request to POST.  
                request.Method = "POST";

                // Set the ContentType property of the WebRequest.  
                request.ContentType = "application/xml";

                // Create POST data and convert it to a byte array.  
                //string postData = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?> <mensaje> <sis_informador> <id_sis_informador>03107</id_sis_informador> <estado>OK</estado><num_sistemas>1</num_sistemas><time_generacion>2019_01_18 11_53_55</time_generacion></sis_informador><sis_alerta><id_sis_alerta>BAAMONDE</id_sis_alerta><num_alertas>0</num_alertas></sis_alerta></mensaje>";

                string postData = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?> <mensaje> <sis_informador> <id_sis_informador>03107</id_sis_informador> <estado>OK</estado><num_sistemas>1</num_sistemas><time_generacion>2019_01_18 11_53_55</time_generacion></sis_informador><sis_alerta><id_sis_alerta>BAAMONDE</id_sis_alerta><num_alertas>1</num_alertas><alerta><id_alerta>51</id_alerta><desc_alerta>Eth1 conexion eliminada</desc_alerta><timestamp>2019_01_21 16_43_34</timestamp></alerta></sis_alerta></mensaje>";
                //string postData = "prueba de envio de datos";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentLength property of the WebRequest.  
                request.ContentLength = byteArray.Length;

                request.Headers.Add("TECNOLOGO", "ALSTOM");
                request.Headers.Add("ID-SIS-INFORMADOR", "03107");

                // Get the request stream.  
                Stream dataStreamWrite = request.GetRequestStream();
                // Write the data to the request stream.  
                dataStreamWrite.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.  
                dataStreamWrite.Close();


                Console.WriteLine("paso1");

                // Get the response.  
                response = request.GetResponse();

                Console.WriteLine("paso2");

                // Display the status.  
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                Stream dataStreamRead = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStreamRead);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);
                // Clean up the streams.  
                reader.Close();
                dataStreamRead.Close();
                response.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine("ERROR:" + e.Message);
                Console.WriteLine(((HttpWebResponse)e.Response).StatusCode.ToString());
                Console.WriteLine(((HttpWebResponse)e.Response).StatusDescription.ToString());

            }
        }



        public static void SamGetVersion()
        {
            try
            {
                //var request = (HttpWebRequest)WebRequest.Create("http://43.68.8.26:8088/main/system/webdev/xml/xml/?id=03107");
                //var request = (HttpWebRequest)WebRequest.Create("http://43.68.8.26:8000/Siam/api/inf/?id=03001");
                var request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:1234/main/system/webdev/xml/xml/?id=03107");

                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "Accept-Encoding";

                // var response = (HttpWebResponse)request.GetResponse();
                var response = request.GetResponse();


                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseString = reader.ReadToEnd();
                //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                // Display the content.
                Console.WriteLine("**************************************");
                Console.WriteLine(responseString);
                Console.WriteLine("**************************************");
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                RespGet respGet = JsonConvert.DeserializeObject<RespGet>(responseString);

                Console.WriteLine(respGet.envio+"   -- "+respGet.id);


                dataStream.Close();
                reader.Close();
                response.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Status.ToString() + "--"+ e.Message);
            }

        }

 
        public static void testGetConParametros()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://postman-echo.com/cookies/set?foo1=bar1&foo2=bar2");

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "Accept-Encoding";

            request.Headers.Add("set-cookie", "sails.sid=s%3AHs9KLfpekGm8oAXQqTiwRNKYKGZx10Ac.BPPSKHi3dyXS9bFTAjLxiW1UC%2Bnlsj7dO%2FDzVTC0FSY; Path=/; HttpOnly");


            // var response = (HttpWebResponse)request.GetResponse();
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Status.ToString());
                String a = ((HttpWebResponse)e.Response).StatusCode.ToString();


            }

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseString = reader.ReadToEnd();
            //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Display the content.
            Console.WriteLine("**************************************");
            Console.WriteLine(responseString);
            Console.WriteLine("**************************************");
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            dataStream.Close();
            reader.Close();
            response.Close();

        }

        public static void TestPost()
        {

            try
            {
                // Create a request using a URL that can receive a post.   
                WebRequest request = WebRequest.Create("https://postman-echo.com/post");

                //request.Headers.Add("Authorization: OAuth");
                // Set the Method property of the request to POST.  
                request.Method = "POST";

                // request.Headers.Add("TECNOLOGO", "ALSTOM");
                request.Headers.Add("IDSISTEMA", "SISTEMA");

                // Create POST data and convert it to a byte array.  
                string postData = "Duis posuere augue vel cursus pharetra. In luctus a ex nec pretium. Praesent neque quam, tincidunt nec leo eget, rutrum vehicula magna";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.  
                request.ContentType = "text/plain";

                // Set the ContentLength property of the WebRequest.  
                request.ContentLength = byteArray.Length;
                // Get the request stream.  
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.  
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.  
                dataStream.Close();
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);
                // Clean up the streams.  
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
