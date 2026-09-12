# Four-Layer Architecture2（WinUI 3版）

Web API版の `Four-Layer Architecture` と同じTodoドメインを、WinUI 3のデスクトップUIから利用するサンプルです。

## 4層の責務

```text
TodoApp.WinUI（Presentation）
        ↓
TodoApp.Application（Application）
        ↓
TodoApp.Domain（Domain）
        ↑
TodoApp.Infrastructure（Infrastructure）
```

- **Presentation**: XAMLとViewModel。入力、表示、コマンドを担当します。
- **Application**: `ITodoService`を通してユースケースを実行します。
- **Domain**: `TodoItem`とドメインルールを保持します。
- **Infrastructure**: `InMemoryTodoRepository`でデータを保存します。

WinUI版はWeb API版のApplication、Domain、Infrastructureプロジェクトを参照し、Presentationだけを差し替えています。これにより、UIの変更がドメインルールや永続化処理に漏れません。

## 実行

リポジトリのルートで次を実行します。

```powershell
winapp run ".\Four-Layer Architecture2\TodoApp.WinUI\TodoApp.WinUI.csproj"
```

タイトルを入力して「追加」を押すと、ViewModelからApplication層のサービスを呼び出してタスクを登録できます。
