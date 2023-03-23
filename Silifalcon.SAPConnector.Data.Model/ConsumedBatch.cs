// Silifalcon.SAPConnector.Data.Model.ConsumedBatch

public class ConsumedBatch
{
	public double Quantity { get; set; }

	public virtual Batch Batch { get; set; }
}
