// Silifalcon.SAPConnector.Exceptions.SAPConnectionException
using System;
using System.Runtime.Serialization;

[Serializable]
public class SAPConnectionException : SAPException
{
	public SAPConnectionException(int code, string message)
		: base(code, message)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}
}
