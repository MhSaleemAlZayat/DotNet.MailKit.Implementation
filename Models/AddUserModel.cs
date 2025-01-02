using System.ComponentModel.DataAnnotations;

namespace EmailSenderProject.Models;

public class AddUserModel
{
    public string Email { get; set; }

    public string UserName { get; set; }

}
