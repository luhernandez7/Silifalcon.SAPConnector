// Silifalcon.SAPConnector.Data.Model.Transfer
using System.Collections.Generic;
using System.Linq;

public class Transfer
{
	public int DocEntry { get; set; }

	public string Comments { get; set; }

	public List<TransferItem> Items { get; set; } = new List<TransferItem>();


	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		Transfer transfer = (Transfer)obj;
		return EqualityComparer<int>.Default.Equals(DocEntry, transfer.DocEntry) && EqualityComparer<string>.Default.Equals(Comments, transfer.Comments) && ((Items == null && transfer.Items == null) || Items.SequenceEqual(transfer.Items));
	}

	public override int GetHashCode()
	{
		int num = 1229280785;
		num = num * -1521134295 + DocEntry.GetHashCode();
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Comments);
		return num * -1521134295 + EqualityComparer<List<TransferItem>>.Default.GetHashCode(Items);
	}
}
