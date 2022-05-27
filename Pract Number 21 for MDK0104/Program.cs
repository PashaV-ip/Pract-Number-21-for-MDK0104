using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Pract_Number_21_for_MDK0104
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                int MaxThreadsCount = Environment.ProcessorCount * 4;
                ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount); //Максимальное число рабочих потоков
                ThreadPool.SetMinThreads(2, 2); //Минимальное число рабочих потоков
                Int32 port = 9595; //Устанавливаем порт для TcpListener 9595
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                int counter = 0;
                server = new TcpListener(localAddr, port);
                Console.OutputEncoding = Encoding.GetEncoding(866);
                Console.WriteLine("Конфигурация многопоточного сервера:");
                Console.WriteLine("   IP-адрес   : 127.0.0.1");
                Console.WriteLine("   Порт   : " + port.ToString());
                Console.WriteLine("   Потоки   :" + MaxThreadsCount.ToString());
                Console.WriteLine("\nСервер запущен\n");
                server.Start(); //Запускаем TcpListener и начинаем слушать клиентов.
                while (true) //Цикл для бесконечного приёма клиентов
                {
                    Console.Write("\nОжидание соединения... ");
                    ThreadPool.QueueUserWorkItem(ClientProcessing, server.AcceptTcpClient());
                    counter++;
                    Console.Write("\nСоединение №" + counter.ToString() + "!");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                server.Stop();
            }
            Console.WriteLine("\nНажмите Enter...");
            Console.Read();
        }
        static void ClientProcessing(object client_obj)
        {
            byte[] bytes = new byte[256]; //Буфер для принимаемых данных
            string data = null;
            TcpClient client = client_obj as TcpClient;
            data = null;
            NetworkStream stream = client.GetStream(); //Получение информации от клиента
            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0) //Приём данных от клиента через цикл
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i); //Преобразуем данные в ASCII string
                data = data.ToUpper(); //Преобразование строки в верхний регистр
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data); //Преобразуем полученную строку в массив байт
                stream.Write(msg, 0, msg.Length); //Отправляем данные обратно клиенту (Ответ)
            }
            client.Close();
        }

    }
}
