using AgentChecker.DataBase;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace AgentChecker.Mail
{
    public class Email
    {
        #region COSNTANTS

        private const string MailSubject = "Agent checker";

        #endregion COSNTANTS

        #region FIELDS

        private string _headTable =
            @"
            <table border='1' align='Left' cellpadding='2' cellspacing='0' style='color:black;font-family:arial,helvetica,sans-serif;text-align:Ledt; '>
            <tr style = 'font-size:12px;font-weight: normal;background: #FFFFFF;background-color: #32CD32;' >
                <th align = Center>
                    <b>
                        PC name
                    </b>
                </th>
                <th align = Center >
                    <b>
                        last connect time
                    </b>
                </th>
             </tr > ";

        private string _body;

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

        public void SendMail()
        {
            using
            (
                MailMessage message = new MailMessage(_fromAddress, _toAddress)
                {
                    Subject = MailSubject,
                    Body = _body,
                    IsBodyHtml = true,
                })
            {
                _smtp.Send(message);
            }
        }

        public void ProcessEmailBody(string serverName, List<PC> pCs)
        {
            _body += $"ПК, которые не подключались к серверу {serverName} " +
                $"более 14 дней, но доступны по сети<br>";

            _body += _headTable;
            //_body = string.Empty;

            foreach (PC p in pCs)
            {
                string row = string.Format(
                       $"<tr style='font-size:12px;background-color:#FFFFFF'>" +
                       $"  <td>{p.PcName}</td>" +
                       $"  <td>{p.LastConnectionTime}</td>" +
                       $"</tr > ");

                _body += row;
            }

            _body += "</table>";
        }

        #endregion METHODS

    }
}
