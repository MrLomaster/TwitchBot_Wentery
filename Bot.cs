using TwitchLib.Client;
using TwitchLib.Client.Models;
using Microsoft.Data.Sqlite;

internal class Bot
{
    TwitchClient client = new TwitchClient();
    ConnectionCredentials credentials = new ConnectionCredentials("themisha34bot", "oauth:mgccxn63ulz86gv22hfsqcc0eyh3gp");
    int duel_queue = 1;

    public Bot()
    {
        client.Initialize(credentials, "wentery_");
        client.OnLog += Client_OnLog;
        client.OnJoinedChannel += Client_OnJoinedChannel;
        client.OnChatCommandReceived += Client_OnChatCommandReceived;

        client.Connect();
    }

    public void Client_OnChatCommandReceived(object? sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
    {
        string name = "";
        List<string> Duel = new List<string>(8);
        switch (e.Command.CommandText.ToLower())
        {
            case "инфа":
                client.SendMessage(e.Command.ChatMessage.Channel, "1. Меня зовут Mиша");
                client.SendMessage(e.Command.ChatMessage.Channel, "2. Мне 14 лет");
                client.SendMessage(e.Command.ChatMessage.Channel, "3. Живу в г.Кропоткин (Краснодарский край)");
                client.SendMessage(e.Command.ChatMessage.Channel, "4. Играю на iPad 2020 WiFi + Cellular 32GB(60fps)");
                client.SendMessage(e.Command.ChatMessage.Channel, "5. Мои настройки есть в группе ВК");
                break;

            case "дуэль":
                // Duel[num] = e.Command.ChatMessage.UserId;
                name = e.Command.ChatMessage.Username;
                Console.WriteLine(name);
                duel_queue++;
                client.SendMessage(e.Command.ChatMessage.Channel, "Ожидайте, вы " + duel_queue + " в очереде");
                if (duel_queue == 8)
                {
                    client.SendMessage(e.Command.ChatMessage.Channel, "ВНИМАНИЕ! Буфер листа подошёл к концу, все данные начинаются с начала списка...");
                    duel_queue = 1;
                }

                using (var connection = new SqliteConnection("Data Source=duel_queue.db"))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT duel_queue
                        FROM 
                        WHERE id = $id
                    ";
                    command.Parameters.AddWithValue("$id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var names = reader.GetString(0);

                            Console.WriteLine($"Hello, {names}!");
                        }
                    }
                }
                break;


                /* Меня зовут Миша
                 Мне 14 лет
                 Живу в г.Кропоткин(Краснодарский край)
                 Играю на iPad 2020 WiFi + Cellular 32GB(60fps)
                 Мои настройки есть в группе ВК
                 Задержка 1 сек */
        }

    }

    private void Client_OnJoinedChannel(object? sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
    {
        client.SendMessage(e.Channel, "Проверка взлома жопы пошла...");
        Thread.Sleep(3000);
        client.SendMessage(e.Channel, "Проверка пройдена. Для работы со мной используйте: !инфа | !дуэль");
    }

    private void Client_OnLog(object? sender, TwitchLib.Client.Events.OnLogArgs e)
    {
        Console.WriteLine(e.Data);
    }
}