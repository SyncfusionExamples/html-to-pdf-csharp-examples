echo "Starting dependencies installation script..."

# Ensure rsync is installed
if ! command -v rsync &> /dev/null; then
    echo "rsync could not be found, installing..."
    apt-get update && apt-get install -yq rsync
fi

if [ -d /home/site/wwwroot/Package ]; then
    echo "Package directory exists."
    PACKAGE_USR="/home/site/wwwroot/Package/usr"
    if [ -d "$PACKAGE_USR" ]; then
        echo "Copying user libraries..."
        rsync -av /home/site/wwwroot/Package/usr/lib/x86_64-linux-gnu/ /usr/lib/
    fi
    PACKAGE_LIB="/home/site/wwwroot/Package/lib"
    if [ -d "$PACKAGE_LIB" ]; then
        echo "Removing conflicting libraries..."
        if [ -f /home/site/wwwroot/Package/lib/x86_64-linux-gnu/libc.so.6 ]; then
            rm /home/site/wwwroot/Package/lib/x86_64-linux-gnu/libc.so.6
        fi
        if [ -f /home/site/wwwroot/Package/lib/x86_64-linux-gnu/libc-2.28.so ]; then
            rm /home/site/wwwroot/Package/lib/x86_64-linux-gnu/libc-2.28.so
        fi
        if [ -f /home/site/wwwroot/Package/lib/x86_64-linux-gnu/libselinux.so.1 ]; then
            rm /home/site/wwwroot/Package/lib/x86_64-linux-gnu/libselinux.so.1
        fi
        echo "Copying system libraries..."
        rsync -av /home/site/wwwroot/Package/lib/x86_64-linux-gnu/ /lib/
        ldconfig
    fi
else
    echo "Package directory does not exist. Installing dependencies..."
    apt-get update && apt-get install -yq --no-install-recommends libasound2 libatk1.0-0 libc6 libcairo2 libcups2 libdbus-1-3 libexpat1 libfontconfig1 libgcc1 libgconf-2-4 libgdk-pixbuf2.0-0 libglib2.0-0 libgtk-3-0 libnspr4 libpango-1.0-0 libpangocairo-1.0-0 libstdc++6 libx11-6 libx11-xcb1 libxcb1 libxcursor1 libxdamage1 libxext6 libxfixes3 libxi6 libxrandr2 libxrender1 libxss1 libxtst6 libnss3 libgbm1
    mkdir -p /home/site/wwwroot/Package/usr/lib/x86_64-linux-gnu
    mkdir -p /home/site/wwwroot/Package/lib/x86_64-linux-gnu
    PACKAGE_USR="/home/site/wwwroot/Package/usr"
    if [ -d "$PACKAGE_USR" ]; then
        echo "Copying user libraries to package..."
        rsync -av /usr/lib/x86_64-linux-gnu/ /home/site/wwwroot/Package/usr/lib/
    fi
    PACKAGE_LIB="/home/site/wwwroot/Package/lib"
    if [ -d "$PACKAGE_LIB" ]; then
        echo "Copying system libraries to package..."
        rsync -av /lib/x86_64-linux-gnu/ /home/site/wwwroot/Package/lib/
    fi
fi

echo "Dependencies installation script completed."
