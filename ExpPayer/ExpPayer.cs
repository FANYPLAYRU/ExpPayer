using System;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Extensions;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Linq;
using System.Net;

namespace ExpPay {
	public class ExpPayer : RocketPlugin<Config> {
		protected override void Load()
		{
			WebClient webClient = new WebClient();
			base.Load();
			ExpPayer.Instance = this;
			LastSalary = DateTime.Now;
			TimeBetweenSalaries = ExpPayer.Instance.Configuration.Instance.TimeBetweenSalaries;
			Rocket.Core.Logging.Logger.Log("Автор плагина: Fanya", ConsoleColor.Red);
			Rocket.Core.Logging.Logger.Log("Отдельное спасибо: Greenorine", ConsoleColor.Green);
			Rocket.Core.Logging.Logger.Log("Сообщать об ошибках: https://vk.com/fanyplayservers", ConsoleColor.Green);
			Rocket.Core.Logging.Logger.Log("Версия плагина: " + version, ConsoleColor.Green);
			Rocket.Core.Logging.Logger.Log("Проверяю обновление... ", ConsoleColor.Green);
			string latestversion = webClient.DownloadString("https://logs.fanyplay.ru/projects/fanya/plugins/exppayer/version.txt");
			bool flag3 = latestversion.Contains(version);
			if (flag3)
			{
				Rocket.Core.Logging.Logger.Log("У вас последнее обновление", ConsoleColor.Green);
			}
			else
			{
				string changelog = webClient.DownloadString("https://logs.fanyplay.ru/projects/fanya/plugins/exppayer/changelog.txt");
				string url = webClient.DownloadString("https://logs.fanyplay.ru/projects/fanya/plugins/exppayer/url.txt");
				Rocket.Core.Logging.Logger.Log("Найдено обновление");
				Rocket.Core.Logging.Logger.Log(url);
				Rocket.Core.Logging.Logger.Log("Новая версия: " + latestversion);
				Rocket.Core.Logging.Logger.Log(changelog);

			}
			if (ExpPayer.Instance.Configuration.Instance.PluginLoad == true)
			{
				Rocket.Core.Logging.Logger.Log("Плагин включен", ConsoleColor.Green);
			} else
			{
				Rocket.Core.Logging.Logger.Log("Плагин выключен", ConsoleColor.Red);
				Unload();
				return;
			}
		}

		public void FixedUpdate() {
			if ((DateTime.Now - LastSalary).TotalSeconds >= TimeBetweenSalaries) {
				LastSalary = DateTime.Now;
				foreach (SteamPlayer player in Provider.clients) {
					UnturnedPlayer unturnedPlayer = player.ToUnturnedPlayer();
					foreach (Config.SalaryData salaryData in ExpPayer.Instance.Configuration.Instance.SalaryList) {
						if (R.Permissions.GetPermissions(unturnedPlayer).Exists(x => x.Name == salaryData.Permission))
						{
							var salaryPermission = R.Permissions.GetPermissions(unturnedPlayer).First(x =>
								Instance.Configuration.Instance.SalaryList.Exists(s => s.Permission == x.Name));
							if (salaryPermission == null) return;

							var salary =
							Instance.Configuration.Instance.SalaryList.FirstOrDefault(x =>
							x.Permission == salaryPermission.Name);
							unturnedPlayer.Experience += salaryData.salary;
							UnturnedChat.Say(unturnedPlayer.CSteamID, salaryData.Text + salaryData.salary.ToString() + Configuration.Instance.currencySymbol);
						}
					}
				}
			}
		}

		protected override void Unload() {
			base.Unload();
			ExpPayer.Instance = null;
		}

		public static ExpPayer Instance;

		public DateTime LastSalary;

		public string version = "1.1.2";
				
		public float TimeBetweenSalaries;

		public override TranslationList DefaultTranslations
		{
			get
			{
				return new TranslationList()
				{
                    {"exppayer_exp_lengtherror", "[ExpPayer] - /exp <кол-во> | Выдача опыта "},
					{"exppayer_exp_arg1error", "[ExpPayer] -  Укажите число после /exp"},
					{"exppayer_exp_arg1zero", "[ExpPayer] -  Опыт установлен на 0"},
					{"exppayer_exp_success", "[ExpPayer] -  Вы успешно выдали {0}{1}"},
					{"exppayer_pay_lowexperror", "[ExpPayer] -  На вашем балансе {0}{1}, а вы пытались перевести {2}{1}. У вас недостаточно {1}"},
					{"exppayer_pay_lengtherror", "[ExpPayer] -  /pay <имя> <кол-во>"},
					{"exppayer_pay_arg2error", "[ExpPayer] -  Введите число после имени!"},
					{"exppayer_pay_targeterror", "[ExpPayer] -  Введите имя после /pay!"},
					{"exppayer_pay_success", "[ExpPayer] -  Вы перевели игроку {0} {1}{2}"},
					{"exppayer_pay_success_player", "[ExpPayer] -  Вам поступил перевод в размере {1}{2} от игрока {0}"},
					{"exppayer_pay_lowzeroexperror", "[ExpPayer] -  Вы не можете перевести отрицательное число"}
				};
			}
		}
	}
}
