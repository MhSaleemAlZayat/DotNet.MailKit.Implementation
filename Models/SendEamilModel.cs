using System.ComponentModel.DataAnnotations;

namespace EmailSenderProject.Models;

public class SendEamilModel
{
    public string Sender { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }


}
