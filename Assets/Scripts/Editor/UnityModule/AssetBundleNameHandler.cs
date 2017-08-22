using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityModule {

    /// <summary>
    /// AssetBundleName を操作します
    /// </summary>
    /// <remarks>Variant は非対応です。</remarks>
    public class AssetBundleNameHandler {

        /// <summary>
        /// AssetBundle 名のフォーマット
        /// </summary>
        private const string ASSET_BUNDLE_NAME_FORMAT = "{0}.unity3d";

        /// <summary>
        /// AssetBundleName を設定する
        /// </summary>
        /// <remarks>アセットのパスをそのまま AssetBundleName に設定します。</remarks>
        /// <remarks>ディレクトリには設定せず、配下のアセットに対して個別に設定します。</remarks>
        [MenuItem("Project/AssetBundle/Apply AssetBundleName")]
        public static void ApplyAssetBundleNameToSelection() {
            HandleAssetBundleNameToSelection(
                (assetImporter, assetPath) => {
                    assetImporter.SetAssetBundleNameAndVariant(string.Format(ASSET_BUNDLE_NAME_FORMAT, Regex.Replace(assetPath.ToLower(), "^assets/", string.Empty)), string.Empty);
                }
            );
        }

        /// <summary>
        /// 設定されている AssetBundleName をクリアする
        /// </summary>
        [MenuItem("Project/AssetBundle/Clear AssetBundleName")]
        public static void ClearAssetBundleNameToSelection() {
            HandleAssetBundleNameToSelection(
                (assetImporter, assetPath) => {
                    assetImporter.SetAssetBundleNameAndVariant(string.Empty, string.Empty);
                }
            );
        }

        /// <summary>
        /// AssetBundleName を操作する
        /// </summary>
        /// <param name="action">実際に設定・解除をする処理</param>
        private static void HandleAssetBundleNameToSelection(Action<AssetImporter, string> action) {
            Object[] selectedAssets = Selection.objects;
            if (selectedAssets == default(Object[]) || selectedAssets.Length == 0) {
                Debug.LogWarning("1つ以上のアセットを選択してください。");
                return;
            }
            IEnumerable<Object> normalizedSelectedAssetList = FlattenSelectedAssets();
            foreach (Object asset in normalizedSelectedAssetList) {
                string assetPath = AssetDatabase.GetAssetPath(asset);
                AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
                action(assetImporter, assetPath);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 選択されているアセットをフラット化したリストを返す
        /// </summary>
        /// <remarks>ディレクトリを選択している場合は配下のアセットにバラして返す</remarks>
        /// <returns>フラット化したアセットのリスト</returns>
        private static IEnumerable<Object> FlattenSelectedAssets() {
            List<Object> assetList = new List<Object>();
            foreach (Object selectedAsset in Selection.objects) {
                string path = AssetDatabase.GetAssetPath(selectedAsset);
                if (AssetDatabase.IsValidFolder(path)) {
                    string[] guids = AssetDatabase.FindAssets(
                        "t:Object",
                        new [] {
                            path,
                        }
                    );
                    foreach (string guid in guids) {
                        // ディレクトリには AssetBundle 名を付与しないためスキップ
                        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                        if (AssetDatabase.IsValidFolder(assetPath)) {
                            continue;
                        }
                        Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                        assetList.Add(asset);
                    }
                } else {
                    assetList.Add(selectedAsset);
                }
            }
            return assetList;
        }

    }

}