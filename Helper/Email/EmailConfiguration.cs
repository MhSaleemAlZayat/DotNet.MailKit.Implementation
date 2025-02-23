﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EmailSenderProject.Helper.Email
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int PortLocal { get; set; }
        public int PortPublished { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
