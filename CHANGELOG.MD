# Changed in this release

* **Improve error logging**: Changed error logging to be less generic and added a exception logging in case of unexpected errors. Thank you @vncastanheira for help and inspiration!
* **Prevent project files overwrite**: Changed so that project file and .ini file are not generated again if target archive exist in same directory as the output directory (in this situation project already can have those files). Thank you @Half-Time-Demon for the issue report!