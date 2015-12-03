rd /q/s data\server
mkdir data\server
start consul agent -server -bootstrap-expect 1 -data-dir data\server -ui-dir ui
