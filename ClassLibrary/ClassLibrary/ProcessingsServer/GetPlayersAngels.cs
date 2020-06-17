namespace ClassLibrary.ProcessingsServer
{
	public class GetPlayersAngels : ProcessingServer
	{
		public double angels;
		ModelServer Model;
		public override void Process(ModelServer Model)
		{
			this.Model = Model;
			if (Model.ListUsers[num] != null)
				Model.ListUsers[num].Rotate = angels;

		}
	}
}
