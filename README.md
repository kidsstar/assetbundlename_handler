# What?

* AssetBundle 名を設定するためのメニューを提供します。
* 設定される名称はアセットのパスをほぼそのまま利用します。

# Why?

* 個別に設定すると typo リスクがあり、また読み込みに失敗するケースが想定されるため、システム的に設定するようにしたかったからです。

# Install

```shell
$ npm install -D @kidsstar/assetbundlename_handler
```

# Usage

## 設定

1. 設定したい対象のアセットを Project View で選択します。
    * ディレクトリを選択した場合は、配下のアセットを全て選択したと見なします。
1. メニューバーの `Project` &gt; `AssetBundle` &gt; `Apply AssetBundleName` を選択します。

## 解除

1. 解除したい対象のアセットを Project View で選択します。
    * ディレクトリを選択した場合は、配下のアセットを全て選択したと見なします。
1. メニューバーの `Project` &gt; `AssetBundle` &gt; `Clear AssetBundleName` を選択します。

# License

Copyright (c) 2017 Tetsuya Mori

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)

