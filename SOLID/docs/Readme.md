# レポート出力システム (SOLID原則 研修用サンプル)

本システムは、オブジェクト指向設計における**SOLID原則**を深く理解するための研修用サンプルアプリケーションです。
同一の機能要件に対して、意図的に原則に違反した「**Badパターン**」と、原則を綺麗に適用した「**Goodパターン**」の2つの実装を包含しており、設計の違いによる拡張性やテスト容易性の差を体感学習できます。

---

## 1. システム概要 & 機能要件

業務で頻出する典型的な処理フローである「**売上データの取得 → 特定フォーマットへの整形 → 指定された出力先への配送**」を行います。

### 核心となる3つのステップ
1.  **データ取得 (DataAccess)**
    * 商品名（`Product`）と売上金額（`Amount`）を持つ売上レコードを取得します。
    * ※研修用のため、実DB接続ではなくインメモリのダミーデータ（Apple: 100, Banana: 200, Cherry: 300）を使用します。
2.  **データ整形 (Formatters)**
    * 取得した売上データを以下のいずれかの形式のテキストに変換します。
        * **CSV**: ヘッダー付きのカンマ区切り文字列
        * **JSON**: `System.Text.Json` を用いたオブジェクト配列形式
        * **Markdown**: 読みやすい表形式（テーブル文字列）
3.  **データ出力 (Output)**
    * 整形された文字列を指定の方法で出力・送信します。
        * **Console**: 標準出力へ表示
        * **File**: 実行ディレクトリ等の指定パスへテキストファイルとして書き出し
        * **Email**: 指定された宛先へメール送信（※実送信ではなくコンソールへの擬似ログ出力）

---

## 2. 開発環境・前提条件

* **言語・フレームワーク**: C# (.NET 10)
* **アプリケーション形態**: コンソールアプリケーション
* **主要な依存パッケージ** (Good版・テスト用):
    * `Microsoft.Extensions.DependencyInjection` (DIコンテナの構築)
    * `xunit` (テストフレームワーク)
    * `FluentAssertions` (直感的なアサーション)
    * `Moq` (インターフェースのモック化)

---

## 3. プロジェクト構成 & コードポインタ

プロジェクトは大きく「Badパターン」「Goodパターン」「単体テスト」の3つに分かれています。

```

SolidSample/
├── SolidSample.Bad/               # 【アンチパターン】SOLID原則違反の塊
│   ├── Program.cs                 # 動作確認用エントリーポイント
│   └── ReportManager.cs           # すべての処理を抱えるGodクラス
│
├── SolidSample.Good/              # 【クリーン設計】SOLID原則を遵守
│   ├── Program.cs                 # Composition Root (DIコンテナの組み立て・起動)
│   ├── Domain/
│   │   └── SalesRecord.cs         # 単一責任のデータモデル (record型)
│   ├── DataAccess/
│   │   ├── ISalesRepository.cs    # データアクセスの抽象 (DIP/ISP)
│   │   └── InMemorySalesRepository.cs
│   ├── Formatters/
│   │   ├── IReportFormatter.cs    # 整形処理の拡張ポイント (OCP/LSP)
│   │   ├── CsvFormatter.cs
│   │   ├── JsonFormatter.cs
│   │   └── MarkdownFormatter.cs
│   ├── Output/
│   │   ├── IConsoleWriter.cs      # 出力先ごとに細分化されたIF (ISP)
│   │   ├── IFileWriter.cs
│   │   ├── IEmailSender.cs
│   │   ├── ConsoleWriter.cs
│   │   ├── FileWriter.cs
│   │   └── SmtpEmailSender.cs
│   └── Services/
│       └── ReportService.cs       # 抽象にのみ依存するユースケース調整役 (SRP/DIP)
│
└── SolidSample.Tests/             # 単体テストプロジェクト
├── OutputTests.cs             # 出力クラスのテスト
├── ReportFormatterTests.cs    # 各種フォーマッターの個別テスト
├── ReportManagerTests.cs      # Bad版の挙動テスト（外部依存が強くテストが困難）
├── ReportServiceTests.cs      # Good版のテスト（Moqを用いて完全に独立したテストが可能）
├── SalesRecordTests.cs
└── SalesRepositoryTests.cs

```

---

## 4. SOLID原則に基づく設計比較

### ❌ Badパターン (`SolidSample.Bad`) の問題点
* **SRP (単一責任の原則) 違反**: `ReportManager` がデータ取得、全フォーマットの整形、全出力先への操作をすべて抱え込んでおり、変更理由が多すぎます。
* **OCP (開放閉鎖の原則) 違反**: 新しいフォーマット（例: XML）や新しい出力先を追加する際、既存の `GenerateAndDeliverReport` メソッド内の `if-else` 分岐を直接書き換える必要があり、デグレのリスクが高まります。
* **LSP (リスコフの置換原則) 違反**: 「PDF形式の場合はコンソール出力不可」というサブタイプ特有の制約をメソッド内で強制し、`NotSupportedException` をスローする契約違反の伏線があります。
* **DIP (依存関係逆転の原則) 違反**: クラス内部に接続文字列やSMTPサーバー設定をハードコードし、具体的な実装や外部環境へ直接依存しているため、モック差し替えができずテストが非常に困難です。

### ✅ Goodパターン (`SolidSample.Good`) の解決策
* **SRP**: 各クラスは「データの保持」「CSVへの変換」「ファイルへの書き出し」など、完全に1つの責任に特化しています。
* **OCP**: `IReportFormatter` インターフェースにより拡張ポイントを明確化。新たなフォーマットを追加する際は、既存コードを一切修正せず、新しい具象クラスを追加して `Program.cs` でDI登録するだけで拡張可能です。
* **LSP**: 整形処理（文字列を生成する契約）と出力処理（受け取った文字列を届ける契約）を分離。どのフォーマッターも堅牢に契約を履行し、例外を投げて呼び出し側を脅かすことはありません。
* **ISP (インターフェース分離の原則)**: 巨大なひとまとめのIFを作るのではなく、`IFileWriter` や `IEmailSender` のようにクライアントが必要とする最小限のメソッドに分割しています。
* **DIP**: 最上位の業務ロジックを司る `ReportService` は、具象クラスを一切 `new` せず、すべてインターフェース（抽象）を介して会話します。具象の結合は起動地点の `Program.cs` (Composition Root) のみに集約されています。

---

## 5. 実行・テスト方法

### アプリケーションの実行
それぞれのディレクトリに移動し、以下のコマンドでコンソールアプリケーションを実行できます。

* **Bad版の実行**:
    ```bash
    cd SolidSample.Bad
    dotnet run
    ```
* **Good版の実行**:
    ```bash
    cd SolidSample.Good
    dotnet run
    ```

### 単体テストの実行
プロジェクトのルート、または `SolidSample.Tests` ディレクトリにて以下を実行します。
```bash
dotnet test
```

* `ReportServiceTests.cs` を参照することで、DIPの恩恵により `Moq` を使っていかに簡単にビジネスロジックの網羅テストが書けるかを確認できます。
