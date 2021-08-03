namespace QuazalWV
{
	public class Privilege
	{
		public uint m_ID { get; set; }
		public string m_description { get; set; }
	}

	public class PrivilegeEx
	{
		public uint m_ID { get; set; }
		public string m_description { get; set; }
		public ushort m_duration { get; set; }
	}
}
