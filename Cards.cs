using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace Lobo
{
    public class Cards
    {
        private static HeroCard CreateHeroCard(string newUserName,string KG,int,Array Rooms)
        {
            int AmountOfItems = Rooms.Length;
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
                Buttons = new List<CardAction>
                {for (int i = 0; i < AmountOfItems; i++)
                {
                new CardAction(ActionTypes.ImBack,Rooms[i], text: "Me", displayText: "Me", value: i),
        
                },
                },
            };
            return heroCardMeetingRooms;
        }




