namespace MackerelPluginSet.Common
{
    enum ConfigurationLoadMode
    {
		/// <summary>
		/// 読み込みに失敗したらその際の例外を送出します
		/// </summary>
		ThrowIfFailed,
		/// <summary>
		/// 読み込みに失敗したらnullを返します
		/// </summary>
		NullIfFailed,
		/// <summary>
		/// 読み込みに失敗したらデフォルトコンストラクタで初期化したインスタンスを返します
		/// </summary>
		DefaultIfFailed,
		/// <summary>
		/// 読み込みに失敗したら空のファイルを作成し、デフォルトコンストラクタで初期化したインスタンスを返します
		/// </summary>
		CreateIfFailed,
    }
}
