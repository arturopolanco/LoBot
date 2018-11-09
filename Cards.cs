using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace Lobo
{
    public class Cards
    {       
      public static HeroCard CreateHeroCard(string KG, string[] Rooms)
        {

            int AmountOfItems = Rooms.Length;
            List<CardAction> cardButtons = new List<CardAction>();
            for (int i = 0; i < AmountOfItems; i++)
            {
                CardAction addButton = new CardAction()
                {

                    Value = i+1,
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
      public static HeroCard WelcomeCard(string UserName)
        {
            var heroCard = new HeroCard($"Welcome {UserName}")
            {
                Images = new List<CardImage>
                {
                    new CardImage(
                        "https://i0.wp.com/static1.wikia.nocookie.net/__cb20121001100335/adventuretimewithfinnandjake/images/5/56/Get_A_Room.png",
                        "Get A Room"
                       ),
                },
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, "Not Me", text: "Not Me", displayText: "Not Me", value: "Not Me"),
                    new CardAction(ActionTypes.ImBack, "Continue", text: "Continue", displayText: "Continue", value: "Continue"),
                },
            };
            return heroCard;
        }
    }
}
      