namespace GitHubSharp
{
    public interface ISerializer
    {
		string Serialize(object o);

		TData Deserialize<TData>(string data);
    }
}

