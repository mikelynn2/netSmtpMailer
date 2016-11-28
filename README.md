# netSmtpMailer
```
Copyright (c) by respective owners. All rights reserved.  Released under license as described in the file LICENSE.txt
```

Simple multi-threaded replacement for System.web.mail. Its is fast/stable because it only implements 1 smtp method - (200+ msg/sec P4-3GHZ). It is mainly designed for message injection to another SMTP server for end delivery. Written in 100% managed C#.

Moved From https://sourceforge.net/projects/netsmtpmailer/


## VB Example
```
Imports netSmtpMailer

''''' YOU ONLY NEED TO CALL THIS METHOD ONCE - EVER PER COMPUTER
''''' IT SETS UP THE PERFORMANCE COUNTER
''''' IT DOESN'T HURT TO CALL IT ONCE IN YOUR APPLICATION AT STARTUP
''''' BUT DON'T DO IT ON A PER MESSAGE BASIS - THAT WOULD SLOW YOUR MAILINGS DOWN

PerformanceMonitor.InstallPerformanceCounter("SmtpMailer", "Messages Per Second", Diagnostics.PerformanceCounterType.RateOfCountsPerSecond64)


''' THE THREAD POOL DEFAULTS TO 50 CONCURRENT THREADS - YOU CAN CHANGE IT HIGHER IF YOU WANT FOR WHATEVER YOUR SYSTEM WILL ALLOW.
SmtpMailerThreadPool.SmtpMailerThreadPoolMax = 200
SmtpMailer.MAILTO = "test@yourdomain.com"
SmtpMailer.MAILFROM = "test@yourdomain.com"
SmtpMailer.SUBJECT = "Dang this thing is fast"
SmtpMailer.BODYTEXT = "Did you get this email?"
SmtpMailer.BODYTYPE = 0     '1 = html 0 = text
SmtpMailer.SMTPSERVER = "10.1.1.21"  '"mx3.mail.yahoo.com" ' '
SmtpMailer.SendMail()
```



## c# Example

```
Imports netSmtpMailer

///// YOU ONLY NEED TO CALL THIS METHOD ONCE - EVER PER COMPUTER
///// IT SETS UP THE PERFORMANCE COUNTER
///// IT DOESN'T HURT TO CALL IT ONCE IN YOUR APPLICATION AT STARTUP
///// BUT DON'T DO IT ON A PER MESSAGE BASIS - THAT WOULD SLOW YOUR MAILINGS DOWN

PerformanceMonitor.InstallPerformanceCounter("SmtpMailer", "Messages Per Second", Diagnostics.PerformanceCounterType.RateOfCountsPerSecond64);


//// THE THREAD POOL DEFAULTS TO 50 CONCURRENT THREADS - YOU CAN CHANGE IT HIGHER IF YOU WANT FOR WHATEVER YOUR SYSTEM WILL ALLOW.


SmtpMailerThreadPool.SmtpMailerThreadPoolMax = 200;
SmtpMailer.MAILTO = "test@yourdomain.com";
SmtpMailer.MAILFROM = "test@yourdomain.com";
SmtpMailer.SUBJECT = "Dang this thing is fast";
SmtpMailer.BODYTEXT = "Did you get this email?";
SmtpMailer.BODYTYPE = 0;
SmtpMailer.SMTPSERVER = "10.1.1.21";
SmtpMailer.SendMail();
```


