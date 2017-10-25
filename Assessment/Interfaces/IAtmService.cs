using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment.Interfaces
{
    public interface IAtmService
    {
        void RestockAtm();
        string GetWelcomeMessage();
        bool Withdraw(int requestedAmount);
        List<string> BuildBalanceOutput(List<int> valuesToInquiry = null);
        string Quit();
    }
}
