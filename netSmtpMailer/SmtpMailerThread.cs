using System;
using System.Text;
//using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace netSmtpMailer
{
public class SmtpMailerThread
{
	private const int RECEIVE_TIMEOUT = 10000;
	private const int PAYLOAD_RECEIVE_TIMEOUT = 60000;

	public string MAILTO = "";
	public string MAILFROM = "";
	public string SUBJECT = "";
	public string BODYTEXT = "";
	public string SMTPSERVER = "";
	public bool HTMLBODYTYPE = false;
	
	PerformanceCounter myCounter = new PerformanceCounter("netSmtpMail", "Messages Per Second", false);

	private enum SMTPResponse : int	{
		// ALL POSSIBLE RESPONSES FROM MAIL SERVERS SHOULD BE DEFINED HERE
		CONNECT_SUCCESS = 220,
		GENERIC_SUCCESS = 250,
		DATA_SUCCESS = 354,
		QUIT_SUCCESS = 221
	}

	public void send() {
		try {
			
			// ATTEMPT TO CONNECT TO SERVER
			IPHostEntry ipHost = Dns.Resolve(SMTPSERVER);
			IPEndPoint endPt = new IPEndPoint(ipHost.AddressList[0], 25);
			Socket s = new Socket(endPt.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, RECEIVE_TIMEOUT);
			s.Connect(endPt);
			if (!(checkResponse(s, SMTPResponse.CONNECT_SUCCESS))) {
				s.Close();
			}

			// INITIAL SMTP COMMANDS
			sendData(s, string.Format("HELO {0}\r\n", Dns.GetHostName() ));
			if (!(checkResponse(s, SMTPResponse.GENERIC_SUCCESS))) {
				s.Close();
			}
			sendData(s, string.Format("MAIL FROM: <{0}>\r\n", MAILFROM ));
			if (!(checkResponse(s, SMTPResponse.GENERIC_SUCCESS))) {
				s.Close();
			}
			sendData(s, string.Format("RCPT TO: <{0}>\r\n", MAILTO ));
			if (!(checkResponse(s, SMTPResponse.GENERIC_SUCCESS))) {
				s.Close();
			}
			
			// BUILDING BODY OF EMAIL
			StringBuilder Header = new StringBuilder();
			Header.Append("From: ");
			Header.Append(MAILFROM);
			Header.Append("\r\n");
			Header.Append("To: ");
			Header.Append(MAILTO);
			Header.Append("\r\n");
			Header.Append("Subject: ");
			Header.Append(SUBJECT);
			Header.Append("\r\n");

			// CHECK FOR HTML BODY
			if (HTMLBODYTYPE) {
				Header.Append("Mime-Version: 1.0\r\n");
				Header.Append("Content-Type: text/html; charset=\"ISO-8859-1\"\r\n");
			}

			// IF BODY DOESN'T END WITH CR LF - THEN ADD IT
			if(!BODYTEXT.EndsWith("\r\n")){
				BODYTEXT+="\r\n";
			}
			
			// DATA COMMAND & BODY
			sendData(s, ("DATA\r\n"));
			if (!(checkResponse(s, SMTPResponse.DATA_SUCCESS))) {
				s.Close();
			}
			
			Header.Append( "\r\n" );
			Header.Append( BODYTEXT );
			Header.Append( ".\r\n" );
			Header.Append( "\r\n" );
			Header.Append( "\r\n" );
			sendData(s, Header.ToString());
			s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, PAYLOAD_RECEIVE_TIMEOUT);
			if (!(checkResponse(s, SMTPResponse.GENERIC_SUCCESS))) {
				s.Close();
			}

			// QUIT
			sendData(s, "QUIT\r\n");
			s.Close();

			myCounter.Increment();
			
		} catch (Exception e) {
			myCounter.Close();
			throw(e);

		} finally {
			try {
				SmtpMailerThreadPool.decrementThreadPool();
				myCounter.Close();
			} catch (Exception e) {
				throw(e);
			}
		}
		
	}

	
	private static void sendData(Socket s, string msg) { 
		try {
			byte[] _msg = Encoding.ASCII.GetBytes(msg);
			s.Send(_msg , 0, _msg .Length, SocketFlags.None);
		} catch (Exception e) {
			throw (e);
		}
	}

	private bool checkResponse(Socket s, SMTPResponse response_expected) {
		try {
			string sResponse;
			int response;
			byte[] bytes = new byte[1024];
//			while (s.Available==0) {
//				System.Threading.Thread.Sleep(100);
//			}
			int len = s.Receive(bytes, SocketFlags.None);
			sResponse = Encoding.ASCII.GetString(bytes, 0, len);
			response = Convert.ToInt32(sResponse.Substring(0,3));
			if(response != (int)response_expected) {
				return false;
			}
			else {
				return true;
			} 
		} catch (Exception e) {
				throw(e);
			}
	}
}
}
