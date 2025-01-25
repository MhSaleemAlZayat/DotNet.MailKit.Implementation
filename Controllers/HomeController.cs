using EmailSenderProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EmailSenderProject.Helper.Email;
using EmailSenderProject.Helper.Email.EmailSender;
using EmailSenderProject.Helper.ViewRender;
using EmailSenderProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace EmailSenderProject.Controllers
{
    public class HomeController :Controller
    {
         private readonly IEmailSenderSerivce _emailService;
        private readonly IViewRenderService _viewRenderService;

        public HomeController(IEmailSenderSerivce emailService, IViewRenderService viewRenderService) 
        {
            _emailService = emailService;
            _viewRenderService = viewRenderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(SendEamilModel model)
        {
            try
            {
                UserRegistrationConfirmationViewModel viewModel = new() { Url = "" };

                var renderedView = await _viewRenderService.RenderToStringAsync("Template/UserRegistrationConfirmationTemplate", viewModel);

                var emailContent = new EmailContent()
                {
                    To = new List<MailboxAddress> { new MailboxAddress(model.Email) },
                    Subject = "<Subject>",
                    Content = renderedView,
                    Attachments = new List<MimePart> {
                             new MimePart (){
                                ContentId = viewModel.MinistryLogo.Replace("cid:", ""),
                                Content = new MimeContent(new MemoryStream(System.IO.File.ReadAllBytes("<Message Directory>"))),
                                ContentTransferEncoding = ContentEncoding.Base64,
                                ContentDisposition = new ContentDisposition(ContentDisposition.Inline)
                            }
                        }

                };

                await _emailService.SendEmailAsync(emailContent);

                
                return RedirectToAction("AddUser");
            }
            catch (Exception exp)
            {
                throw;                
            }
        }


      
    }
}
