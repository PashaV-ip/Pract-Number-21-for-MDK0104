using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client_for_server_Pract_21
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("\n Соединение # " + i.ToString() + "\n");
                Connect("127.0.0.1", "Hello world! #" + i.ToString());
            }
            Console.WriteLine("\n Нажмите Enter...");
            Console.Read();
        }
        static void Connect(string server, string message)
        {
            try
            {
                //Создаём TCPClient
                Int32 port = 9595;
                TcpClient client = new TcpClient(server, port);
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message); //Переводим наше сообщение в ASCII, а затем в массив Byte
                NetworkStream stream = client.GetStream();//Получаем поток для чтения и записи данных
                stream.Write(data, 0, data.Length); //Отправляем сообщение серверу
                Console.WriteLine($"Отправлено: {message}");
                //Получаем ответ от сервера
                data = new byte[256]; //Буфер для хранения принятого массива bytes
                string responseData = string.Empty; // Строка для хранения ASCII данных
                // Читаем первый пакет ответа сервера.
                // Можно читать всё сообщение.
                // Для этого надо организовать чтение в цикле как на сервере.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine($"Получено: {responseData}");
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"ArgumentNullException: {e}");
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException : {e}");
            }
        }
    }
}
