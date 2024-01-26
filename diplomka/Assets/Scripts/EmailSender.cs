//https://sushanta1991.blogspot.com/2021/10/how-to-send-email-from-unity-using.html
using System.ComponentModel;
using System.Net.Mail;
using UnityEngine;
public class EmailSender
{
    public void Send(string mailAddressTo, string text, string subject)
    {
        var client = new SmtpClient("smtp.gmail.com", 587);
        client.Credentials = new System.Net.NetworkCredential(
            "eduenvifmfi@gmail.com", 
            "pkhcikrheeespoyu");
        client.EnableSsl = true;
        var from = new MailAddress(
            "eduenvifmfi@gmail.com",
            "eduenvi",
            System.Text.Encoding.UTF8);
        var to = new MailAddress(mailAddressTo);
        var message = new MailMessage(from, to);
        message.Body = text;
        message.BodyEncoding = System.Text.Encoding.UTF8;
        message.Subject = subject;
        message.SubjectEncoding = System.Text.Encoding.UTF8;
        client.SendCompleted += SendCompletedCallback;
        var userState = "message";
        client.SendAsync(message, userState);
    }

    public void SendPassword(string mailAddressTo, string login, string newPassword)
    {
        var subject = "Zabudnuté heslo";
        var text = "Dobrý deň " + login + ",\ntoto je vaše nové heslo: " + newPassword + "\nPo prihlásení si ho zmeňte!"; //TODO zmen
        Send(mailAddressTo, text, subject);
    }
    
    public void SendWelcome(string mailAddressTo, string login)
    {
        var subject = "Vitajte v EduEnvi";
        var text = "Dobrý deň " + login + ",\nVitajte v EduEnvi!"; //TODO zmen
        Send(mailAddressTo, text, subject);
    }
    private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        // Get the unique identifier for this asynchronous operation.
        string token = (string)e.UserState;
        if (e.Cancelled) Debug.Log("Send canceled "+ token);
        if (e.Error != null) Debug.Log("[ "+token+" ] " + " " + e.Error.ToString());
        else Debug.Log("Message sent.");
    }
}