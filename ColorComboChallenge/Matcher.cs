using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorComboChallenge
{
    public class Matcher
    {
        public const string INVALID_COMBINATION = "Cannot unlock master panel";
        
        public static string TryMatch(string input)
        {
            var outputString = INVALID_COMBINATION;
            var validList = true;

            try
            {
                if (!String.IsNullOrEmpty(input))
                {
                    List<String> inputs = input.Split(',').Select(p => p.Trim()).ToList();
                    if (inputs.Count % 2 == 0 && inputs.Count >= 4)
                    {
                        var leftConnector = inputs[0];
                        var rightConnector = inputs[1];

                        var inputList = inputs.Skip(2).ToList();

                        var chipList = GetChipList(inputList);

                        var outputList = new List<Chip>();

                        //Check if there is only one chip and it matches
                        if (chipList.Count == 1 && chipList[0].LeftSymbol.ToUpper() == leftConnector.ToUpper() &&
                            chipList[0].RightSymbol.ToUpper() == rightConnector.ToUpper())
                        {
                            outputList.Add(chipList[0]);
                            outputString = GetOutputString(outputList);
                        }
                        else
                        {
                            var potentialFirstChips = chipList.Where(e => e.LeftSymbol.ToUpper() == leftConnector.ToUpper()).ToList();
                            var potentialLastChips = chipList.Where(e => e.RightSymbol.ToUpper() == rightConnector.ToUpper()).ToList();

                            if (potentialFirstChips.Any() && potentialLastChips.Any())
                            {
                                foreach (var firstChip in potentialFirstChips)
                                {
                                    var workingChipList = chipList.ToList();
                                    workingChipList.Remove(firstChip);
                                    outputList.Clear();
                                    outputList.Add(firstChip);
                                    foreach (var lastChip in potentialLastChips)
                                    {
                                        workingChipList.Remove(lastChip);
                                        validList = BuildList(firstChip.RightSymbol, workingChipList, outputList, lastChip.LeftSymbol);
                                        if (validList)
                                        {
                                            outputList.Add(lastChip);
                                            break;
                                        }
                                        else
                                        {
                                            outputList.Clear();
                                            outputList.Add(firstChip);
                                        }
                                    }
                                    if (validList)
                                    {
                                        break;
                                    }

                                }
                            }
                            
                            if (validList)
                            {
                                outputString = GetOutputString(outputList);
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                outputString = INVALID_COMBINATION;
            }

            return outputString;
        }

        private static bool BuildList(string startingSymbolRight, List<Chip> workingItems, List<Chip> outputList, string lastSymbolLeft)
        {
            if (!workingItems.Any() && outputList.Last().RightSymbol == lastSymbolLeft)
            {
                return true;
            }
            var temporaryOutputList = outputList.ToList();

            var isValid = true;

            var potentialItems = workingItems.Where(e => e.LeftSymbol == startingSymbolRight).ToList();

            if (!potentialItems.Any())
            {
                return false;
            }
            
            foreach (var potentialItem in potentialItems)
            {
                var remainingItems = workingItems.ToList();
                remainingItems.Remove(potentialItem);
                
                outputList.Add(potentialItem);
                isValid = BuildList(potentialItem.RightSymbol, remainingItems, outputList, lastSymbolLeft);
                if (!isValid)
                {
                    outputList = temporaryOutputList;
                }
                else
                {
                    break;
                }
            }
            return isValid;
        }
        
        
        private static string GetOutputString(List<Chip> outputList)
        {
            var stringBuilder = new StringBuilder();
            if (outputList.Count == 0)
            {
                stringBuilder.Append(INVALID_COMBINATION);
            }
            else
            {
                foreach (var chip in outputList)
                {
                    if (stringBuilder.ToString() != string.Empty)
                    {
                        stringBuilder.Append(", ");
                    }
                    stringBuilder.Append(String.Format("{0}, {1}", chip.LeftSymbol, chip.RightSymbol));
                }
            }
            
            return stringBuilder.ToString();
        }

        private static List<Chip> GetChipList(List<string> inputList)
        {

            var chipList = new List<Chip>();
            var leftColor = string.Empty;
            var rightColor = string.Empty;

            for (var i = 0; i < inputList.Count; i++)
            {
                if (i % 2 != 0)
                {
                    rightColor = inputList[i];
                    chipList.Add(new Chip { LeftSymbol = leftColor, RightSymbol = rightColor });
                    leftColor = string.Empty;
                }
                else
                {
                    leftColor = inputList[i];
                }
            }
            return chipList;
        }
    }
}
