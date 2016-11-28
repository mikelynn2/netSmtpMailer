using System;
using System.Diagnostics;

namespace netSmtpMailer
{
public class PerformanceMonitor
{

	public static void installPerformanceCounter()
	{
	
		CounterCreationDataCollection CounterDatas = new System.Diagnostics.CounterCreationDataCollection();
		CounterCreationData cdCounter1 = new System.Diagnostics.CounterCreationData();
		try {
			cdCounter1.CounterName = "Messages Per Second";
			cdCounter1.CounterType = PerformanceCounterType.RateOfCountsPerSecond64;
			CounterDatas.Add(cdCounter1);
			if (!((PerformanceCounterCategory.Exists("netSmtpMail")))) {
				PerformanceCounterCategory.Create("netSmtpMail", "", CounterDatas);
			}
		} catch (Exception e) {
			throw (e);
		}
	}

}
}
