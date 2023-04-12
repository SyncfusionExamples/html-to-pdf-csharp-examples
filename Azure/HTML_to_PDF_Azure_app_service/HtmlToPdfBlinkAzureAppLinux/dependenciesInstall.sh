DIR="/home/site/wwwroot/Package"
if [ -d "$DIR" ]; then
   echo "'$DIR' found and now copying files, please wait ..."
   PACKAGE_USR="/home/site/wwwroot/Package/usr"
	if [ -d "$PACKAGE_USR" ]; then
		cp -r /home/site/wwwroot/Package/usr/lib/x86_64-linux-gnu/ /usr/lib/
	fi
	PACKAGE_LIB="/home/site/wwwroot/Package/lib"
	if [ -d "$PACKAGE_LIB" ]; then
		rm /home/site/wwwroot/Package/lib/x86_64-linux-gnu/libc.so.6;
		rm /home/site/wwwroot/Package/lib/x86_64-linux-gnu/libc-2.28.so;
	    rm /home/site/wwwroot/Package/lib/x86_64-linux-gnu/libselinux.so.1;
		cp -r /home/site/wwwroot/Package/lib/x86_64-linux-gnu/ /lib/;
		ldconfig;
	fi
else
   apt-get update && apt-get install -yq --no-install-recommends libasound2 libatk1.0-0 libc6 libcairo2 libcups2 libdbus-1-3 libexpat1 libfontconfig1 libgcc1 libgconf-2-4 libgdk-pixbuf2.0-0 libglib2.0-0 libgtk-3-0 libnspr4 libpango-1.0-0 libpangocairo-1.0-0 libstdc++6 libx11-6 libx11-xcb1 libxcb1 libxcursor1 libxdamage1 libxext6 libxfixes3 libxi6 libxrandr2 libxrender1 libxss1 libxtst6 libnss3 libgbm1;
   mkdir /home/site/wwwroot/Package;
   mkdir /home/site/wwwroot/Package/usr;
   mkdir /home/site/wwwroot/Package/usr/lib;
   mkdir /home/site/wwwroot/Package/usr/lib/x86_64-linux-gnu;
   mkdir /home/site/wwwroot/Package/lib;
   mkdir /home/site/wwwroot/Package/lib/x86_64-linux-gnu;
   PACKAGE_USR="/home/site/wwwroot/Package/usr"
	if [ -d "$PACKAGE_USR" ]; then
		cp -r /usr/lib/x86_64-linux-gnu/ /home/site/wwwroot/Package/usr/lib/
	fi
	PACKAGE_LIB="/home/site/wwwroot/Package/lib"
	if [ -d "$PACKAGE_LIB" ]; then
		cp -r /lib/x86_64-linux-gnu/ /home/site/wwwroot/Package/lib/
	fi
fi
