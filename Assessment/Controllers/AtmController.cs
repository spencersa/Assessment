using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assessment.Implementations;
using Assessment.Interfaces;

namespace Assessment.Controllers
{
    [Route("api/[controller]")]
    public class AtmController : Controller
    {
        private readonly AtmService _atmService;

        public AtmController(AtmService atmService)
        {
            _atmService = atmService;
        }

        [HttpGet("[action]")]
        public IEnumerable<string> GetWelcomeMessage()
        {
            return new List<string> { _atmService.GetWelcomeMessage() };
        }

        [HttpPost("[action]")]
        public List<string> SendCommand([FromBody] List<string> command)
        {
            command = command.Select(x => x.Trim('$')).ToList();
            var commandLetter = command[0].ToUpper();
            List<string> commandResult = new List<string>();
            switch (commandLetter)
            {
                case "R":
                    _atmService.RestockAtm();
                    commandResult = _atmService.BuildBalanceOutput();
                    break;
                case "W":
                    if (command.Count() > 1)
                    {
                        commandResult = GetWithdrawOutput(command[1]);
                    }
                    else
                    {
                        commandResult.Add("Failure: Invalid Command, no withdrawl ammount");
                    }
                    break;
                case "I":
                    var intCommand = command.Skip(1).Select(x => int.Parse(x));
                    commandResult = _atmService.BuildBalanceOutput(intCommand.ToList());
                    break;
                case "Q":
                    commandResult.Add(_atmService.Quit());
                    break;
                default:
                    commandResult.Add("Failure: Invalid Command");
                    break;
            }
            return commandResult;
        }

        private List<string> GetWithdrawOutput(string command)
        {
            var amount = 0;
            List<string> commandResult = new List<string>();
            if (int.TryParse(command, out amount))
            {
                if (amount == 0)
                {
                    commandResult.Add($"Cannot withdraw {amount}");
                    return commandResult;
                }
                if (_atmService.Withdraw(amount))
                {
                    commandResult.Add($"Success: Dispensed ${amount}");
                    commandResult.AddRange(_atmService.BuildBalanceOutput());
                }
                else
                {
                    commandResult.Add("Failure: Insufficient funds");
                }
            }
            else
            {
                commandResult.Add("Failure: Invalid Command");
            }
            return commandResult;
        }
    }
}
