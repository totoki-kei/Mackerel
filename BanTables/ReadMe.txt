TShock BanTables プラグイン
(最終更新日: 2014/05/18 ServerApi = 1.16)

1. はじめに

これは、Terrariaサーバプログラム用プラグインTShockで使用できる
ban機能を拡張するプラグインです。


2. 導入方法

TShockのServerPluginフォルダに、BanTables.dll を配置してください。


3. 使い方

導入後にTerrariaServer.exeを起動すると「banex」コマンドが追加されます。
banexコマンドは、banパーミッションを持つユーザから見えるようになります。

何も引数を入れずに"/banex"コマンドを実行すると、簡易ヘルプを表示します。


3.1 banex list コマンド
現在設定されているアクセスコントロール(以下、「エントリ」と呼ぶ)の
リストを表示します。

表示例：
　Server executed: /banex list.
　BanTable entries
　0 ip(192.168.1.0/24) => Allow Reason : Local User
　1000 name(AdminUser) => Allow Reason : Admin
　2000 name(TrustedUser1) => Allow Reason : Safe user.
　2001 name(TrustedUser2) => Allow Reason : Friend.
　8000 hostname(.*\.ocn\.ne\.jp) => Deny Reason : Your remote host is in blacklist.
　8001 hostname(softbank.*) => Deny Reason : Your remote host is in blacklist.
　10000 name(.*) => Allow Reason : Allow all user
　Total 6 entries.

各エントリの表示内容は以下の通りです：
　<priority> <function>(<function_arg>) => <judge> [Reason : <reason_text>]

　<priority>
　　エントリの優先度、兼、ID番号です。数値が小さいほど優先されます。
　　重複しない32ビット符号付整数です。

　<function>
　　判定方法です。現在、以下の3つが利用可能です。
　　　ip       : IPアドレスとネットマスクによる判定
　　　           「<IPAddr>[/NetMask]」の形式で指定。マスク省略時は「/32」と同じ
　　　name     : キャラクター名の正規表現一致
　　　hostname : リモートホストの正規表現一致

　<function_arg>
　　判定に使用する文字列パラメータです。

　<judge>
　　判定結果が真であった時の動作です。
　　"Allow"の場合は以後のエントリを飛ばし、接続を許可します。
　　"Deny"の場合は接続を許可せず、Kickします。

　<reason_text>
　　<judge>が"Deny"の時に表示するKick理由です。
　　空の場合はここには表示されず、Kick理由として "You are banned." と表示します。
　　<judge>が"Allow"の時には使用されないので、メモを書けます。

「表示例」の場合、以下のような動作になります。
　・ローカルネットワーク(192.168.1.0/255.255.255.0 に属しているPC)からは接続可
　・キャラクター名が「AdminUser」「TrustedUser1」「TrustedUser2」は接続可
　・リモートホストがパターン「.*\.ocn\.ne\.jp」に一致する場合は接続不可
　・同様に、リモートホストがパターン「softbank.*」に一致する場合は接続不可
　・上記以外のユーザは接続可


3.2 banex add コマンド
エントリを追加します。

　コマンド書式：
　　banex add <priority> <function> <function_arg> [<judge> [<reason>]]

　<judge>と<reason>は省略可能です。
　<judge>を省略すると"Deny"になります。


3.3 banex del コマンド
指定の優先度を持ったエントリを削除します。

　コマンド書式：
　　banex del <priority>


3.4 banex move コマンド
エントリの優先度を変更します。

　コマンド書式：
　　banex move <old_priority> <new_priority>


3.5 banex test コマンド
動作テストを行います。テストデータの種類と値を入力します。

　コマンド書式：
　　banex move <function> <value>


4. テーブル
このプラグインは、TShockのデータベース内に「BanEx」という名前のテーブルを追加します。
SqlTableの宣言はこんな感じです。

　table = new SqlTable("BanEx",
　　new SqlColumn("Priority", MySqlDbType.Int32) { NotNull = true, Unique = true, Primary = true },
　　new SqlColumn("Judge", MySqlDbType.String, 8) { NotNull = true, DefaultValue = "Deny" },
　　new SqlColumn("Predicate", MySqlDbType.String, 64) { NotNull = true },
　　new SqlColumn("Parameter", MySqlDbType.Text),
　　new SqlColumn("Reason", MySqlDbType.Text)
　);


5. 再配布などについて
再配布は自由に行っていただいて構いません。
改変なども自由です。

6. 更新履歴

2014/05/29
	Terraria 1.2.4.x / TShockAPI 1.16 対応
	listコマンドにページャ機能を付加。
	ページャをつけた関係上、リストの表示方法を１エントリ１行に変更。
	接続を許可/不許可した時に、どのエントリで判定されたかをログメッセージに出力するよう変更。
	IPアドレス判定時のアドレス部、ネットマスク部の内部的な解釈方法を変更。
	IPアドレス判定時にネットマスクとして4オクテットの形式(255.255.0.0等)を使用できるよう変更。
	ホスト名を逆引きした結果を数分間キャッシュするように変更。
	サーバコンソール以外からの実行を許可。

2014/02/16
	Terraria 1.2.3 / TShockAPI 1.15 対応
	addコマンドによるエントリ追加時、<function>が有効な判定方法でない場合はエラーとするように修正。

2014/01/08
	testコマンド追加

2013/10/07
	Terraria 1.2.0.1 / TShock 4.2.0.1005 対応

2013/08/18
	初版公開
