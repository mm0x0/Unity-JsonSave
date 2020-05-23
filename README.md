# JsonSave.cs

## 説明
- クラスとシリアライズされたメンバ変数をJsonでまるごと保存
- セーブスロットの作成・削除・コピー機能
- データの保存場所
  - Editor    ... [プロジェクトルート]/SaveData/
  - Editor以外 ... [Application.persistentDataPathのパス]/SaveData/

## 動作環境
- C# 6.0以上

## 使い方

- セーブスロットの作成
  ```
  JsonSave.CreateSlot (1);
  ```

- セーブ
  ```
  MyClass myClass = new MyClass ();

  JsonSave.Save<MyClass> (myClass);    // セーブスロット1に保存
  JsonSave.Save<MyClass> (myClass, 2); // セーブスロット2に保存
  ```

- ロード
  ```
  MyClass myClass = JsonSave.Load<MyClass> ();  // セーブスロット1からロード
  MyClass myClass = JsonSave.Load<MyClass> (2); // セーブスロット2からロード
  ```

- セーブスロットの削除
  ```
  JsonSave.DeleteSlot (1);
  ```

- セーブスロットのコピー
  ```
  JsonSave.CopySlot (1, 2);
  ```

- セーブスロットの最大数を設定（初期値は2）
  ```
  JsonSave.SetMaxSlotNum (5);
  ```

- セーブスロットがあるか判別
  ```
  JsonSave.HasSlot (2);
  ```

- セーブデータがあるか判別
  ```
  JsonSave.HasSaveData ();
  ```
