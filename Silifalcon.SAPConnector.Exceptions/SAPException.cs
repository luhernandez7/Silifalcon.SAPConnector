// Silifalcon.SAPConnector.Exceptions.SAPException
using System;
using System.Runtime.Serialization;

[Serializable]
public class SAPException : Exception
{
	public int SAPErrorCode { get; set; }

	public string SAPErrorMessage { get; set; }

	public SAPException(int code, string message)
		: base("[" + code + "]: " + message)
	{
		SAPErrorCode = code;
		SAPErrorMessage = message;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("SAPErrorCode", SAPErrorCode);
		info.AddValue("SAPErrorMessage", SAPErrorMessage);
	}
}
