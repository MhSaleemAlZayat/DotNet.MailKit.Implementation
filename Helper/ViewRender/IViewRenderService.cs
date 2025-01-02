using System;
using System.Threading.Tasks;

namespace EmailSenderProject.Helper.ViewRender
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}