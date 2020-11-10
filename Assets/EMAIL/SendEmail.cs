using System.Collections;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Threading;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

public class SendEmail : MonoBehaviour
{
    static string pathToDelete = "";

    public void SendSimple()
    {
        string email = "lucasbenvenuti2010@gmail.com";
        string subject = MyEscapeURL("My Subject");
        string body = MyEscapeURL("My Body\r\nFull of non-escaped chars");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public static void Send(string fileName)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("lucasbenvenuti2010@gmail.com");
        mail.To.Add("lucasbenvenuti2010@gmail.com");
        mail.Subject = "Subject";
        mail.Body = "Teste";

        string pathToInstance = Path.Combine(Application.persistentDataPath, fileName);
        pathToDelete = pathToInstance;

        mail.Attachments.Add(new Attachment(pathToInstance));

        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
        smtp.Port = 587;
        smtp.Credentials = new System.Net.NetworkCredential("lucasbenvenuti2010@gmail.com", "10071996") as ICredentialsByHost;
        smtp.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

        smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

        // smtp.Send(mail);

        string userState = "send message";
        smtp.SendAsync(mail, userState);

        //START HERE WHATEVER ANIMATION TO WAIT FOR MAIL SENT
    }

    private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        // Get the unique identifier for this asynchronous operation.
        string token = (string)e.UserState;

        //STOP HERE WHATEVER ANIMATION TO WAIT FOR MAIL SENT

        if (e.Cancelled)
        {
            Debug.Log("[{0}] Send canceled. " + token);
        }
        if (e.Error != null)
        {
            Debug.Log("[{0}] {1} " + token + " " + e.Error.ToString());
        }
        else
        {
            Debug.Log("Message sent.");

            foreach (var file in Directory.GetFiles(Application.persistentDataPath))
            {
                FileInfo file_info = new FileInfo(file);
                try
                {
                    file_info.Delete();
                }
                catch
                {
                    Debug.Log("File Delete Error! Probably is in use.");
                }
            }
        }
    }
}
