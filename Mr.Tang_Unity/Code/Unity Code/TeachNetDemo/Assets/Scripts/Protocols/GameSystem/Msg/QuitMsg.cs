using System.Collections.Generic;

namespace GameSystem
{
	public class QuitMsg : BaseMsg
	{

		public override byte[] Writing()
		{
			byte[] body = base.Writing();
			byte[] bytes = new byte[4 + 4 + body.Length];
			int index = 0;
			WriteInt(bytes, GetID(), ref index);
			WriteInt(bytes, body.Length, ref index);
			body.CopyTo(bytes, index);
			return bytes;
		}
		public override int GetID()
		{
			return 1003;
		}
	}
}