# Changed in this release

* **Fix [issue #5](https://github.com/uuksu/RPGMakerDecrypter/issues/5)**: Some games created with a operating system supporting wider encoding scheme might not extract correctly on all systems. File paths extracted from the archives are now cleaned before trying to use them when writing a file. Thanks @izayoi256 for reporting the issue!