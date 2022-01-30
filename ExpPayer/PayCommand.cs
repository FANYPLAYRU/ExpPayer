using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace ExpPay
{
    public class PayCommand : IRocketCommand
	{

		public void Execute(IRocketPlayer caller, string[] command)
		{
			var up = (UnturnedPlayer)caller;
			if (command.Length != 2)
			{
				UnturnedChat.Say(caller, ExpPayer.Instance.Translate("exppayer_pay_lengtherror"), UnityEngine.Color.red);
				return;
			}
			if (string.IsNullOrEmpty(command[0]))
			{
				UnturnedChat.Say(caller, ExpPayer.Instance.Translate("exppayer_exp_lengtherror"), UnityEngine.Color.red);
				return;
			}
			if (!PlayerTool.tryGetSteamPlayer(command[0], out var target))
			{
				UnturnedChat.Say(caller, ExpPayer.Instance.Translate("exppayer_pay_targeterror"), UnityEngine.Color.red);
				return;
			}
			if (!uint.TryParse(command[1], out var arg2))
			{
				UnturnedChat.Say(caller, ExpPayer.Instance.Translate("exppayer_pay_arg2error"), UnityEngine.Color.red);
				return;
			}
			if (arg2 > up.Player.skills.experience)
			{
				UnturnedChat.Say(caller, ExpPayer.Instance.Translate("exppayer_pay_lowexperror", up.Player.skills.experience, ExpPayer.Instance.Configuration.Instance.currencySymbol, arg2), UnityEngine.Color.red);
				return;
			}
			up.Player.skills.ServerSetExperience(up.Player.skills.experience - arg2);
			target.player.skills.ServerSetExperience(target.player.skills.experience + arg2);
			UnturnedChat.Say(caller, ExpPayer.Instance.Translate("exppayer_pay_success", target.player.name, arg2, ExpPayer.Instance.Configuration.Instance.currencySymbol), UnityEngine.Color.green, false);
			ChatManager.say(target.playerID.steamID, ExpPayer.Instance.Translate("exppayer_pay_success_player", up.Player.name, arg2, ExpPayer.Instance.Configuration.Instance.currencySymbol), UnityEngine.Color.green, false);
			}

		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "pay";
		public string Help => "Transfer specified amount of money to specified player";
		public string Syntax => "<player> <amount>";
		public List<string> Aliases { get { return new List<string>(); } }
		public List<string> Permissions => new List<string> { "exppayer.pay" };
	}
}