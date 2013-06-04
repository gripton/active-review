using System;
using System.Collections.Generic;
using System.Net.Mail;
using ARR.Data.Entities;
using FluentEmail;

namespace ARR.Notifications
{
    public class NotificationArgs
    {
        public string BaseUrl { get; set; }
        public string Recipient { get; set; }
        public string SessionId { get; set; }
        public string SessionName { get; set; }
        public string DisplayName { get; set; }
    }

    public class NotificationGenerator : INotificationGenerator
    {
        private readonly Dictionary<EventType, Tuple<string, string>> mailInfoDictionary;

        private const string releasedSubject = "Review Session Released";

        private const string releasedMessage =
            "Greetings @(Model.DisplayName). Review Session '@(Model.SessionName)' has been released for particpation. " +
            "Click <a href=\"@(Model.BaseUrl)Screens/Editor.html?reviewSession=@(Model.SessionId)\">here</a> " +
            "to begin the Questionnaire.";

        private const string assignedSubject = "Reviewer Assigned";
        private const string assignedMessage =
            "Greetings @(Model.DisplayName). You have been assigned as the reviewer for Active Review Session '@(Model.SessionName)'. Once the Review " +
            "Session has been released you will reveice another notification with a link to the Review Session Questionnaire.";

        private const string completedSubject = "Review Session Questionnaire Completed";
        private const string completedMessage =
            "Greetings @(Model.DisplayName). The Questionnaire for Review Session '@(Model.SessionName)' has been completed by the reviewer. " +
            "Click <a href=\"@(Model.BaseUrl)Screens/Forum.html?reviewSession=@(Model.SessionId)\">here</a> " +
            "to return to the Review Session forum.";

        public NotificationGenerator()
        {
            mailInfoDictionary = new Dictionary<EventType, Tuple<string, string>> { 
                { EventType.ReviewSessionReleased, new Tuple<string, string>(releasedSubject, releasedMessage) }, 
                { EventType.ReviewerAssigned, new Tuple<string, string>(assignedSubject, assignedMessage) },
                { EventType.QuestionnaireCompleted, new Tuple<string, string>(completedSubject, completedMessage)}
            };
        }

        public MailMessage Generate(EventType eventType, NotificationArgs args)
        {
            var info = mailInfoDictionary[eventType];

            var email = Email
                .From("noreply@activerequirementsreview.apphb.com")
                .To(args.Recipient)
                .Subject(string.Format("Active Requirements Review - {0}", info.Item1))
                .UsingTemplate(info.Item2, new
                    {
                        BaseUrl = args.BaseUrl,
                        Recipient = args.Recipient,
                        SessionId = args.SessionId,
                        SessionName = args.SessionName,
                        DisplayName = args.DisplayName
                    });

            return email.Message;
        }
    }
}