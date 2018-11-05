using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace Lobo
{
    public class Cards
    {       
      public static  HeroCard CreateHeroCard(string KG, string[] Rooms)
        {

            int AmountOfItems = Rooms.Length;
            List<CardAction> cardButtons = new List<CardAction>();
            for (int i = 0; i < AmountOfItems; i++)
            {
                CardAction addButton = new CardAction()
                {

                    Value = i,
                    Type = "ImBack",
                    Title = Rooms[i],

                };
                cardButtons.Add(addButton);
            }
            var heroCardMeetingRooms = new HeroCard($"For KG {KG}, I have these Rooms listed", "Choose which one you want to use")
            {
                Images = new List<CardImage>
                {
                    new CardImage(
                        "https://botframeworksamples.blob.core.windows.net/samples/aadlogo.png",
                        "TEST",
                        new CardAction(
                            ActionTypes.OpenUrl,
                            value: "https://ms.portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/Overview")),
                },
                Buttons = cardButtons
            };
            return heroCardMeetingRooms;
        }
    }
}
      