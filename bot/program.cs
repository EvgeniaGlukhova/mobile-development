using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using Telegram.Bot.Types.Enums;



namespace bot
{
    internal class Program
    {
        private static bool isRun = true; //флаг состояния



        static void Main(string[] args)
        {
            var client = new TelegramBotClient("7567341738:AAGFIKIrLNGpqNVJnZ1TBUlR0589T4CIlTA");
            client.StartReceiving(Update, Error); 
            Console.ReadLine(); //это чтоб бот ждал сообщения от пользователя, а не завершал работу при запуске

        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message; // сообщение пользователя
            if (message.Text == "/start")
            {
                isRun = true;
                await botClient.SendTextMessageAsync(message.Chat, "Здравствуй!\r\nВыберите категорию рецептов:", replyMarkup: GetMainButtons());
            }

            else if (message.Text == "/end")
            {
                isRun = false;
                await botClient.SendTextMessageAsync(message.Chat, "До свидания!\r\n");
            }

            

            else if (isRun && message.Text != null && message.Text != "/end")
            {
                string userInput = message.Text.ToLower(); // нижний регистр

                switch (userInput)
            {
                case "meat":
                        await botClient.SendTextMessageAsync(message.Chat, "Выберите подкатегорию мяса:", replyMarkup: GetMeatButtons());
                        break;
                case "chicken":
                        string RandomChicken = GetRandomChicken();
                        await botClient.SendTextMessageAsync(message.Chat, RandomChicken);
                        break;
                case "beef":
                        string RandomBeef = GetRandomBeef();
                        await botClient.SendTextMessageAsync(message.Chat, RandomBeef);
                        break;
                case "pork":
                        string RandomPork = GetRandomPork();
                        await botClient.SendTextMessageAsync(message.Chat, RandomPork);
                        break;

                case "vegatable":
                        await botClient.SendTextMessageAsync(message.Chat, "Выберите подкатегорию овоща:", replyMarkup: GetVegatableButtons());
                        break;
                case "potatoes": // картошка
                        string RandomPotatoese = GetRandomPotatoes();
                        await botClient.SendTextMessageAsync(message.Chat, RandomPotatoese);
                        break;
                case "cabbage":  // капуста
                        string RandomCabbage = GetRandomCabbage();
                        await botClient.SendTextMessageAsync(message.Chat, RandomCabbage);
                        break;
                case "carrots": // морковка
                        string RandomCarrots = GetRandomCarrots();
                        await botClient.SendTextMessageAsync(message.Chat, RandomCarrots);
                        break;
                case "mushrooms":  // грибы 
                        string RandomMushroom = GetRandomMushroom();
                        await botClient.SendTextMessageAsync(message.Chat, RandomMushroom);
                        break;
                case "назад": //  Назад
                        await botClient.SendTextMessageAsync(message.Chat, "Выберите категорию рецептов:", replyMarkup: GetMainButtons());
                        break;

                    default:
                        await botClient.SendTextMessageAsync(message.Chat, "Неизвестная команда. Попробуйте еще раз.");
                        break;

                }


               
            }
            else if (message.Type == MessageType.Photo)
            {
                
                await botClient.SendTextMessageAsync(message.Chat, "Пожалуйста, введите текстовую команду.");
            }
            else if (!isRun)
            {
                await botClient.SendTextMessageAsync(message.Chat, "Пожалуйста, напишете /start для того чтобы бот начала работать.");
            }
        }

      

        private static string GetRandomChicken()
        {
            return GetRandomRecipe("C:\\Users\\HUAWEI\\source\\repos\\bot\\bot\\Chicken.txt");  

        }

        private static string GetRandomBeef()
        {
            return GetRandomRecipe("C:\\Users\\HUAWEI\\source\\repos\\bot\\bot\\Beef.txt");
        }

        private static string GetRandomPork()
        {
            return GetRandomRecipe("C:\\Users\\HUAWEI\\source\\repos\\bot\\bot\\Pork.txt");
        }

        private static string GetRandomPotatoes()
        {
            return GetRandomRecipe("C:\\Users\\HUAWEI\\source\\repos\\bot\\bot\\Potatoes.txt");
        }

        private static string GetRandomCabbage()
        {
            return GetRandomRecipe("C:\\Users\\HUAWEI\\source\\repos\\bot\\bot\\Cabbage.txt");
        }

        private static string GetRandomCarrots()
        {
            return GetRandomRecipe("C:\\Users\\HUAWEI\\source\\repos\\bot\\bot\\Carrots.txt");
        }

        private static string GetRandomMushroom()
        {
            return GetRandomRecipe("C:\\Users\\HUAWEI\\source\\repos\\bot\\bot\\Mushrooms.txt");
        }

        


        private static string GetRandomRecipe(string filePath)
        {
            try
            {
                // Чтение всего содержимого файла
                string fileContent = File.ReadAllText(filePath);
                
                string[] recipes = fileContent.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (recipes.Length > 0)
                {
                    Random random = new Random();
                    int index = random.Next(recipes.Length);
                    return recipes[index].Trim(); 
                }
                else
                {
                    return "Файл пуст или не найден.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                return "Произошла ошибка при чтении файла.";
            }
        }




        private static IReplyMarkup GetMainButtons() // первый слой 
        {
            return new ReplyKeyboardMarkup(
                new[]
                {
                    new[] { new KeyboardButton("meat"), new KeyboardButton("vegatable") },
                    

                }
            );
        }

        private static IReplyMarkup GetMeatButtons()  // зашли в мясо
        {
            return new ReplyKeyboardMarkup(
                new[]
                {
                    new[] { new KeyboardButton("chicken"), new KeyboardButton("beef") },
                    new[] { new KeyboardButton("pork"), new KeyboardButton("назад") }

                }
            );
        }


        private static IReplyMarkup GetVegatableButtons()    // зашли в овощи
        {
            return new ReplyKeyboardMarkup(
                new[]
                {
                    new[] { new KeyboardButton("potatoes"), new KeyboardButton("cabbage") },
                    new[] { new KeyboardButton("carrots"), new KeyboardButton("mushrooms") },
                    new[] { new KeyboardButton("назад") }


                }
            );
        }

        private static async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine($"Произошла ошибка: {exception.Message}");

        }
    }
}
