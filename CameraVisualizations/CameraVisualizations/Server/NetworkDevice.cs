using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CameraVisualizations.Server
{
    public class NetworkDevice
    {
        //public static void Main()
        //{
        //    var i = new NetworkDevice();
        //    //i.CreateHostDirectory("10.6.6.124");
        //    i.GetPhotosFromDevice("10.6.6.124");
        //    //i.SendImage("10.6.6.124", "test.jpg");
        //}

        public void GetPhotosFromDevice(string host, int port)
        {
            try
            {
                var tcpClient = new TcpClient();
                tcpClient.Connect(host, port);

                Console.WriteLine("Connected on port " + port  + "...");

                var tcpListener = new TcpListener(IPAddress.Any, 9000);
                tcpListener.Start();

                var stream = tcpClient.GetStream();
                var writer = new StreamWriter(stream);
                var reader = new StreamReader(stream);

                writer.WriteLine("Transmit");
                writer.Flush();
                Console.WriteLine("Waiting for ACK...");
                if (reader.ReadLine() == "ACK")
                {
                    var path = CreateHostDirectory(host);
                    var noOfFiles = Convert.ToInt32(reader.ReadLine());
                    Console.WriteLine("Number of files: " + noOfFiles);
                    var i = 0;
                    Console.WriteLine("Begin receiving...");

                    while (i < noOfFiles)
                    {
                        Console.WriteLine("Accepting clients...");
                        var s = tcpListener.AcceptTcpClient();

                        var inStream = s.GetStream();
                        var inReader = new StreamReader(inStream);

                        var fs = inReader.ReadLine();
                        var fileSize = Convert.ToInt32(fs);

                       // byte[] data = new byte[4];
                       // //Read The Size
                       // inStream.Read(data, 0, data.Length);
                       // int size = (BitConverter.ToInt32(data,0));
                       // // prepare buffer
                       //// data = new byte[size];

                        var fileName = inReader.ReadLine();

                        Console.WriteLine("Filename: " + fileName + " Filesize: " + fileSize);

                        var fileStream = new FileStream(path + fileName, FileMode.OpenOrCreate, FileAccess.Write);
                        var cursor = 0;
                        var receivedBytes = 0;

                        var recData = new byte[fileSize];

                        while ((receivedBytes = inStream.Read(recData, 0, fileSize - cursor)) > 0)
                        {
                            fileStream.Write(recData, 0, receivedBytes);
                            cursor += receivedBytes;
                        }

                        fileStream.Close();
                        inStream.Close();
                        inReader.Close();
                        s.Close();
                        i++;
                    }

                    stream.Close();
                    tcpClient.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.Message);
            }
        }

        public void SendImage(string host, string filename, int port)
        {
            var file = new FileInfo(filename);

            var tcpclnt = new TcpClient();
            Console.WriteLine("Connecting.....");

            tcpclnt.Connect(host, port);

            Console.WriteLine("Connected");

            var nStream = tcpclnt.GetStream();
            var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);

            var writer = new StreamWriter(nStream);

            writer.WriteLine("Receive");
            writer.Flush();
            Thread.Sleep(100);
            writer.WriteLine(file.Length.ToString());
            writer.Flush();
            Thread.Sleep(100);
            writer.WriteLine(file.Name);
            writer.Flush();
            var sendingBuffer = new byte[file.Length];

            fs.Read(sendingBuffer, 0, (int)file.Length);
            Thread.Sleep(100);
            nStream.Write(sendingBuffer, 0, (int)file.Length);

            Console.WriteLine("Done transmitting...");

            nStream.Close();
            tcpclnt.Close();
        }

        public string CreateHostDirectory(string host)
        {
            var specDir = host.Replace(".", "_");
            var dir = Environment.CurrentDirectory;
            var path = dir.EndsWith("\\") ? dir + specDir : dir + "\\" + specDir + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

    }
}
