using Assessment.Interfaces;
using Assessment.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assessment.Implementations
{
    public class AtmService : IAtmService
    {
        private List<Bill> Bills;
        private const int NumberOfBills = 10;
        private readonly List<int> DollarValues = new List<int> { 1, 5, 10, 20, 50, 100 };

        public AtmService()
        {
            RestockAtm();
        }

        public void RestockAtm()
        {
            Bills = new List<Bill>();
            foreach (var dollarValue in DollarValues)
            {
                Bills.AddRange(Enumerable.Repeat(new Bill { Value = dollarValue }, NumberOfBills));
            }
        }

        public string GetWelcomeMessage()
        {
            return "Welcome!";
        }

        public bool Withdraw(int requestedAmount)
        {
            var billValues = new Stack();
            var tempBills = Bills.ToList();
            if (Bills.Any())
            {
                foreach (var value in Bills.GroupBy(x => x.Value))
                {
                    billValues.Push(value.FirstOrDefault().Value);
                }
            }
            else
            {
                return false;
            }

            var billValue = (int)billValues.Pop();
            while (requestedAmount != 0)
            {
                if (requestedAmount >= billValue && tempBills.Where(x => x.Value == billValue).Count() > 0)
                {
                    requestedAmount -= billValue;
                    tempBills.Remove(Bills.FirstOrDefault(x => x.Value == billValue));
                }
                else
                {
                    if (billValue == DollarValues.Min())
                    {
                        return false;
                    }
                    billValue = (int)billValues.Pop();
                }
            }
            Bills = tempBills;
            return true;
        }

        public List<string> BuildBalanceOutput(List<int> valuesToInquiry = null)
        {
            List<string> output = new List<string>();
            if (valuesToInquiry == null || valuesToInquiry.Count() == 0)
            {
                output.AddRange(Bills.OrderByDescending(x => x.Value)
                    .GroupBy(x => x.Value)
                    .ToList()
                    .Select(x => $"${x.Key} - {x.Count()}"));

                //Add 0 values
                foreach (var dollarValue in DollarValues)
                {
                    if (output.Where(x => x.StartsWith($"${dollarValue} - ")).Count() == 0)
                    {
                        output.Add($"${dollarValue} - 0");
                    }
                }
            }
            else
            {
                var invalidInquiries = new List<string>();
                foreach (var value in valuesToInquiry)
                {
                    if (!DollarValues.Contains(value))
                    {
                        invalidInquiries.Add($"Invalid inquiry: {value}");
                    }
                }

                if (invalidInquiries.Any())
                    return invalidInquiries;

                output.AddRange(Bills.Where(x => valuesToInquiry.Contains(x.Value))
                    .OrderByDescending(x => x.Value)
                    .GroupBy(x => x.Value)
                    .ToList()
                    .Select(x => $"${x.Key} - {x.Count()}"));
            }

            output = output.OrderByDescending(x => int.Parse(x.TrimStart('$').Split(' ')[0])).ToList();
            output.Insert(0, "Machine balance:");
            return output;
        }

        public string Quit()
        {
            RestockAtm();
            return GetWelcomeMessage();
        }
    }
}
