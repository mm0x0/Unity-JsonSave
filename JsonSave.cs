using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class JsonSave
{
	static string saveFolderDev    = "SaveData/";
	static string saveFolderMaster = Application.persistentDataPath + "/SaveData/";

	// セーブスロットはデフォルトで2つ
	static int maxSlotNum = 2;

	// セーブ
	public static void Save<T> (T data, int slot = 1)
	{
		// スロットがなかったらエラー
		if (!HasSlot (slot))
		{
			Debug.LogError ("スロット" + slot + "がないよ〜");
			return;
		}

		// JSONテキストに変換して書き込み
		string jsonTxt  = JsonUtility.ToJson (data);
		string fileName = typeof (T).Name;
		File.WriteAllText (GetSaveFilePath (fileName, slot), jsonTxt);
	}

	// ロード
	public static T Load<T> (int slot = 1) where T : new ()
	{
		// スロットがなかったらエラー
		if (!HasSlot (slot))
			Debug.LogError ("スロット" + slot + "がないよ〜");

		// JSONをT型のクラスに変換して返す
		return (T) JsonUtility.FromJson<T> (GetJson<T> (slot));
	}

	// スロットがあるか判別
	public static bool HasSlot (int slot) =>
		Directory.Exists (GetSaveDirectory () + slot);

	// セーブデータがあるか判別
	public static bool HasSaveData ()
	{
		bool hasData = false;

		for (int i = 1; i <= maxSlotNum; i++)
			if (HasSlot (i))
				hasData = true;

		return hasData;
	}

	// スロットの作成
	public static void CreateSlot (int slot = 1)
	{
		if (slot <= 0 || slot > maxSlotNum)
		{
			Debug.LogError ("使えるスロットは1〜" + maxSlotNum + "までだよ〜");
			return;
		}

		// スロットがある場合は何もしない
		if (HasSlot (slot))
		{
			Debug.LogWarning ("スロット" + slot + "はもうあるよ〜");
			return;
		}

		Directory.CreateDirectory (GetSaveDirectory () + slot);
	}

	// スロットの削除
	public static void DeleteSlot (int slot = 1)
	{
		// フォルダがない場合は終了
		if (!HasSlot (slot))
		{
			Debug.LogWarning ("MY : JsonSave >　スロット" + slot + "はなかったよ〜");
			return;
		}

		// セーブデータをフォルダごと削除
		string slotDir = GetSaveDirectory () + slot;
		Directory.Delete (slotDir, true);
	}

	// スロットのコピー
	public static void CopySlot (int copySlot, int pasteSlot)
	{
		if (pasteSlot <= 0 || pasteSlot > maxSlotNum)
		{
			Debug.LogError ("使えるスロットは1〜" + maxSlotNum + "までだよ〜");
			return;
		}

		// コピー・ペーストパスを取得
		string copyPath  = GetSaveDirectory () + copySlot  + "/";
		string pastePath = GetSaveDirectory () + pasteSlot + "/";

		// ペースト先にデータがあれば削除
		if (HasSlot (pasteSlot))
			DeleteSlot (pasteSlot);

		// スロットを作成
		CreateSlot (pasteSlot);

		// ファイルを1つづつコピー
		foreach (var file in Directory.GetFiles (copyPath))
			File.Copy (file, Path.Combine (pastePath, Path.GetFileName (file)));
	}

	// スロットにデータがあるか判別
	public static bool HasData<T> (int slot)
	{
		string fileName = typeof (T).Name;

		return File.Exists (GetSaveFilePath (fileName, slot));
	}

	// JSONテキストを取得
	static string GetJson<T> (int slot) where T : new ()
	{
		string text;
		string fileName = typeof (T).Name;

		// ファイルがない場合はエラー
		if (!HasData<T> (slot))
		{
			Debug.LogError ("" + fileName + ".jsonがないよ〜");
			return "";
		}

		// データを取得
		text = File.ReadAllText (GetSaveFilePath (fileName, slot));

		return text;
	}

	// 保存パスを取得
	static string GetSaveFilePath (string fileName, int slot) =>
		GetSaveDirectory () + slot + "/" + fileName + ".json";

	// 保存ディレクトリルートを取得
	static string GetSaveDirectory ()
	{
		string saveDir = "";

		#if UNITY_EDITOR
		saveDir = saveFolderDev;

		#else
		saveDir = saveFolderMaster;

		#endif

		return saveDir;
	}

	// セーブスロットの個数を設定
	public static void SetMaxSlotNum (int num) =>
		maxSlotNum = num;
}