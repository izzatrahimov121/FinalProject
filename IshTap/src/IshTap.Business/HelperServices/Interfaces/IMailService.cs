using IshTap.Business.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IshTap.Business.HelperServices.Interfaces;

public interface IMailService
{
    Task SendEmailAsync(MailRequestDto mailRequest);
}
