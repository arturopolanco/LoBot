// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Attachment = Microsoft.Bot.Schema.Attachment;

namespace Lobo
{
    // This class calls the Microsoft Graph API. The following OAuth scopes are used:
    // 'OpenId' 'email' 'Mail.Send.Shared' 'Mail.Read' 'profile' 'User.Read' 'User.ReadBasic.All'
    // for more information about scopes see:
    // https://developer.microsoft.com/en-us/graph/docs/concepts/permissions_reference
    public static class OAuthHelpers
    {
        // Enable the user to send an email via the bot.
        public static async Task SendMailAsync(ITurnContext turnContext, TokenResponse tokenResponse, string recipient)
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            if (tokenResponse == null)
            {
                throw new ArgumentNullException(nameof(tokenResponse));
            }

            var client = new SimpleGraphClient(tokenResponse.Token);
            var me = await client.GetMeAsync();

            await client.SendMailAsync(
                recipient,
                "Message from a bot!",
                $"Hi there! I had this message sent from a bot. - Your friend, {me.DisplayName}");

            await turnContext.SendActivityAsync(
                $"I sent a message to '{recipient}' from your account.");
        }
        private const string WelcomeText = "Hi, Im able to look for a room and book it on your name";
        // Displays information about the user in the bot.
        public static async Task SendWelcomeCardMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            
            var reply = turnContext.Activity.CreateReply();
                    string[] Sala = { "one", "two", "three" };  //lo puse yo
                    reply.Text = WelcomeText;
                    reply.Attachments = new List<Attachment> { Cards.WelcomeCard("!").ToAttachment() }; // change with name later
                    //reply.Attachments = new List<Attachment> { CreateHeroCard("BOG", Sala).ToAttachment() };
                    //reply.Attachments = new List<Attachment> { CreateHeroCard(member.Id).ToAttachment() };
                    await turnContext.SendActivityAsync(reply, cancellationToken);
                
            
        }
        public static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {

            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    var reply = turnContext.Activity.CreateReply();
                    string[] Sala = { "one", "two", "three" };  //lo puse yo
                    reply.Text = WelcomeText;
                    reply.Attachments = new List<Attachment> { Cards.WelcomeCard(member.Name).ToAttachment() }; // change with name later
                    //reply.Attachments = new List<Attachment> { CreateHeroCard("BOG", Sala).ToAttachment() };
                    //reply.Attachments = new List<Attachment> { CreateHeroCard(member.Id).ToAttachment() };
                    await turnContext.SendActivityAsync(reply, cancellationToken);
                }
            }
        }
        public static async Task ListMeAsync(ITurnContext turnContext, TokenResponse tokenResponse)
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            if (tokenResponse == null)
            {
                throw new ArgumentNullException(nameof(tokenResponse));
            }

            // Pull in the data from the Microsoft Graph.
            var client = new SimpleGraphClient(tokenResponse.Token);
            var me = await client.GetMeAsync();
            // var RoomList = await client.GetList();
            //var manager = await client.GetManagerAsync(); //by art
            // var photoResponse = await client.GetPhotoAsync(); //by art

            // Generate the reply activity.
            var reply = turnContext.Activity.CreateReply();
            //var photoText = string.Empty;
            //if (photoResponse != null)//by art
            //{//by art
            //    var replyAttachment = new Attachment(photoResponse.ContentType, photoResponse.Base64String);//by art
            //    reply.Attachments.Add(replyAttachment);//by art
            //}//by art
            //else//by art
            //{//by art
            //    photoText = "Consider adding an image to your Outlook profile.";//by art
            //}//by art

            reply.Text = $"You are {me} and I don't give a shhh";
            await turnContext.SendActivityAsync(reply);
        }
        public static async Task ContinueAsync(ITurnContext turnContext, TokenResponse tokenResponse)
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            if (tokenResponse == null)
            {
                throw new ArgumentNullException(nameof(tokenResponse));
            }

            // Pull in the data from the Microsoft Graph.
            var client = new SimpleGraphClient(tokenResponse.Token);
            var me = await client.GetMeAsync();
            var KG = me.CompanyName;
            //var Sala = await client.FindRoomAsync();
            string[] Sala = { "Sala one", "Sala two", "Sala three" };  //lo puse yo
            var reply = turnContext.Activity.CreateReply();           
            reply.Text = $"You are in {me.DisplayName} .n {me.Mail}  {me.CompanyName} and {me.Manager}";
            reply.Attachments = new List<Attachment> { Cards.CreateHeroCard(KG, Sala).ToAttachment() };
            await turnContext.SendActivityAsync(reply);
        }
        public static async Task ListMeetingTimesAsync(ITurnContext turnContext, TokenResponse tokenResponse)
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            if (tokenResponse == null)
            {
                throw new ArgumentNullException(nameof(tokenResponse));
            }

            // Pull in the data from the Microsoft Graph.
            var client = new SimpleGraphClient(tokenResponse.Token);
            var MeetingTimes = await client.FindRoomAsync(DateTime.Now, DateTime.Now.AddDays(24), "Teststrins", "Test room", "testroom@devsdb.com");
            




            var reply = turnContext.Activity.CreateReply();
            reply.Text = $"You are {MeetingTimes} and I don't give a shhh";
            await turnContext.SendActivityAsync(reply);
        }
        // Gets recent mail the user has received within the last hour and displays up
        // to 5 of the emails in the bot.
        public static async Task ListRecentMailAsync(ITurnContext turnContext, TokenResponse tokenResponse)
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            if (tokenResponse == null)
            {
                throw new ArgumentNullException(nameof(tokenResponse));
            }

            var client = new SimpleGraphClient(tokenResponse.Token);
            var messages = await client.GetRecentMailAsync();
            var reply = turnContext.Activity.CreateReply();

            if (messages.Any())
            {
                var count = messages.Length;
                if (count > 5)
                {
                    count = 5;
                }

                reply.Attachments = new List<Attachment>();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                for (var i = 0; i < count; i++)
                {
                    var mail = messages[i];
                    var card = new HeroCard(
                        mail.Subject,
                        $"{mail.From.EmailAddress.Name} <{mail.From.EmailAddress.Address}>",
                        mail.BodyPreview,
                        new List<CardImage>()
                        {
                            new CardImage(
                                "https://botframeworksamples.blob.core.windows.net/samples/OutlookLogo.jpg",
                                "Outlook Logo"),
                        });
                    reply.Attachments.Add(card.ToAttachment());
                }
            }
            else
            {
                reply.Text = "Unable to find any recent unread mail.";
            }

            await turnContext.SendActivityAsync(reply);
        }

        // Prompts the user to log in using the OAuth provider specified by the connection name.
        public static OAuthPrompt Prompt(string connectionName)
        {
            return new OAuthPrompt(
                "loginPrompt",
                new OAuthPromptSettings
                {
                    ConnectionName = connectionName,
                    Text = "Please click here to login",
                    Title = "Login",
                    Timeout = 300000, // User has 5 minutes to login
                });
        }
    }
}
