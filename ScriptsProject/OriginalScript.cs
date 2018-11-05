using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;


//by pasvitas twitch.tv/pasvitas

namespace RutonyChat {
    public class Script {
        public void RunScript(string site, string username, string text, string param) {
			
			string file = ProgramProps.dir_scripts + @"\robbers.json";



			int credit;
			RankControl.ChatterRank cr = RankControl.ListChatters.Find(r => r.Nickname == username.Trim().ToLower());

			if (cr == null) {
                RutonyBot.BotSay(site,
                    username + ", ошибка! Вашей записи нет в базе данных или она повреждена!");
                return;
            }
			
			if (!Int32.TryParse(text.Substring(text.IndexOf(" ") + 1), out credit)) {
                RutonyBot.BotSay(site, "Количество кредитов должно быть больше 0!");
                return;
            }
            if (credit <= 0) {
                RutonyBot.BotSay(site, "Кредитов должно быть больше 0!");
                return;
            }
            if (cr.CreditsQty < credit) {
                RutonyBot.BotSay(site, string.Format("У вас всего {0} кредитов!", cr.CreditsQty));
                return;
            }

			cr.CreditsQty -= credit;
			
			

			if (!File.Exists(file))
			{
				AddRobber(username, credit, site);
                new Thread(() => {
                    Thread.CurrentThread.IsBackground = true;

                    Thread.Sleep(60000);

						int sum = 0;

                        Robbers players = GetListRobbers();
						foreach (Robber player in players.ListRobbers)
        				{
            				sum += player.amount;
        				}
			
						Random winrnd = new Random();
						int winrandom = winrnd.Next(1, 100);
						
						int sumrandom = (sum/10);
						if (sumrandom > 25) 
						{ 
							sumrandom = 25; 
						}
						
						if (winrandom+sumrandom > 80)
						{
							string messageSuccess = "";
							foreach (Robber player in players.ListRobbers)
    						{
								player.amount = player.amount*2;
								RankControl.ChatterRank cr_win = RankControl.ListChatters.Find(r => r.Nickname == player.name);
								cr_win.CreditsQty += player.amount;
								messageSuccess += player.name + " получил " + player.amount + " кредитов! ";
							}
							RutonyBot.BotSay(site, "Ограбление прошло успешно! " + messageSuccess);
						}
						else
						{
							bool userBest = false;
							foreach (Robber player in players.ListRobbers)
    						{
								if (player.name == "alexkwest" || player.name == "alexkwest craft")
								{
									userBest = true;
									RankControl.ChatterRank cr_win = RankControl.ListChatters.Find(r => r.Nickname == player.name);
									cr_win.CreditsQty += sum;
									RutonyBot.BotSay(site, player.name + " заложил свою банду. Обнёс общаг суммой: " + sum + " и свалил. Его подельники остались с носом.");
								}
							}
							if (!userBest)
							{
								Random randomBank = new Random();
								int zeroCash = randomBank.Next(1, 100);
								if (zeroCash < 2)
    							{
    								string userBank = " ";
									foreach (Robber player in players.ListRobbers)
									{
										RankControl.ChatterRank cr_win = RankControl.ListChatters.Find(r => r.Nickname == player.name);
										cr_win.CreditsQty = 0;
										userBank += player.name + " ";
									}
									RutonyBot.BotSay(site, userBank + ", вас накрыл ОМОН и изъял все ваши грязные деньги.");
    							}
    							else 
    							{
    								GetAnswer(site, "Примечание автора");
    								//RutonyBot.BotSay(site, "Ограбление не удалось, но грабителям удалось унести ноги.");
    							}		
							}	
						}
										
						try {
							File.Delete(file);
						} catch { }

					}).Start();
                return;
			}
			else
			{
				AddRobber(username, credit, site);
			}
        }

        public void GetAnswer(string site, string bandaName)
        {
        	List<string> answer = new List<string>();
        	answer.Add("Ваша банда оказалась слишком слабой, и её отпинали в подворотне. Ограбление не удалось.");
        	answer.Add("Ограбление прошло успешно, но придя на хату, обнаружили, что вы ограбили Банк Приколов. Настоящих денег нет.");
        	answer.Add("Вы встретили Черепашек Ниндзя и с трудом унесли ноги. Ограбление не удалось.");
        	answer.Add("Вместо организации ограбления ваша банда пошла в бар и оставила там весь общаг.");
        	answer.Add("Вы успешно ограбили банк. Что бы отпраздновать дело ваша, банда пригласила на хату клофелинщицу Соню, по прозвищу Золотые Ручки. Когда банда очнулась денег и Сони уже не было.");
        	answer.Add("К вам пришел батюшка. Общаг ушел на пожертвование церкви. Ограбление не удалось.");
        	answer.Add("В городе состоялся концерт Михаила Круга. Общаг ушел на цветы артисту. Ограбление не удалось.");
			answer.Add("Во время ограбления вы встретили коммандера Шепарда. Ограбление не удалось.");
			answer.Add("Во время ограбления вы встретили Айзека Кларка. И некроморфов. Вы решили, что сегодня не ваш день для ограбления.");
			answer.Add("Когда ваша банда пришла в банк, вы узнали, что все деньги уже забрал человек с дипломатом и четырьмя пальцами на левой руке. Ограбление не удалось.");
			answer.Add("Вашу банду поймал Бетмен. К счастью, продажные копы вас отпустили. Ограбление не удалось.");
			answer.Add("Вашу банду поймал Джокер. К счастью, пока его пинал Бетмен, вы сумели убежать и скрыться. Ограбление не удалось.");
			answer.Add("Вашу банду поймала Стрипирелла. К счастью. Ограбление не удалось, но вы остались довольны.");
			answer.Add("Уличные хулиганы отобрали рядом со школой весь общаг вашей банды. Вы написали заявление в полицию.");
			answer.Add("Во время ограбления вы встретили коммандера Шепарда. Ограбление удалось, но всю добычу пришлось отдать на борьбу со Жнецами.");
			answer.Add("Ваша банда на улице встретила старуху Шапокляк и крокодила Гену. Пока пенсионерка и рептилойд отвлекали ваше внимание, крыса Лариска украла весь ваш общаг.");
			answer.Add("В банке вы встрели белых ходоков. Со словами, Зима близко, ваш общаг забрал Король Ночи. Ограбление не удалось.");
			answer.Add("Пока вы грабили банк, наступил зомби апокалипсис. К счастью, помимо мозгов, зомби еще любят есть деньги. Вся добыча ушла на то, что бы отвлечь живых мертвецов.");
			answer.Add("По дороге в банк ваша банда решила вместо ограбления вложиться в перспективную молодую компанию МММ. Денег нет, но вы держитесь.");
			answer.Add("По дороге в банк ваша банда встретила цыганку. Ваша банда потеряла весь общаг, коня, две рубашки, магнитафон, портсигар финский и куртку из нейлона.");
			answer.Add("Ваша банда решила прибухнуть. Ограбление не удалось.");
			answer.Add("Ограбление прошло успешно, но в стране случился дефолт и ваша добыча теперь ничего не стоит.");
			answer.Add("Когда вы пришли в банк, оказалось, что до вас его уже ограбил Тревор. Может постримить ГТА5?");
			answer.Add("Вы подскользнулись на банане, потеряли сознание, очнулись - гипс. А общага уже нет. Ограбление не удалось.");
			answer.Add("Вместо ограбления ваша банда решила пожертвовать общаг голодающим Африки.");
			answer.Add("Ограбление удалось, но у украденных денег выросли ложноножки, и они убежали.");
			answer.Add("Ваша банда спустила весь общаг в зале игровых автоматов. При попытке ограбить этот зал, вашу банду спустили из этого зала.");
			answer.Add("Вашу банду поймал Человек Паук. К счастью, он согласился вас отпустить, если вы сделаете репост стрима. Ограбление не удалось.");
			answer.Add("Ваша банда успешно ограбила банк, но неожиданно появился Супер Марио и скушал все монеты.");
			answer.Add("Ваша банда вскрыла хранилище банка, но вместо денег лежала записка с приветом от Джека Воробья. Капитана Джека Воробья.");
			answer.Add("Пацаны к успеху шли, не получилось, не фартануло. Ограбление не удалось.");

        	Random rnd = new Random();
            int rndValue = rnd.Next(answer.Count - 1);

            string hitText = "";
            try 
            {
                hitText = answer[rndValue];
            } 
            catch 
            { 
                RutonyBot.BotSay(site, "Что-то пошло не так как задумывал Кодераст");
            }

            hitText = hitText.Replace("bandaName", bandaName);
            RutonyBot.BotSay(site, hitText);
        }
	
		public Robbers GetListRobbers()
		{
			string file = ProgramProps.dir_scripts + @"\robbers.json";

			Robbers players = new Robbers();

            if (File.Exists(file))
			{
                string[] filetexts = File.ReadAllLines(file);

			    players = JsonConvert.DeserializeObject<Robbers>(filetexts[0]);
				
			}

			return players;
		}

		public void AddRobber(string username, int vklad, string site)
		{
			string file = ProgramProps.dir_scripts + @"\robbers.json";
			Robbers players = GetListRobbers();

			Robber thisrobber = players.ListRobbers.Find(r => r.name == username.Trim().ToLower());

			if (thisrobber == null) {
                
                players.ListRobbers.Add(new Robber() {name=username.Trim().ToLower(), amount = vklad});
                thisrobber = players.ListRobbers.Find(r => r.name == username.Trim().ToLower());
				RutonyBot.BotSay(site, username + ", спасибо за вклад. Ждем других участников!");

                try {
                        File.Delete(file);
             	 } catch { }

            	string serialized = JsonConvert.SerializeObject(players);
            	RutonyBotFunctions.FileAddString(file, serialized);
            }
			else
			{
				RutonyBot.BotSay(site, username + ", вы уже вложились в ограбление.");

			}

		}

           

    }
	public class Robbers
    {
        public List<Robber> ListRobbers = new List<Robber>();
    }

    public class Robber
	{

		public string name {get; set;}
        public int amount {get; set;}

	}
}