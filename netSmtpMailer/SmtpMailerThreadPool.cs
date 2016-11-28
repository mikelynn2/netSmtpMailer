
namespace netSmtpMailer
{

[System.Runtime.Remoting.Contexts.SynchronizationAttribute()]
public class SmtpMailerThreadPool
{
	public static int ThreadPool;
	public static int ThreadPoolMax = 50;

	public static void incrementThreadPool()
	{
		ThreadPool++;
	}

	public static void decrementThreadPool()
	{
		ThreadPool--;
	}

}

}
