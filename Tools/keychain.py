import sys
import os
import json
import base64

if len(sys.argv) < 2:
    print("Usage: python keychain.py /path/to/fortnite-aes-archive")
    exit()

out = []

archive_path = os.path.join(sys.argv[1], "api", "archive")
file_names = os.listdir(archive_path)
for file_name in file_names:
    data = json.load(open(os.path.join(archive_path, file_name)))

    if "dynamicKeys" in data:
        for key in data["dynamicKeys"]:
            if "key" in key and "guid" in key:
                scnd = base64.b64encode(bytes.fromhex(key["key"][2:])).decode()
                out.append(f"{key["guid"]}:{scnd}")

json.dump(out, open("keychain.json", "w"), indent = 4)
