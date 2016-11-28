using System;
using System.Threading;

namespace netSmtpMailer
{

	[System.Runtime.Remoting.Contexts.SynchronizationAttribute()]
	public class SmtpMail
	{
		public static string MAILTO;
		public static string MAILFROM;
		public static string SUBJECT;
		public static string BODYTEXT;
		public static string SMTPSERVER;
		public static bool HTMLBODYTYPE;
		
		public static void sendMail()
		{
			try {
				while (SmtpMailerThreadPool.ThreadPool == SmtpMailerThreadPool.ThreadPoolMax) {
					Thread.Sleep(10);
				}
	
				SmtpMailerThread sm = new SmtpMailerThread();
				sm.MAILTO = MAILTO;
				sm.MAILFROM = MAILFROM;
				sm.SUBJECT = SUBJECT;
				sm.BODYTEXT = BODYTEXT;
				sm.HTMLBODYTYPE = HTMLBODYTYPE;
				sm.SMTPSERVER = SMTPSERVER;
				Thread t = new Thread(new ThreadStart(sm.send));
				t.Name = "SmtpMailerThread2";
				SmtpMailerThreadPool.incrementThreadPool();
				t.Start();
			} catch (Exception e) {
				throw(e);
			}
		}
	}

}
