﻿using System.Net;
using System.Net.Mail;

namespace AgentCheker.Mail
{
    internal class Email
    {
        #region COSNTANTS

        private const string MailSubject = "Agent cheker";

        #endregion COSNTANTS

        #region FIELDS

        private string _body =
            @"
            <p>Titles was chanched for users:<br></p>

            <table border='1' align='Left' cellpadding='2' cellspacing='0' style='color:black;font-family:arial,helvetica,sans-serif;text-align:Ledt;'>
            <tr style = 'font-size:12px;font-weight: normal;background: #FFFFFF;background-color: #32CD32;' >
                <th align = Center>
                    <b>
                        Login
                    </b>
                </th>
                <th align = Center >
                    <b>
                        new Title
                    </b>
                </th>
                <th align = Center >
                    <b>
                        old Title
                    </b>
                </th >
                <th align = Center >
                    <b>
                        access to systems
                    </b>
                </th >
             </tr > ";

        private SmtpClient _smtp;
        private MailAddress _fromAddress;
        private MailAddress _toAddress;

        #endregion FIELDS

        #region CTORs

        public Email(
            string fromddress,
            string toAddress,
            string mailServer,
            string fromPass,
            int port)
        {
            _fromAddress = new MailAddress(fromddress);
            _toAddress = new MailAddress(toAddress);
            _smtp = new SmtpClient
            {
                Host = mailServer,
                Port = port,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials =
                    new NetworkCredential(fromddress, fromPass),
            };
        }

        #endregion CTORs

        ~Email()
        {
            _smtp.Dispose();
        }

        #region METHODS

        /// <summary>
        /// Send en email.
        /// </summary>
        /// <param name="body">
        /// Body message.
        /// </param>
        public void SendMail(string body)
        {
            using
            (
                MailMessage message = new MailMessage(_fromAddress, _toAddress)
                {
                    Subject = MailSubject,
                    Body = body,
                    IsBodyHtml = true,
                })
            {
                _smtp.Send(message);
            }
        }

        /// <summary>
        /// Processing mai.
        /// </summary>
        /// <param name="usersTbl">
        /// Table with users.
        /// </param>
        public void ProcessEmailBody(string[] usersTbl)
        {
            for (int i = 0; i < usersTbl.Length; i++)
            {
                string sasAMAccountName = usersTbl[i].Split(';')[0];
                string oldTitle = usersTbl[i].Split(';')[1];
                string newTitle = usersTbl[i].Split(';')[2];
                string systems = usersTbl[i].Split(';')[3];

                string row = string.Format(
                    $"" +
                    $"<tr style='font-size:12px;background-color:#FFFFFF'>" +
                    $"  <td>{sasAMAccountName}</td>" +
                    $"  <td>{newTitle}</td>" +
                    $"  <td>{oldTitle}</td>" +
                    $"  <td>{systems}</td>" +
                    $"</tr > ");

                _body += row;
            }

            SendMail(_body);
        }

        #endregion METHODS

    }
}